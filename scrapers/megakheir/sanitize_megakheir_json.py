#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Sanitize megakheir scraped JSON to ensure URLs and text are safe for import.
- Validates and normalizes fields: name, description, image_url, external_website_url, megakheir_url
- Drops entries missing a name
- Sets malformed URLs to null
- Writes a *_clean.json alongside the original

Usage:
    python sanitize_megakheir_json.py [input_json]

If input_json is omitted, defaults to:
    ./data/megakheir_ngos_slim_20251026_023532.json
"""
from __future__ import annotations
import json
import os
import re
import sys
from urllib.parse import urlparse, urlunparse, quote

# Accept common image extensions we expect to render in UI
IMAGE_EXTENSIONS = {".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".avif", ".svg", ".jfif"}

# Keys used in the JSON file
KEY_NAME = "name"
KEY_DESC = "description"
KEY_IMAGE = "image_url"
KEY_WEBSITE = "external_website_url"
KEY_MEGA = "megakheir_url"


def is_valid_url(url: str) -> bool:
    try:
        p = urlparse(url)
        if p.scheme not in ("http", "https"):
            return False
        if not p.netloc:
            return False
        return True
    except Exception:
        return False


def is_image_url(url: str) -> bool:
    path = urlparse(url).path.lower()
    for ext in IMAGE_EXTENSIONS:
        if path.endswith(ext):
            return True
    return False


def normalize_url(url: str, *, require_image: bool = False) -> str | None:
    if not url:
        return None
    # Trim whitespace and enclosing quotes if any accidental copy paste
    s = url.strip().strip("'\"")
    # Replace spaces in URL with %20 to avoid broken links
    s = s.replace(" ", "%20")
    # Remove stray backslashes that sometimes sneak in from scraped sources
    # but preserve typical escape sequences already resolved by JSON loader
    s = s.replace("\\(", "(").replace("\\)", ")")

    if not is_valid_url(s):
        return None
    if require_image and not is_image_url(s):
        return None
    # Optionally, re-encode path to be safe
    p = urlparse(s)
    safe_path = quote(p.path, safe="/-_.()~%")
    safe = urlunparse((p.scheme, p.netloc, safe_path, p.params, p.query, p.fragment))
    return safe


def normalize_text(text: str | None) -> str | None:
    if text is None:
        return None
    if not isinstance(text, str):
        try:
            text = str(text)
        except Exception:
            return None
    # Remove control characters except newlines and tabs
    text = re.sub(r"[\x00-\x08\x0B\x0C\x0E-\x1F\x7F]", "", text)
    # Trim excessive whitespace around
    return text.strip()


def sanitize_record(rec: dict) -> dict | None:
    name = normalize_text(rec.get(KEY_NAME))
    if not name:
        return None  # drop nameless entries

    description = normalize_text(rec.get(KEY_DESC)) or ""

    img = rec.get(KEY_IMAGE)
    img = normalize_url(img, require_image=True) if isinstance(img, str) else None

    site = rec.get(KEY_WEBSITE)
    site = normalize_url(site, require_image=False) if isinstance(site, str) else None

    mega = rec.get(KEY_MEGA)
    mega = normalize_url(mega, require_image=False) if isinstance(mega, str) else None

    return {
        KEY_NAME: name,
        KEY_DESC: description,
        KEY_IMAGE: img,
        KEY_WEBSITE: site,
        KEY_MEGA: mega,
    }


def main():
    default_path = os.path.join(os.path.dirname(__file__), "data", "megakheir_ngos_slim_20251026_023532.json")
    in_path = sys.argv[1] if len(sys.argv) > 1 else default_path
    if not os.path.isfile(in_path):
        print(f"Input file not found: {in_path}")
        sys.exit(2)

    out_dir = os.path.dirname(in_path)
    base, ext = os.path.splitext(os.path.basename(in_path))
    out_path = os.path.join(out_dir, f"{base}_clean{ext}")

    # Load JSON. If it fails due to malformed JSON (likely copy/paste), exit with guidance.
    try:
        with open(in_path, "r", encoding="utf-8") as f:
            data = json.load(f)
    except Exception as e:
        print("Failed to parse JSON. Please ensure the source file is intact (avoid manual paste into editors that break escaping).")
        print(f"Error: {e}")
        sys.exit(3)

    if not isinstance(data, list):
        print("Input must be a JSON array of objects.")
        sys.exit(4)

    cleaned = []
    dropped = 0
    fixed_img = 0
    fixed_site = 0
    fixed_mega = 0

    for rec in data:
        if not isinstance(rec, dict):
            dropped += 1
            continue
        srec = sanitize_record(rec)
        if not srec:
            dropped += 1
            continue
        # Count fixes by comparing raw vs sanitized
        if rec.get(KEY_IMAGE) and not srec.get(KEY_IMAGE):
            fixed_img += 1
        if rec.get(KEY_WEBSITE) and not srec.get(KEY_WEBSITE):
            fixed_site += 1
        if rec.get(KEY_MEGA) and not srec.get(KEY_MEGA):
            fixed_mega += 1
        cleaned.append(srec)

    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(cleaned, f, ensure_ascii=False, indent=2)

    print(f"Sanitized {len(cleaned)} records -> {out_path}")
    print(f"Dropped: {dropped} | Invalid image_url set to null: {fixed_img} | Invalid external_website_url set to null: {fixed_site} | Invalid megakheir_url set to null: {fixed_mega}")


if __name__ == "__main__":
    main()
