export function parseLoginError(err) {
    if (err.response && err.response.data) {
        const msg =
            err.response.data.message ||
            err.response.data.error ||
            "خطأ غير معروف";

        if (msg.toLowerCase().includes("password")) {
            return { emailerror: null, passworderror: "كلمة المرور غير صحيحة" };
        }

        if (msg.toLowerCase().includes("email")) {
            return { emailerror: "الايميل غير صحيح", passworderror: null };
        }
    }

    return { emailerror: null, passworderror: null };
}
