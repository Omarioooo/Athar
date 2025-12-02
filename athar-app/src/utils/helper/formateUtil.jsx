export const formatArabicDate = (dateString) => {
    try {
        const date = new Date(dateString);
        return date.toLocaleDateString("ar-EG", {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric",
        });
    } catch (error) {
        console.error("Date formatting error:", error);
        return dateString;
    }
};
