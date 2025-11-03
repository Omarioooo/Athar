# -*- coding: utf-8 -*-
"""
Scrape MegaKheir campaigns and export JSON ready for Athar API import.

- Uses crawl4ai when available; falls back to requests+BeautifulSoup.
- Extracts: title (Arabic preserved), description (cleaned), image_url, supporting_charities (names), external_id.
- Fills numeric/date fields with sensible random values (no nulls); is_critical always false unless overridden.
- Removes external_website_url from output per requirement.
- De-duplicates by normalized title and keeps ~30 max.

Usage (Windows PowerShell):
    py -3 scrapers/megakheir/scrape_megakheir_campaigns_crawl4ai.py --out scrapers/megakheir/data/megakheir_campaigns.json --validate-images

Options:
    --api-base http://localhost:5192   # to fetch 101 charities and filter supporting_charities to existing only
"""
from __future__ import annotations
import argparse
import json
import os
import random
import re
import sys
import time
from dataclasses import dataclass, asdict
from datetime import datetime
from typing import List, Optional, Dict, Any, Set

try:
    # Optional dependency
    from crawl4ai import WebCrawler
except Exception:
    WebCrawler = None  # type: ignore

try:
    import requests
    from bs4 import BeautifulSoup
except Exception:
    requests = None  # type: ignore
    BeautifulSoup = None  # type: ignore

URL = "https://www.megakheir.com/"
MAX_PAGES = 40

@dataclass
class CampaignItem:
    title: Optional[str] = None
    description: Optional[str] = None
    image_url: Optional[str] = None
    supporting_charities: Optional[List[str]] = None
    charity_name: Optional[str] = None
    # Optional fields the API may randomize if None
    goal_amount: Optional[float] = None
    raised_amount: Optional[float] = None
    is_critical: Optional[bool] = None
    start_date: Optional[str] = None  # ISO date
    duration_days: Optional[int] = None
    category: Optional[str] = None
    external_id: Optional[str] = None


def abs_url(base: str, src: str) -> str:
    if not src:
        return src
    if src.startswith("http://") or src.startswith("https://"):
        return src
    if src.startswith("//"):
        return "https:" + src
    return base.rstrip("/") + "/" + src.lstrip("/")


def clean_text(s: str) -> str:
    if not s:
        return s
    s = re.sub(r"\s+", " ", s, flags=re.UNICODE).strip()
    # Keep Arabic as-is; remove weird control chars
    s = s.replace("\u200f", "").replace("\u200e", "")
    return s


def normalize_title(t: str) -> str:
    t = (t or "").strip().lower()
    t = re.sub(r"\s+", " ", t)
    return t


def scrape_with_bs4(base_url: str) -> List[CampaignItem]:
    if not requests or not BeautifulSoup:
        print("BeautifulSoup/requests not installed; skipping fallback.")
        return []

    resp = requests.get(base_url, timeout=30)
    # Improve Arabic decoding
    try:
        resp.encoding = resp.apparent_encoding or resp.encoding
    except Exception:
        pass
    resp.raise_for_status()
    soup = BeautifulSoup(resp.text, "lxml")

    items: List[CampaignItem] = []

    # Heuristic: find prominent cards/links that look like campaigns
    # First pass: visible cards on landing page
    for card in soup.select("section, article, div.card, div, a"):
        title_el = None
        for sel in ["h2", "h3", ".card-title", ".title"]:
            title_el = card.select_one(sel)
            if title_el:
                break
        img_el = None
        for sel in ["img"]:
            img_el = card.select_one(sel)
            if img_el:
                break
        if not title_el or not img_el:
            continue
        title = clean_text(title_el.get_text(" ", strip=True))
        if not title or len(title) < 3:
            continue
        img_src = img_el.get("src") or img_el.get("data-src") or ""
        image_url = abs_url(base_url, img_src)

        # Description fallback: use alt or surrounding text
        desc = None
        if img_el.get("alt"):
            desc = img_el.get("alt")
        if not desc:
            desc = title

        items.append(CampaignItem(title=title, description=clean_text(desc), image_url=image_url))

    # Second pass: follow a subset of internal links for more detailed pages
    anchors = [a.get("href") for a in soup.select("a[href]")]
    seen: Set[str] = set()
    followed = 0
    for href in anchors:
        if not href:
            continue
        u = abs_url(base_url, href)
        if not u.startswith(base_url.rstrip("/")):
            continue
        if u in seen:
            continue
        seen.add(u)
        if any(ext in u.lower() for ext in [".pdf", ".zip", "#", "tel:", "mailto:"]):
            continue
        try:
            r = requests.get(u, timeout=20)
            try:
                r.encoding = r.apparent_encoding or r.encoding
            except Exception:
                pass
            if r.status_code != 200:
                continue
            s = BeautifulSoup(r.text, "lxml")
        except Exception:
            continue

        block = None
        for sel in ["main", "article", "section", "div.container", "body"]:
            block = s.select_one(sel)
            if block:
                break
        if not block:
            block = s

        title_el = None
        for sel in ["h1", "h2", ".title", ".page-title", "[class*=title]"]:
            title_el = block.select_one(sel)
            if title_el:
                break
        img_el = block.select_one("img")
        if not title_el or not img_el:
            continue
        title = clean_text(title_el.get_text(" ", strip=True))
        if not title or len(title) < 3:
            continue
        img_src = img_el.get("src") or img_el.get("data-src") or ""
        image_url = abs_url(base_url, img_src)
        desc = block.get_text(" ", strip=True)
        if desc and len(desc) > 2000:
            desc = desc[:2000]
        items.append(CampaignItem(title=title, description=clean_text(desc), image_url=image_url))

        followed += 1
        if followed >= MAX_PAGES:
            break

    # Deduplicate by normalized title
    unique: Dict[str, CampaignItem] = {}
    for it in items:
        key = normalize_title(it.title or "")
        if not key:
            continue
        if key not in unique:
            unique[key] = it
    return list(unique.values())


