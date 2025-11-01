import argparse
import json
import re
import sys
import time
from dataclasses import dataclass
from html import unescape
from typing import List, Optional, Tuple

import requests

SEARCH_UA = (
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
    "(KHTML, like Gecko) Chrome/129.0 Safari/537.36"
)
DDG_HTML = "https://html.duckduckgo.com/html/"

BASE = "https://www.megakheir.com"
DEFAULT_JSON = "scrapers/megakheir/data/megakheir_campaigns_23.json"

# Candidate path templates to try for each external_id (slug)
CANDIDATE_PATTERNS = [
    "/{slug}",
    "/campaigns/{slug}",
    "/donate/{slug}",
    "/donations/{slug}",
    "/charity-category/{slug}",
    "/category/{slug}",
    "/categories/{slug}",
    "/programs/{slug}",
    "/initiatives/{slug}",
]

UA = (
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
    "(KHTML, like Gecko) Chrome/129.0 Safari/537.36"
)


@dataclass
class FixResult:
    title: str
    slug: str
    page_url: Optional[str]
    old: Optional[str]
    new: Optional[str]
    status: str  # updated | unchanged | failed
    note: Optional[str] = None


def http_get(url: str, timeout: int = 15) -> Optional[str]:
    try:
        resp = requests.get(url, headers={"User-Agent": UA}, timeout=timeout)
        if resp.status_code == 200 and resp.text:
            return resp.text
        return None
    except requests.RequestException:
        return None


def absolutize(url: str) -> str:
    url = unescape(url).strip()
    if url.startswith("//"):
        return "https:" + url
    if url.startswith("/"):
        return BASE.rstrip("/") + url
    return url


BG_RE_STRICT = re.compile(
    r"<div[^>]+class=\"[^\"]*cover-section[^\"]*\"[^>]*style=\"[^\"]*background-image\s*:\s*url\((?:&quot;|&apos;|\"|')?([^\)\"'&]+)",
    flags=re.I | re.S,
)
BG_RE_LOOSE = re.compile(
    r"background-image\s*:\s*url\((?:&quot;|&apos;|\"|')?([^\)\"'&]+)",
    flags=re.I,
)


def extract_bg_image(html: str) -> Optional[str]:
    # Prefer the hero cover-section background if present
    m = BG_RE_STRICT.search(html)
    if m:
        return absolutize(m.group(1))
    # Fallback: first background-image on the page
    m2 = BG_RE_LOOSE.search(html)
    if m2:
        return absolutize(m2.group(1))
    return None


def search_megakheir_by_title(title: str, max_results: int = 5) -> List[str]:
    # Use DuckDuckGo HTML to search for pages likely containing the Arabic title
    q = f"site:megakheir.com {title}"
    try:
        resp = requests.post(
            DDG_HTML,
            headers={"User-Agent": SEARCH_UA, "Content-Type": "application/x-www-form-urlencoded"},
            data={"q": q},
            timeout=15,
        )
        if resp.status_code != 200 or not resp.text:
            return []
        html = resp.text
    except requests.RequestException:
        return []

    # Extract candidate links from the results
    links: List[str] = []
    for m in re.finditer(r"<a[^>]+href=\"(https?://[^\"]+)\"", html, flags=re.I):
        href = m.group(1)
        if "megakheir.com" not in href:
            continue
        # Skip NGOS listing pages since we're targeting campaigns/landing pages
        if "/ngos" in href:
            continue
        if href not in links:
            links.append(href)
        if len(links) >= max_results:
            break
    return links


def pick_page_for_slug(slug: str) -> Tuple[Optional[str], Optional[str]]:
    for pat in CANDIDATE_PATTERNS:
        url = BASE.rstrip("/") + pat.format(slug=slug.strip("/"))
        html = http_get(url)
        if not html:
            continue
        img = extract_bg_image(html)
        if img:
            return url, img
        # If we reached a page but didn't find background, still remember URL and keep trying
    return None, None


def pick_page_by_title(title: str) -> Tuple[Optional[str], Optional[str]]:
    # Try searching for a page that contains the Arabic title and a hero background
    for url in search_megakheir_by_title(title):
        html = http_get(url)
        if not html:
            continue
        img = extract_bg_image(html)
        if img:
            return url, img
    return None, None


def load_json(path: str):
    with open(path, "r", encoding="utf-8-sig") as f:
        return json.load(f)


def save_json(path: str, data):
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)


def fix_file(path: str, write: bool = False, delay: float = 0.5) -> List[FixResult]:
    data = load_json(path)
    results: List[FixResult] = []
    changed = False

    for item in data:
        title = item.get("title") or ""
        slug = (item.get("external_id") or "").strip()
        old = item.get("image_url")

        if not slug:
            results.append(FixResult(title=title, slug=slug, page_url=None, old=old, new=None, status="failed", note="missing external_id (slug)"))
            continue

        page_url, new_img = pick_page_for_slug(slug)
        if not page_url or not new_img:
            # Fallback: try to find by Arabic title via search
            page_url, new_img = pick_page_by_title(title)
            if not page_url or not new_img:
                results.append(FixResult(title=title, slug=slug, page_url=page_url, old=old, new=None, status="failed", note="no background-image found"))
                time.sleep(delay)
                continue

        if new_img == old:
            results.append(FixResult(title=title, slug=slug, page_url=page_url, old=old, new=new_img, status="unchanged"))
        else:
            item["image_url"] = new_img
            changed = True
            results.append(FixResult(title=title, slug=slug, page_url=page_url, old=old, new=new_img, status="updated"))

        time.sleep(delay)

    if write and changed:
        save_json(path, data)

    return results


def main(argv: List[str]) -> int:
    ap = argparse.ArgumentParser(description="Fix campaign image_url by scraping hero background-image from campaign pages.")
    ap.add_argument("--file", default=DEFAULT_JSON, help="Path to campaigns JSON file")
    ap.add_argument("--write", action="store_true", help="Persist changes to file (otherwise dry-run)")
    ap.add_argument("--delay", type=float, default=0.5, help="Delay between requests in seconds")
    args = ap.parse_args(argv)

    path = args.file
    results = fix_file(path, write=args.write, delay=args.delay)

    # Pretty print summary
    updated = sum(1 for r in results if r.status == "updated")
    unchanged = sum(1 for r in results if r.status == "unchanged")
    failed = sum(1 for r in results if r.status == "failed")

    print(f"Processed: {len(results)} | updated={updated} unchanged={unchanged} failed={failed}")
    for r in results:
        note = f" ({r.note})" if r.note else ""
        print(f"- [{r.status}] {r.title} | slug={r.slug} | page={r.page_url or '-'} | old={r.old} -> new={r.new}{note}")

    if updated and not args.write:
        print("\nNote: run again with --write to persist updated image_url values.")

    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
