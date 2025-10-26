import os
import re
import json
import math
import asyncio
import logging
from datetime import datetime, timezone
from dataclasses import dataclass, field
from typing import List, Optional, Dict, Any, Tuple

# Optional: load .env variables if python-dotenv is installed
try:
    from dotenv import load_dotenv  # type: ignore
    load_dotenv()
except Exception:
    pass

from tenacity import retry, stop_after_attempt, wait_exponential, retry_if_exception_type

try:
    from crawl4ai import AsyncWebCrawler, BrowserConfig, CrawlerRunConfig, CacheMode  # type: ignore
    _HAS_CRAWL4AI = True
except Exception:
    _HAS_CRAWL4AI = False

try:
    from groq import Groq  # type: ignore
except Exception:
    Groq = None  # type: ignore

logging.basicConfig(level=logging.INFO, format='[%(levelname)s] %(message)s')
logger = logging.getLogger("megakheir-c4ai")

BASE_URL = "https://www.megakheir.com"
LIST_URL = f"{BASE_URL}/ngos"
PAGE_PARAM = "755ebc3a_page"

SOCIAL_HOSTS = {
    "facebook.com": "facebook",
    "fb.com": "facebook",
    "instagram.com": "instagram",
    "x.com": "twitter",
    "twitter.com": "twitter",
    "youtube.com": "youtube",
    "tiktok.com": "tiktok",
    "linkedin.com": "linkedin",
}

@dataclass
class CharityRecord:
    name_ar: str
    megakheir_slug: str
    megakheir_url: str
    official_website: Optional[str] = None
    description_ar: Optional[str] = None
    email: Optional[str] = None
    phone: Optional[str] = None
    socials: Dict[str, str] = field(default_factory=dict)
    image_url: Optional[str] = None
    source: str = BASE_URL
    scraped_at: str = field(default_factory=lambda: datetime.now(timezone.utc).isoformat())

    def to_db_shape(self) -> Dict[str, Any]:
        # Clean schema: remove Email, Socials, Source, ScrapedAt per requirement
        return {
            "Name": self.name_ar,
            "Description": self.description_ar or "",
            "PhoneNumber": self.phone,
            "ExternalWebsiteUrl": self.official_website,
            "MegaKheirUrl": self.megakheir_url,
            "ImageUrl": self.image_url,
        }

    def to_slim(self) -> Dict[str, Any]:
        return {
            "name": self.name_ar,
            "description": self.description_ar or "",
            "image_url": self.image_url,
            "external_website_url": self.official_website,
        }

class GroqExtractor:
    def __init__(self, api_key: Optional[str] = None, model: Optional[str] = None, enabled: bool = True):
        self.client = None
        if not enabled:
            self.api_key = None
            self.model = None
            return

        self.api_key = api_key or os.getenv("GROQ_API_KEY")
        self.model = model or os.getenv("GROQ_MODEL", "llama3-70b-8192")
        if self.api_key and Groq is not None:
            try:
                self.client = Groq(api_key=self.api_key)
                logger.info("Groq client initialized; model=%s", self.model)
            except Exception as e:
                logger.warning("Failed to init Groq client: %s", e)
                self.client = None

    def available(self) -> bool:
        return self.client is not None

    @retry(stop=stop_after_attempt(3), wait=wait_exponential(multiplier=1, min=1, max=8),
           retry=retry_if_exception_type(Exception))
    def extract_links(self, text_snippet: str) -> Dict[str, Any]:
        if not self.available():
            return {}
        system = (
            "أنت مساعد ذكي لاستخراج الروابط. من النص التالي (Markdown/نصي)، استخرج إن وجد:" \
            " رابط الموقع الرسمي للجمعية (خارجي وليس على megakheir.com), البريد الإلكتروني، رقم الهاتف، وروابط الشبكات الاجتماعية." \
            " أعد JSON فقط بالشكل التالي:\n" \
            "{\n  \"official_website\": string|null,\n  \"email\": string|null,\n  \"phone\": string|null,\n  \"socials\": {\n    \"facebook\": string|null, \"instagram\": string|null, \"twitter\": string|null, \"youtube\": string|null, \"tiktok\": string|null, \"linkedin\": string|null\n  }\n}"
        )
        prompt = text_snippet[:6000]
        try:
            resp = self.client.chat.completions.create(
                model=self.model,
                temperature=0,
                messages=[
                    {"role": "system", "content": system},
                    {"role": "user", "content": prompt},
                ],
            )
            content = resp.choices[0].message.content  # type: ignore
            content = re.sub(r"^```[a-zA-Z]*\n|```$", "", content.strip())
            return json.loads(content)
        except Exception as e:
            logger.warning("Groq extraction failed: %s", e)
            return {}

