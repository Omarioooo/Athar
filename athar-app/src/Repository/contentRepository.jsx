import axios from "axios";

const API_URL = "https://localhost:5192/api/Content";

export const fetchAllContents = async (page = 1, pageSize = 12) => {
    const response = await axios.get(`${API_URL}/GetAll`, {
        params: { page, pageSize },
    });
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};

export const searchContents = async (word) => {
    const response = await axios.get(`${API_URL}/search`, {
        params: { word },
    });
    return response.data.items || [];
};

export const getMediaOfCampaign = async (id, page = 1, pageSize = 12) => {
    const response = await axios.get(`${API_URL}/campaign/${id}/paged`, {
        params: { page, pageSize },
    });
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};
