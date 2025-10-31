import json
import os
import sys
from typing import List, Dict, Any

"""
Filter the cleaned MegaKheir JSON to only include charities with both:
- image_url != null (sanitizer already set invalid to null)
- external_website_url != null
and drop the 'megakheir_url' field. Output UTF-8 JSON.

Usage:
  py -3 scrapers/megakheir/filter_megakheir_for_import.py <input_json> [<output_json>]
Default output path: <input_basename>_filtered.json
"""

def main():
    if len(sys.argv) < 2:
        print("Usage: py -3 scrapers/megakheir/filter_megakheir_for_import.py <input_json> [<output_json>]")
        sys.exit(1)

    in_path = sys.argv[1]
    if not os.path.exists(in_path):
        print(f"Input not found: {in_path}")
        sys.exit(1)

    if len(sys.argv) >= 3:
        out_path = sys.argv[2]
    else:
        base, ext = os.path.splitext(in_path)
        out_path = f"{base}_filtered.json"

    with open(in_path, "r", encoding="utf-8") as f:
        data: List[Dict[str, Any]] = json.load(f)

    original = len(data)
    # Keep only items having both valid image_url and external_website_url
    filtered = [
        {
            k: v for k, v in item.items()
            if k != "megakheir_url"
        }
        for item in data
        if (item.get("image_url") is not None) and (item.get("external_website_url") is not None)
    ]

    kept = len(filtered)
    dropped = original - kept

    # Write UTF-8 without BOM
    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(filtered, f, ensure_ascii=False, indent=2)

    print(f"Filtered {kept}/{original} items -> {out_path}")
    print(f"Dropped: {dropped}")


if __name__ == "__main__":
    main()