class Crawl4AIScraper:
    def __init__(self, use_llm: bool = True):
        if not _HAS_CRAWL4AI:
            raise RuntimeError("crawl4ai is not installed. Please pip install crawl4ai and run crawl4ai-setup.")
        self.llm = GroqExtractor(enabled=use_llm)
        self.browser_config = BrowserConfig(verbose=False, headless=True)
        self.run_config = CrawlerRunConfig(cache_mode=CacheMode.BYPASS)
        self.use_search = os.getenv("USE_SEARCH", "1").strip() == "1"

    async def _fetch_markdown(self, crawler: AsyncWebCrawler, url: str) -> str:
        result = await crawler.arun(url=url, config=self.run_config)
        md = getattr(result, "markdown", None)
        if hasattr(md, "raw_markdown"):
            return md.raw_markdown
        return md if isinstance(md, str) else ""

    def _find_pagination_from_markdown(self, md: str) -> int:
        max_page = 1
        for m in re.finditer(r"\(https://www\\.megakheir\\.com/ngos\\?" + PAGE_PARAM + r"=(\\d+)\)", md):
            try:
                p = int(m.group(1))
                if p > max_page:
                    max_page = p
            except Exception:
                pass
        total_match = re.search(r"إظهار\s+(\d+)\s+من\s+(\d+)\s+جمعيات", md)
        if total_match:
            try:
                total = int(total_match.group(2))
                approx = int(math.ceil(total / 20.0))
                max_page = max(max_page, approx)
            except Exception:
                pass
        return max_page

    def _parse_listing_from_markdown(self, md: str) -> List[Tuple[str, str]]:
        urls: List[str] = []
        for m in re.finditer(r"(?:https://www\\.megakheir\\.com)?(/ngos/[A-Za-z0-9\-]+)", md, flags=re.I):
            path = m.group(1)
            if "/donate/" in path:
                continue
            url = BASE_URL + path
            if url not in urls:
                urls.append(url)
        pairs: List[Tuple[str, str]] = []
        for url in urls:
            inline = re.search(rf"\[([^\]]+)\]\(({re.escape(url)}|{re.escape(url.replace(BASE_URL, ''))})\)", md)
            name = inline.group(1).strip() if inline else None
            if not name:
                pos = md.find(url)
                window = md[max(0, pos - 600):pos]
                candidates = re.findall(r"^#{1,6}\s+(.+)$", window, flags=re.M)
                candidates = [c.strip() for c in candidates if re.search(r"[\u0600-\u06FF]", c)]
                if candidates:
                    name = candidates[-1]
                else:
                    lines = [ln.strip() for ln in window.splitlines() if ln.strip()]
                    arabic_lines = [ln for ln in lines if re.search(r"[\u0600-\u06FF]", ln) and not ln.startswith("[")]
                    if arabic_lines:
                        name = arabic_lines[-1]
            if name:
                pairs.append((name, url))
        seen: Dict[str, str] = {}
        for name, url in pairs:
            seen.setdefault(url, name)
        return [(n, u) for u, n in seen.items()]

    def _classify_link(self, url: str) -> Optional[Tuple[str, str]]:
        try:
            host = re.sub(r"^https?://", "", url).split("/")[0].lower()
        except Exception:
            return None
        bare_host = host[4:] if host.startswith("www.") else host
        blocked_hosts = {
            "play.google.com", "apps.apple.com",
            "tatelecom.com", "tatelecom.cc", "accept.paymob.com", "paymob.com",
        }
        blocked_suffixes = (".megakheir.com", ".webflow.io", ".website-files.com")
        blocked_websites = {
            # Search engines / wrappers
            "duckduckgo.com", "html.duckduckgo.com", "google.com", "bing.com", "search.yahoo.com",
            # Link aggregators / directories (non-official)
            "linktr.ee", "linktree.com", "yellowpages.com.eg", "logodalil.com.eg",
            "dawa.center", "bizmideast.com", "findhealthclinics.com", "egyptencyclopedia.com",
            "elmqal.com", "vymaps.com", "findglocal.com", "foursquare.com", "yallapages.com",
            "egypt-business.com", "maps.google.com", "g.page", "goo.gl", "bit.ly",
            # Donation/marketplace platforms (avoid as official)
            "ensany.com",
        }
        if ("megakheir.com" in bare_host or bare_host.endswith(blocked_suffixes) or bare_host in blocked_hosts):
            return None
        for bh in blocked_websites:
            if bare_host == bh or bare_host.endswith("." + bh):
                return None
        if re.search(r"\.(?:webp|avif|svg|png|jpe?g|gif|ico)(?:\?|#|$)", url, flags=re.I):
            return None
        for k, v in SOCIAL_HOSTS.items():
            if k in host:
                return (v, url)
        return ("website", url)

    def _clean_paragraphs(self, paras: List[str]) -> List[str]:
        cleaned: List[str] = []
        cta_kw = ["تـــابع", "تابع", "تبرع", "Donate", "Follow", "سجل", "اشترك"]

        def strip_cta_anchors(text: str) -> str:
            def repl(m: re.Match) -> str:
                anchor = (m.group(1) or "").strip()
                return "" if any(kw in anchor for kw in cta_kw) else m.group(0)
            return re.sub(r"\[([^\]]+)\]\(([^)]+)\)", repl, text)

        for p in paras:
            s = strip_cta_anchors(p.strip())
            if not s or s == "جمعية موثقة":
                continue

            lines = [ln.rstrip() for ln in s.splitlines() if ln.strip()]
            if lines and lines[0].lstrip().startswith("#"):
                heading_text = re.sub(r"^#+\s*", "", lines[0].lstrip())
                if re.search(r"[\u0600-\u06FF]", heading_text) and not any(kw in heading_text for kw in cta_kw):
                    cleaned.append(heading_text)
                lines = lines[1:]

            buf: List[str] = []
            def flush_buf():
                if buf:
                    merged_local = " ".join([b for b in buf if b.strip() != "جمعية موثقة"]).strip()
                    buf.clear()
                    if re.search(r"[\u0600-\u06FF]", merged_local) and len(merged_local) >= 15:
                        cleaned.append(merged_local)

            for ln in lines:
                lns = ln.lstrip()
                if lns.startswith("#"):
                    flush_buf()
                    heading_text = re.sub(r"^#+\s*", "", lns)
                    if re.search(r"[\u0600-\u06FF]", heading_text) and not any(kw in heading_text for kw in cta_kw):
                        cleaned.append(heading_text)
                    continue
                if re.match(r"^\s*(?:[-*•]\s+).+", ln) and re.search(r"[\u0600-\u06FF]", ln):
                    flush_buf()
                    cleaned.append(ln.strip())
                    continue
                if re.match(r"^\[[^\]]+\]\(.*\)$", ln):
                    anchor = re.sub(r"\]\(.*\)$", "", ln).replace("[", "").strip()
                    if any(kw in anchor for kw in cta_kw):
                        continue
                buf.append(ln)

            flush_buf()

        return cleaned

    def _extract_desc_and_links_from_markdown(self, md: str, title: Optional[str] = None) -> Tuple[Optional[str], Optional[str], Dict[str, str], Optional[str], Optional[str]]:
        scope = md
        h1_match = re.search(r"^#\s+(.+)$", md, flags=re.M)
        if h1_match and re.search(r"[\u0600-\u06FF]", h1_match.group(1)):
            start = h1_match.end()
            boundaries = [
                r"\n###\s+أوجه التبرع",
                r"\n###\s+أنشطة دائمة",
                r"\n###\s+حملة رمضان",
                r"\n###\s+حملة صك الأضحية",
                r"\n###\s+ميجا خير الشريك الرسمي",
                r"\n##\s+أخبار وفاعليات",
                r"\n###\s+سجل في نشرة",
                r"\n###\s+طرق التبرع",
            ]
            end = len(md)
            for pat in boundaries:
                m = re.search(pat, md[start:])
                if m:
                    cand_end = start + m.start()
                    end = min(end, cand_end)
            scope_candidate = md[start:end]
            paras_candidate = [p.strip() for p in re.split(r"\n\s*\n", scope_candidate) if p.strip()]
            cleaned_candidate = self._clean_paragraphs(paras_candidate)
            if len(cleaned_candidate) >= 1:
                scope = scope_candidate
        paragraphs = [p.strip() for p in re.split(r"\n\s*\n", scope) if p.strip()]
        paragraphs = self._clean_paragraphs(paragraphs)
        desc = None
        if paragraphs:
            acc: List[str] = []
            total = 0
            for p in paragraphs:
                if total + len(p) > 2800:
                    break
                acc.append(p)
                total += len(p)
                if len(acc) >= 20:
                    break
            desc = "\n\n".join(acc)

        website = None
        socials: Dict[str, str] = {}
        email = None
        phone = None

        for src in (scope, md):
            for m in re.finditer(r"\[([^\]]+)\]\((https?://[^)]+)\)", src):
                href = m.group(2).strip()
                if href.startswith("mailto:"):
                    email = email or href.split(":", 1)[1]
                    continue
                if href.startswith("tel:"):
                    phone = phone or re.sub(r"[^+\d]", "", href.replace("tel:", ""))
                    continue
                cls = self._classify_link(href)
                if not cls:
                    continue
                kind, url = cls
                if kind == "website":
                    anchor_text = (m.group(1) or "").strip().lower()
                    anchor_is_site = any(kw in anchor_text for kw in ["الموقع", "الرسمي", "website", "site"])
                    has_reasonable_tld = bool(re.search(r"\.(com|org|net|eg|ngo)(?:/|$)", url))
                    if not website and (anchor_is_site or has_reasonable_tld):
                        website = url
                else:
                    socials.setdefault(kind, url)
            if website:
                break

        if not website and self.llm.available():
            snippet = "\n".join(paragraphs[:40])
            llm_res = self.llm.extract_links(snippet)
            if isinstance(llm_res, dict):
                website = llm_res.get("official_website") or website
                email = llm_res.get("email") or email
                phone = llm_res.get("phone") or phone
                socials_from_llm = llm_res.get("socials") or {}
                for k, v in socials_from_llm.items():
                    if v and k not in socials:
                        socials[k] = v

        if website and any(s in website for s in ("facebook.com", "instagram.com", "twitter.com", "x.com")):
            website = None

        return desc, website, socials, email, phone

    def _extract_image_from_markdown(self, md: str, title: Optional[str]) -> Optional[str]:
        images = []
        for m in re.finditer(r"!\[[^\]]*\]\((https?://[^)]+)\)", md):
            images.append((m.group(0), m.group(1)))
        if not images:
            return None
        if title:
            for full, url in images[:10]:
                alt_m = re.match(r"!\[([^\]]*)\]", full)
                alt = alt_m.group(1) if alt_m else ""
                if alt and any(tok for tok in title.split() if tok in alt):
                    return url
        return images[0][1]

    async def list_all_pages(self) -> List[str]:
        async with AsyncWebCrawler(config=self.browser_config) as crawler:
            md = await self._fetch_markdown(crawler, LIST_URL)
            max_page = self._find_pagination_from_markdown(md)
            pages = [LIST_URL] + [f"{LIST_URL}?{PAGE_PARAM}={i}" for i in range(2, max_page + 1)]
            consecutive_empty = 0
            i = max_page + 1
            while True:
                test_url = f"{LIST_URL}?{PAGE_PARAM}={i}"
                try:
                    tmd = await self._fetch_markdown(crawler, test_url)
                except Exception:
                    break
                items = self._parse_listing_from_markdown(tmd)
                if items:
                    pages.append(test_url)
                    consecutive_empty = 0
                else:
                    consecutive_empty += 1
                    if consecutive_empty >= 2:
                        break
                i += 1
            return pages

    def _rank_site(self, u: str) -> int:
        u_l = u.lower()
        if re.search(r"\.org\.eg(?:/|$)", u_l):
            return 0
        if re.search(r"\.ngo(?:/|$)", u_l):
            return 1
        if re.search(r"\.eg(?:/|$)", u_l):
            return 2
        if re.search(r"\.org(?:/|$)", u_l) and ("egypt" in u_l or ".eg" in u_l or "-eg" in u_l):
            return 2
        if re.search(r"\.com(?:/|$)", u_l) and ("egypt" in u_l or "-eg" in u_l):
            return 4
        return 5

    def _decode_duckduckgo(self, href: str) -> str:
        if re.search(r"^https?://duckduckgo\.com/", href):
            m_ud = re.search(r"[?&]uddg=([^&]+)", href)
            if m_ud:
                try:
                    import urllib.parse as _up
                    return _up.unquote(m_ud.group(1))
                except Exception:
                    return href
        return href

    def _good_tld(self, href: str) -> bool:
        bad_tlds = (".sa", ".qa", ".ae", ".kw", ".bh")
        host = re.sub(r"^https?://", "", href).split("/")[0].lower()
        if any(host.endswith(t) for t in bad_tlds):
            return False
        return True

    async def _search_official_site(self, crawler: AsyncWebCrawler, name_ar: str) -> Optional[str]:
        if not self.use_search:
            return None
        q = name_ar + " الموقع الرسمي"
        url = "https://html.duckduckgo.com/html/?q=" + re.sub(r"\s+", "+", q)
        try:
            md = await self._fetch_markdown(crawler, url)
        except Exception:
            return None
        candidates: List[str] = []
        for m in re.finditer(r"\[([^\]]+)\]\((https?://[^)]+)\)", md):
            href = self._decode_duckduckgo(m.group(2))
            cls = self._classify_link(href)
            if not cls:
                continue
            kind, u = cls
            if kind == "website":
                candidates.append(u)
        candidates = list(dict.fromkeys(candidates))
        candidates.sort(key=self._rank_site)
        name_tokens = [t for t in re.split(r"\s+", name_ar) if len(t) >= 2 and t not in ("جمعية", "مؤسسة", "الخيرية", "الخيرية.", "مصر")]
        for cand in candidates[:5]:
            if not self._good_tld(cand):
                continue
            try:
                md2 = await self._fetch_markdown(crawler, cand)
            except Exception:
                continue
            hits = sum(1 for t in name_tokens if t in md2)
            if hits >= 1:
                return cand
        for cand in candidates:
            if self._good_tld(cand):
                return cand
        return None

    async def scrape(self) -> List[CharityRecord]:
        pages = await self.list_all_pages()
        records: Dict[str, CharityRecord] = {}
        async with AsyncWebCrawler(config=self.browser_config) as crawler:
            for idx, page_url in enumerate(pages, start=1):
                logger.info("Processing page %s/%s: %s", idx, len(pages), page_url)
                try:
                    md = await self._fetch_markdown(crawler, page_url)
                except Exception as e:
                    logger.warning("Failed to fetch %s: %s", page_url, e)
                    continue
                items = self._parse_listing_from_markdown(md)
                logger.info("Found %d NGOs on listing page", len(items))
                for name, detail_url in items:
                    slug = detail_url.rstrip("/").split("/")[-1]
                    if slug in records:
                        continue
                    try:
                        dmd = await self._fetch_markdown(crawler, detail_url)
                    except Exception as e:
                        logger.warning("Failed detail %s: %s", detail_url, e)
                        continue
                    for m in re.finditer(r"^#\s+(.+)$", dmd, flags=re.M):
                        nm = m.group(1).strip()
                        if re.search(r"[\u0600-\u06FF]", nm):
                            name = nm
                            break
                    desc, website, socials, email, phone = self._extract_desc_and_links_from_markdown(dmd, name)
                    if not website:
                        website = await self._search_official_site(crawler, name)
                    image_url = self._extract_image_from_markdown(dmd, name)
                    rec = CharityRecord(
                        name_ar=name,
                        megakheir_slug=slug,
                        megakheir_url=detail_url,
                        official_website=website,
                        description_ar=desc,
                        email=email,
                        phone=phone,
                        socials=socials,
                        image_url=image_url,
                    )
                    records[slug] = rec
                    logger.info("Captured: %s | website=%s", name, website)
                    await asyncio.sleep(0.1)
        return list(records.values())

    async def scrape_single(self, slug_or_url: str) -> List[CharityRecord]:
        if slug_or_url.startswith("http://") or slug_or_url.startswith("https://"):
            detail_url = slug_or_url
        else:
            detail_url = f"{BASE_URL}/ngos/{slug_or_url.strip('/')}"

        async with AsyncWebCrawler(config=self.browser_config) as crawler:
            try:
                dmd = await self._fetch_markdown(crawler, detail_url)
            except Exception as e:
                logger.warning("Failed detail %s: %s", detail_url, e)
                return []
            try:
                os.makedirs("data", exist_ok=True)
                with open(os.path.join("data", f"debug_{slug_or_url.replace('/', '_')}.md"), "w", encoding="utf-8") as _f:
                    _f.write(dmd)
            except Exception:
                pass
            name = slug_or_url
            for m in re.finditer(r"^#\s+(.+)$", dmd, flags=re.M):
                nm = m.group(1).strip()
                if re.search(r"[\u0600-\u06FF]", nm):
                    name = nm
                    break
            desc, website, socials, email, phone = self._extract_desc_and_links_from_markdown(dmd, name)
            if not website:
                async with AsyncWebCrawler(config=self.browser_config) as crawler2:
                    website = await self._search_official_site(crawler2, name)
            image_url = self._extract_image_from_markdown(dmd, name)
            rec = CharityRecord(
                name_ar=name,
                megakheir_slug=detail_url.rstrip("/").split("/")[-1],
                megakheir_url=detail_url,
                official_website=website,
                description_ar=desc,
                email=email,
                phone=phone,
                socials=socials,
                image_url=image_url,
            )
            return [rec]


