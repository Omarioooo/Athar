export function validateFirstName(name) {
    if (!name) return "هذا الحقل مطلوب";
    if (name.length > 10) return "طول الاسم يجب ان يكون اقل من 10 حروف";
    return null;
}

export function validateLastName(name) {
    if (!name) return "هذا الحقل مطلوب";
    if (name.length > 10) return "طول الاسم يجب ان يكون اقل من 10 حروف";
    return null;
}