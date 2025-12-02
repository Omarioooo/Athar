import { jwtDecode } from "jwt-decode";

export function extractRoleFromToken(token) {
    const decoded = jwtDecode(token);

    return decoded[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];
}
