import { fetchAllCampaigns, fetchCampaignsByType, searchCampaign } from "../repository/compaignyrepository";

export const getAllCampaigns = async (page, pageSize) => {
  try {
    const data = await fetchAllCampaigns(page, pageSize);
    return data;
  } catch (error) {
    console.error("Business Layer Error:", error.message);
   
    throw new Error("حدث خطأ أثناء جلب جميع الحملات. " + error.message);
  }
};

export const getCampaignsByType = async (type) => {
  try {
    const data = await fetchCampaignsByType(type);
    return data;
  } catch (error) {
    console.error("Business Layer Error:", error.message);
    throw new Error("حدث خطأ أثناء جلب الحملات حسب التصنيف. " + error.message);
  }
};

export const searchCampaigns = async (keyword) => {
  try {
    if (!keyword) throw new Error("الرجاء إدخال كلمة للبحث.");
    const data = await searchCampaign(keyword);
    if (data.length === 0) throw new Error("لا توجد حملات مطابقة للكلمة المدخلة.");
    return data;
  } catch (error) {
    console.error("Business Layer Error:", error.message);
    throw new Error("حدث خطأ أثناء البحث عن الحملات: " + error.message);
  }
};

// دوال مساعدة UI
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
