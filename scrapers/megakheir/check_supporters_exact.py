import json, sys, os
base = os.path.dirname(__file__)
json_path = os.path.join(base, 'data', 'megakheir_campaigns_23.json')
db_path = os.path.join(base, 'data', 'charity_names_from_db.txt')

with open(json_path, 'r', encoding='utf-8') as f:
    data = json.load(f)

supporters = set()
for item in data:
    for s in item.get('supporting_charities', []) or []:
        if s:
            supporters.add(s)

with open(db_path, 'r', encoding='utf-8') as f:
    def clean_line(s: str) -> str:
        return s.lstrip('\ufeff').strip()
    db_names = {clean_line(line) for line in f if clean_line(line)}

missing = sorted(s for s in supporters if s not in db_names)
print('Total supporters unique:', len(supporters))
print('Not in DB (exact) count:', len(missing))
for s in missing:
    print('-', s)
