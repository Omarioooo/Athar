import { jwtDecode } from "jwt-decode";

export function extractUserId(token) {
    const decoded = jwtDecode(token);

    return decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    ];
}
