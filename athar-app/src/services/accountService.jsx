import { loginRequest } from "../Repository/accountRepository";
import { extractRoleFromToken } from "../Auth/extractRoleFromToken";
import { validateEmail } from "../utils/validation/validateEmail";
import { validatePassword } from "../utils/validation/validatePassword";
import { parseLoginError } from "../utils/errors/parseLoginError";

export function validateLoginForm(user) {
    const errors = {
        emailerror: validateEmail(user.email),
        passworderror: validatePassword(user.password),
    };
    return errors;
}

export async function loginUser(user) {
    const errors = validateLoginForm(user);
    if (Object.values(errors).some((e) => e !== null)) {
        return { success: false, errors };
    }

    try {
        const response = await loginRequest(user.email, user.password);
        const token = response.data.token;
        const role = extractRoleFromToken(token);

        // Handle cookies
        document.cookie = `token=${token}; path=/`;
        document.cookie = `role=${role}; path=/`;

        return { success: true, data: { email: user.email, token, role } };
    } catch (err) {
        return { success: false, errors: parseLoginError(err) };
    }
}
