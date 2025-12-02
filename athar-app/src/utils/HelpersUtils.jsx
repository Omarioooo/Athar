export function getFilteredDonations(data = [], filter) {
    const now = new Date();
    return data.filter((don) => {
        const date = new Date(don.date);

        if (filter === "all") return true;

        if (filter === "day") return date.toDateString() === now.toDateString();

        if (filter === "week") {
            const weekAgo = new Date(now - 7 * 24 * 60 * 60 * 1000);
            return date >= weekAgo;
        }

        if (filter === "month")
            return (
                date.getMonth() === now.getMonth() &&
                date.getFullYear() === now.getFullYear()
            );

        if (filter === "year") return date.getFullYear() === now.getFullYear();

        return true;
    });
}

export function getFilteredCampaignsByProgress(data = [], filter) {
    if (!Array.isArray(data)) return [];

    return data.filter((cmp) => {
        const raised = Number(cmp.raisedAmount || 0);
        const goal = Number(cmp.goalAmount || 1);
        const progress = Math.round((raised / goal) * 100);

        if (filter === "completed") {
            return progress >= 100;
        }

        if (filter === "inprogress") {
            return progress < 100;
        }

        return true;
    });
}

export function computeDaysLeftForCampaigns(startDate, endDate) {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diffTime = end - start;
    let diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays > 0 ? diffDays : 0;
}

export function computeProgressPercentageForCampaigns(
    raisedAmount,
    goalAmount
) {
    return Math.round((raisedAmount / goalAmount) * 100);
}

export function getRelativeTimeForNotification(dateString) {
    const now = new Date();
    const past = new Date(dateString);
    const diffMs = now - past;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 60) return `منذ ${diffMins} دقيقة`;
    if (diffHours < 24) return `منذ ${diffHours} ساعة`;
    return `منذ ${diffDays} يوم`;
}
