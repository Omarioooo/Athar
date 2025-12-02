export function validateCharityName(name) {
    if (!name || name.trim() === "") return "هذا الحقل مطلوب";
    return null;
}

export function validateCharityDescription(desc) {
    if (!desc || desc.trim() === "") return "هذا الحقل مطلوب";
    return null;
}
