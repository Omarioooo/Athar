export const computeDaysLeft = (startDate, endDate) => {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diffTime = end - start;
    let diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays > 0 ? diffDays : 0;
};

export const computeProgressPercentage = (raisedAmount, goalAmount) => {
    return goalAmount > 0 ? Math.round((raisedAmount / goalAmount) * 100) : 0;
};
