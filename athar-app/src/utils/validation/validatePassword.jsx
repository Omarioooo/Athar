export function validatePassword(password) {
    if (!password || password.trim() === "") {
        return "هذا الحقل مطلوب";
    }
    if (password.length > 10) {
        return "طول الباسورد يجب أن يكون أقل من 10 حروف";
    }
    return null;
}

export function validateConfirmPassword(password, confirmPassword) {
    if (!confirmPassword || confirmPassword.trim() === "")
        return "هذا الحقل مطلوب";

    if (password !== confirmPassword) return "كلمة المرور غير متطابقة";

    return null;
}