def scrape_with_crawl4ai(base_url: str) -> List[CampaignItem]:
    if WebCrawler is None:
        print("crawl4ai not installed; skipping crawl4ai scrape.")
        return []
    crawler = WebCrawler()
    urls = [base_url]
    results: List[CampaignItem] = []
    for u in urls:
        try:
            page = crawler.run(u)
        except Exception as e:
            print(f"crawl error: {e}")
            continue
        # naive parse: look for images with nearby headings
        html = page.html or ""
        soup = None
        if BeautifulSoup:
            soup = BeautifulSoup(html, "lxml")
        else:
            continue
        for block in soup.select("section, article, div"):
            title_el = None
            for sel in ["h1", "h2", "h3", ".title", ".card-title"]:
                title_el = block.select_one(sel)
                if title_el:
                    break
            img_el = block.select_one("img")
            if not title_el or not img_el:
                continue
            title = clean_text(title_el.get_text(" ", strip=True))
            if not title or len(title) < 3:
                continue
            img_src = img_el.get("src") or img_el.get("data-src") or ""
            image_url = abs_url(base_url, img_src)
            desc = block.get_text(" ", strip=True)
            # trim overly long
            if desc and len(desc) > 2000:
                desc = desc[:2000]
            results.append(CampaignItem(title=title, description=clean_text(desc), image_url=image_url))

    # Deduplicate
    uniq: Dict[str, CampaignItem] = {}
    for r in results:
        key = normalize_title(r.title or "")
        if key and key not in uniq:
            uniq[key] = r
    return list(uniq.values())


def validate_image(url: str) -> bool:
    if not requests:
        return True
    try:
        r = requests.get(url, timeout=15, stream=True)
        ct = r.headers.get("Content-Type", "")
        return r.status_code == 200 and ("image" in ct)
    except Exception:
        return False


def fetch_existing_charities(api_base: Optional[str]) -> Set[str]:
    names: Set[str] = set()
    if not api_base or not requests:
        return names
    try:
        r = requests.get(f"{api_base.rstrip('/')}/api/Charities", params={"page": 1, "pageSize": 200}, timeout=15)
        if r.status_code != 200:
            return names
        data = r.json()
        items = data.get("items") or []
        for it in items:
            n = (it.get("name") or "").strip()
            if n:
                names.add(n)
    except Exception:
        pass
    return names


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--base-url", default=URL)
    ap.add_argument("--out", default=os.path.join("scrapers", "megakheir", "data", f"megakheir_campaigns_{datetime.now():%Y%m%d_%H%M%S}.json"))
    ap.add_argument("--validate-images", action="store_true")
    ap.add_argument("--api-base", default=None, help="Athar API base URL to fetch charity names for linking")
    args = ap.parse_args()

    os.makedirs(os.path.dirname(args.out), exist_ok=True)

    items: List[CampaignItem] = []
    # Prefer crawl4ai then fallback
    items = scrape_with_crawl4ai(args.base_url) or scrape_with_bs4(args.base_url)

    # Load existing charity names (for filtering supporting_charities, if we can detect any)
    existing_names = fetch_existing_charities(args.api_base)

    # Post-process: ensure minimal fields
    clean_items: List[Dict[str, Any]] = []
    rng = random.Random(42)
    for it in items:
        if not it.title or not it.image_url:
            continue
        if args.validate_images and not validate_image(it.image_url):
            continue
        # Fill required fields per spec
        if it.goal_amount is None:
            it.goal_amount = float(rng.randrange(50_000, 500_000, 500))
        if it.raised_amount is None:
            it.raised_amount = round(it.goal_amount * rng.uniform(0.2, 0.8), 2)
        if it.duration_days is None:
            it.duration_days = rng.randint(20, 60)
        if it.start_date is None:
            # ISO date (no time) within last 60 days
            start = datetime.utcnow().date().toordinal() - rng.randint(0, 60)
            it.start_date = datetime.fromordinal(start).date().isoformat()
        it.is_critical = False

        # Try to infer supporting charity by presence of known names in description
        if existing_names and not it.supporting_charities:
            desc_l = (it.description or "").lower()
            found = []
            for n in existing_names:
                try:
                    if n and n in (it.title or ""):
                        found.append(n)
                    elif n and (n.lower() in desc_l):
                        found.append(n)
                except Exception:
                    continue
            if found:
                # Unique, keep order
                seen_names: Set[str] = set()
                uniq = []
                for n in found:
                    if n not in seen_names:
                        uniq.append(n)
                        seen_names.add(n)
                it.supporting_charities = uniq[:3]

        d = asdict(it)
        # Remove external_website_url if present
        d.pop("external_website_url", None)
        clean_items.append(d)

    # Heuristic: keep top ~30 items
    if len(clean_items) > 30:
        clean_items = clean_items[:30]

    with open(args.out, "w", encoding="utf-8") as f:
        json.dump(clean_items, f, ensure_ascii=False, indent=2)
    print(f"Wrote {len(clean_items)} items -> {args.out}")


if __name__ == "__main__":
    main()
