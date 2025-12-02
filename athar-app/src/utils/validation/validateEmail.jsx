export function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!email || email.trim() === "") {
        return "هذا الحقل مطلوب";
    }
    if (!emailRegex.test(email)) {
        return "الايميل غير صالح";
    }
    return null;
}
