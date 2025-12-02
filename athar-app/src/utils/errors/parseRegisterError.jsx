export function parseRegisterError(err) {
    if (err.response && err.response.data) {
        return err.response.data.message || "حدث خطأ أثناء التسجيل";
    }
    return "حدث خطأ أثناء الاتصال بالسيرفر";
}
