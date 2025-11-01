import json
import sys
from urllib import request, error

BASE = 'http://localhost:5192'
LOGIN_URL = BASE + '/api/Account/Login'
IMPORT_URL = BASE + '/api/Campaign/import'
JSON_PATH = r'c:\\Users\\Rowan\\OneDrive\\Desktop\\collage\\depi\\graduationProject\\project\\Athar\\scrapers\\megakheir\\data\\megakheir_campaigns_23.json'


def http_post_json(url, data, headers=None):
    body = json.dumps(data).encode('utf-8') if not isinstance(data, (bytes, bytearray)) else data
    req = request.Request(url, data=body, headers={ 'Content-Type': 'application/json; charset=utf-8', **(headers or {}) })
    try:
        with request.urlopen(req) as resp:
            charset = resp.headers.get_content_charset() or 'utf-8'
            text = resp.read().decode(charset, errors='replace')
            try:
                return json.loads(text), resp.getcode(), text
            except json.JSONDecodeError:
                return None, resp.getcode(), text
    except error.HTTPError as e:
        try:
            err_text = e.read().decode('utf-8', errors='replace')
        except Exception:
            err_text = str(e)
        return None, e.code, err_text
    except Exception as e:
        return None, None, str(e)


def main():
    # Login
    login_payload = { 'UserNameOrEmail': 'admin@athar.local', 'Password': 'Admin#123' }
    login_json, code, raw = http_post_json(LOGIN_URL, login_payload)
    if not login_json or 'token' not in login_json:
        print('Login failed:', code, raw)
        sys.exit(1)
    token = login_json['token']
    print('Got token length:', len(token))

    # Read campaigns JSON
    with open(JSON_PATH, 'r', encoding='utf-8') as f:
        campaigns_text = f.read()
    try:
        payload = json.loads(campaigns_text)
    except json.JSONDecodeError as e:
        print('Local JSON parse error:', e)
        sys.exit(1)

    # Import
    data_bytes = json.dumps(payload, ensure_ascii=False).encode('utf-8')
    headers = { 'Authorization': f'Bearer {token}' }
    req = request.Request(IMPORT_URL, data=data_bytes, headers={ 'Content-Type': 'application/json; charset=utf-8', **headers })
    try:
        with request.urlopen(req) as resp:
            text = resp.read().decode('utf-8', errors='replace')
            print('Import response:', text)
    except error.HTTPError as e:
        print('Import failed:', e.code)
        try:
            print(e.read().decode('utf-8', errors='replace'))
        except Exception:
            pass
        sys.exit(1)


if __name__ == '__main__':
    main()
