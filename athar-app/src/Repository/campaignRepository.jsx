import api from "../Auth/AxiosInstance";

const getRequest = async (endpoint, params = {}) => {
    try {
        const response = await api.get(`/Campaign/${endpoint}`, { params });
        return response.data.items || response.data;
    } catch (error) {
        const message = error.response?.data?.message || error.message;
        throw new Error(message);
    }
};

export const fetchAllCampaigns = async (page = 1, pageSize = 12) => {
    const data = await getRequest("GetAll", { page, pageSize });
    return {
        campaigns: data.items || data,
        totalPages: data.totalPages || 1,
    };
};

export const fetchCampaignsByType = async (typeId) => {
    return getRequest("GetByType", { type: typeId });
};

export const searchCampaigns = async (keyword) => {
    const data = await getRequest("Search", { keyword });
    if (!data || (Array.isArray(data) && data.length === 0)) {
        throw new Error("لا توجد حملات مطابقة للكلمة المدخلة.");
    }
    return data;
};

export const fetchCampaignById = async (id) => {
    return getRequest(`GetCampaign/${id}`, { inProgress: true });
};
