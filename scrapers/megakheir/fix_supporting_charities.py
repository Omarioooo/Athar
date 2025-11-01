import json
import re
import sys
import difflib
from pathlib import Path

# Usage:
#   py -3 fix_supporting_charities.py \
#       --json scrapers/megakheir/data/megakheir_campaigns_23.json \
#       --db-names scrapers/megakheir/data/charity_names_from_db.txt \
#       [--in-place]

STOPWORDS = {
    "جمعية", "مؤسسة", "مستشفى", "مستشفيات", "مؤسسه", "هيئة", "اتحاد", "مجلس", "مركز", "مؤسسةٌ", "مؤسسةً"
}

ar_letters_map = str.maketrans({
    "أ": "ا",
    "إ": "ا",
    "آ": "ا",
    "ى": "ي",
    "ٔ": "",
    "ء": "",
    "ؤ": "و",
    "ئ": "ي",
    "ة": "ه",
    "٪": "%",
})

PUNCT_RE = re.compile(r"[\u0640\u200f\u200e\u061f\u060c,.;:\-–—_/\\()\[\]{}""'`~!@#$^&*+=|<>؟،٫٬]+")
SPACE_RE = re.compile(r"\s+")

ALIASES = {
    # Common shorter labels -> canonical DB naming variants (best effort)
    "مؤسسة مرسال": "مؤسسة مرسال للأعمال الخيرية",
    "مؤسسة مصر الخير": "مؤسسة مصر الخير",
    # keep as-is if present in DB
    "جمعية رسالة": "جمعية رسالة",
    "بنك الشفاء المصري": "بنك الشفاء المصرى",
    "مستشفى 57357": "مستشفى سرطان الأطفال 57357",
    "مستشفى سرطان الاطفال 57357": "مستشفى سرطان الأطفال 57357",
    "مستشفيات شفا الأورمان": "مستشفيات شفا الأورمان (لصعيد بلا سرطان)",
    "الهلال الأحمر": "جمعية الهلال الأحمر المصرى",
    "الهلال الأحمر المصري": "جمعية الهلال الأحمر المصرى",
    "مؤسسة ويانا": "مؤسسة ويانا الدولية للتوعية ودمج الحالات الخاصة في المجتمع",
    "جمعية بنكمل بعض": "جمعية بنكمل بعض",
    "جمعية مصر المحروسة بلدى": "جمعية مصر المحروسة بلدى",
    "جمعية بداية للأعمال الخيرية": "جمعية بداية للأعمال الخيرية",
    "جمعية خير أمة": "جمعية خير أمة",
    "مؤسسة مستشفى 25 يناير": "مؤسسة مستشفى 25 يناير",
    "بوابتك للخير": "بوابتك للخير",
    "جمعية شبرا الخير": "مؤسسة شبرا الخير",
    "مؤسسة شباب الخير بميت فضالة": "مؤسسة شباب الخير بميت فضالة",
    "مؤسسة ديارنا للتنمية": "مؤسسة ديارنا للتنمية",
    "مؤسسة حياة كريمة": "مؤسسة حياة كريمة",
    "جمعية الأورمان": "جمعية الأورمان",
    "بنك الطعام المصرى": "بنك الطعام المصرى",
    "جمعية عمار الأرض": "جمعية عمار الأرض",
}

# Explicitly drop supporters not in DB (known non-present variants)
BLOCKLIST = {
    "بنك الشفاء المصرى", "بنك الشفاء المصري"
}


def normalize(text: str) -> str:
    if not text:
        return ""
    t = text.strip()
    t = t.translate(ar_letters_map)
    t = PUNCT_RE.sub(" ", t)
    t = SPACE_RE.sub(" ", t)
    parts = [p for p in t.split(" ") if p and p not in STOPWORDS]
    return " ".join(parts)


def best_match(name: str, db_names_norm: list[str], db_names: list[str]) -> tuple[str | None, float]:
    n = normalize(name)
    # exact normalized match first
    for i, dn in enumerate(db_names_norm):
        if n == dn:
            return db_names[i], 1.0
    # contains either way
    for i, dn in enumerate(db_names_norm):
        if n and (n in dn or dn in n):
            return db_names[i], 0.92
    # difflib ratio
    ratios = [(i, difflib.SequenceMatcher(a=n, b=dn).ratio()) for i, dn in enumerate(db_names_norm)]
    i, r = max(ratios, key=lambda x: x[1]) if ratios else (-1, 0.0)
    return (db_names[i], r) if r >= 0.80 else (None, r)


def load_db_names(path: Path) -> list[str]:
    raw = path.read_text(encoding="utf-8", errors="ignore").splitlines()
    # strip blanks
    out = []
    for r in raw:
        s = r.lstrip("\ufeff").strip()
        if s:
            out.append(s)
    return out


def main():
    import argparse
    ap = argparse.ArgumentParser()
    ap.add_argument("--json", required=True, help="Path to campaigns JSON")
    ap.add_argument("--db-names", required=True, help="Path to DB charity names (UTF-8, one per line)")
    ap.add_argument("--in-place", action="store_true", help="Overwrite input JSON (default writes *.fixed.json)")
    args = ap.parse_args()

    json_path = Path(args.json)
    db_path = Path(args["db_names"] if isinstance(args, dict) else args.db_names)

    campaigns = json.loads(json_path.read_text(encoding="utf-8"))
    db_names = load_db_names(db_path)

    # Build normalized names index
    db_names_norm = [normalize(n) for n in db_names]

    # Also apply alias expansion to improve matches
    def alias_or_self(n: str) -> str:
        return ALIASES.get(n, n)

    report = []
    fixed_any = False

    for c in campaigns:
        supporters = c.get("supporting_charities", []) or []
        new_list = []
        for s in supporters:
            if s in BLOCKLIST:
                report.append(f"DROP: '{s}' (blocklisted)")
                fixed_any = True
                continue
            s_alias = alias_or_self(s)
            match, score = best_match(s_alias, db_names_norm, db_names)
            if match:
                if match not in new_list:
                    new_list.append(match)
                if s != match:
                    report.append(f"MAP: '{s}' -> '{match}' (score={score:.2f})")
                    fixed_any = True
            else:
                report.append(f"DROP: '{s}' (no DB match, score={score:.2f})")
                fixed_any = True
        c["supporting_charities"] = new_list

    out_path = json_path if args.in_place else json_path.with_suffix(".fixed.json")
    out_path.write_text(json.dumps(campaigns, ensure_ascii=False, indent=2), encoding="utf-8")

    # Write a small report next to the output
    rep_path = out_path.with_suffix(out_path.suffix + ".report.txt")
    rep_path.write_text("\n".join(report), encoding="utf-8")

    print(f"Wrote {out_path}")
    print(f"Changes: {'YES' if fixed_any else 'NO'} | Report: {rep_path.name}")


if __name__ == "__main__":
    try:
        main()
    except Exception as e:
        print("ERROR:", e)
        sys.exit(1)
