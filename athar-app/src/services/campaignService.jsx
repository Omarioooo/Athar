import {
    searchCampaigns as searchRepo,
    fetchAllCampaigns as fetchAllRepo,
    fetchCampaignsByType as fetchByTypeRepo,
    fetchCampaignById as fetchByIdRepo,
    createCampaignByCharityId,
} from "../Repository/campaignRepository";
import {
    computeDaysLeft,
    computeProgressPercentage,
} from "../utils/helper/computeUtil";

export const fetchAllCampaigns = async (page = 1, pageSize = 12) => {
    try {
        const data = await fetchAllRepo(page, pageSize);
        return {
            ...data,
            campaigns: data.campaigns.map((c) => ({
                ...c,
                daysLeft: computeDaysLeft(c.startDate, c.endDate),
                progress: computeProgressPercentage(
                    c.raisedAmount,
                    c.goalAmount
                ),
            })),
        };
    } catch (error) {
        console.error("Service Layer Error:", error.message);
        throw new Error("حدث خطأ أثناء جلب جميع الحملات. " + error.message);
    }
};

export const fetchCampaignsByType = async (typeId) => {
    try {
        const campaigns = await fetchByTypeRepo(typeId);
        return campaigns.map((c) => ({
            ...c,
            daysLeft: computeDaysLeft(c.startDate, c.endDate),
            progress: computeProgressPercentage(c.raisedAmount, c.goalAmount),
        }));
    } catch (error) {
        console.error("Service Layer Error:", error.message);
        throw new Error(
            "حدث خطأ أثناء جلب الحملات حسب التصنيف. " + error.message
        );
    }
};

export const searchCampaignsService = async (keyword) => {
    try {
        if (!keyword) throw new Error("الرجاء إدخال كلمة للبحث.");
        const campaigns = await searchRepo(keyword);
        if (!campaigns || campaigns.length === 0)
            throw new Error("لا توجد حملات مطابقة للكلمة المدخلة.");
        return campaigns.map((c) => ({
            ...c,
            daysLeft: computeDaysLeft(c.startDate, c.endDate),
            progress: computeProgressPercentage(c.raisedAmount, c.goalAmount),
        }));
    } catch (error) {
        console.error("Service Layer Error:", error.message);
        throw new Error("حدث خطأ أثناء البحث عن الحملات: " + error.message);
    }
};

export const fetchCampaignById = async (id) => {
    try {
        const campaign = await fetchByIdRepo(id);
        return {
            ...campaign,
            daysLeft: computeDaysLeft(campaign.startDate, campaign.endDate),
            progress: computeProgressPercentage(
                campaign.raisedAmount,
                campaign.goalAmount
            ),
        };
    } catch (error) {
        console.error("Service Layer Error:", error.message);
        throw new Error("حدث خطأ أثناء جلب تفاصيل الحملة: " + error.message);
    }
};

export const CreateCampaign = async (id, formData) => {
    try {
        const campaign = await createCampaignByCharityId(id, formData);
        return campaign;
    } catch (error) {
        console.error("Service Layer Error:", error.message);
        throw new Error("حدث خطأ أثناء إنشاء الحملة: " + error.message);
    }
};
