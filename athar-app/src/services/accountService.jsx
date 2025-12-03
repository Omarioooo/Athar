import { loginRequest } from "../Repository/accountRepository";
import { validateEmail } from "../utils/validation/validateEmail";
import { validatePassword } from "../utils/validation/validatePassword";

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
        const { token, id, email, userName, role } = response.data;

        const userData = { id, email, userName, role };

        return { success: true, data: { ...userData, token } };
    } catch (err) {
        console.error("Login failed:", err);
        return {
            success: false,
            errors: err.response?.data?.message || "فشل تسجيل الدخول",
        };
    }
}
