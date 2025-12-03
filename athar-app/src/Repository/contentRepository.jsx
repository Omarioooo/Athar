import api from "../Auth/AxiosInstance";

export const fetchAllContents = async (page = 1, pageSize = 12) => {
    const response = await api.get(`/Content/GetAll`, {
        params: { page, pageSize },
    });
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};

export const searchContents = async (word) => {
    const response = await api.get(`/Content/search`, {
        params: { word },
    });
    return response.data.items || [];
};

export const getMediaOfCampaign = async (id, page = 1, pageSize = 12) => {
    const response = await api.get(`/Content/campaign/${id}/paged`, {
        params: { page, pageSize },
    });
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};