def _safe_filename_from_name(name: str) -> str:
    # Keep Arabic letters, word chars, space, hyphen, dot. Replace the rest with underscore.
    s = re.sub(r"[^\w\u0600-\u06FF\-\. ]+", "_", name, flags=re.UNICODE)
    s = re.sub(r"\s+", " ", s).strip()
    if not s:
        s = "charity"
    # Windows reserved characters/names handled by replacing colon-like characters already.
    return s[:180]


def save_outputs(rows: List[CharityRecord], out_dir: str = "data") -> Tuple[str, str, str]:
    os.makedirs(out_dir, exist_ok=True)
    per_dir = os.path.join(out_dir, "charities")
    os.makedirs(per_dir, exist_ok=True)

    ts = datetime.now(timezone.utc).strftime("%Y%m%d_%H%M%S")
    json_path = os.path.join(out_dir, f"megakheir_ngos_crawl4ai_{ts}.json")
    xlsx_path = os.path.join(out_dir, f"megakheir_ngos_crawl4ai_{ts}.xlsx")
    slim_path = os.path.join(out_dir, f"megakheir_ngos_slim_{ts}.json")

    data = [r.to_db_shape() for r in rows]
    slim = [r.to_slim() for r in rows]

    # Combined JSON (clean schema)
    with open(json_path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    # Slim JSON
    with open(slim_path, "w", encoding="utf-8") as f:
        json.dump(slim, f, ensure_ascii=False, indent=2)
    # Per-charity JSON by name
    for rec in rows:
        fname = _safe_filename_from_name(rec.name_ar) + ".json"
        with open(os.path.join(per_dir, fname), "w", encoding="utf-8") as cf:
            json.dump(rec.to_db_shape(), cf, ensure_ascii=False, indent=2)

    # Excel export (clean schema)
    try:
        import pandas as pd
        df = pd.DataFrame(data)
        df.to_excel(xlsx_path, index=False)
    except Exception as e:
        logger.warning("Excel export failed: %s", e)
        xlsx_path = ""

    logger.info("Saved JSON -> %s", json_path)
    logger.info("Saved Slim JSON -> %s", slim_path)
    if xlsx_path:
        logger.info("Saved Excel -> %s", xlsx_path)
    return json_path, xlsx_path, slim_path


async def _amain():
    use_llm = os.getenv("USE_LLM", "0").strip() == "1"
    scraper = Crawl4AIScraper(use_llm=use_llm)
    single = os.getenv("SINGLE_SLUG") or os.getenv("SINGLE_URL")
    if single:
        rows = await scraper.scrape_single(single)
    else:
        rows = await scraper.scrape()
    save_outputs(rows)

if __name__ == "__main__":
    if not _HAS_CRAWL4AI:
        raise SystemExit("crawl4ai not installed. pip install -r requirements.txt and run crawl4ai-setup.")
    asyncio.run(_amain())
