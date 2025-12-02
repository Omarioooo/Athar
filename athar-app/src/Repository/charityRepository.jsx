import axios from "axios";

const API_BASE = "https://localhost:5192/api";

export async function charityRegisterRequest(formData) {
    return axios.post(`${API_BASE}/Account/CharityRegister`, formData);
}

export const fetchCharities = async ({
    query = "",
    page = 1,
    pageSize = 9,
    includeCampaigns = false,
}) => {
    const params = new URLSearchParams({
        page,
        pageSize,
        includeCampaigns: includeCampaigns,
    });

    if (query) params.append("query", query.trim());

    const response = await axios.get(`${API_BASE}/charities?${params}`);
    return response.data;
};

export const fetchCharityByIdFromApi = async (id) => {
    const res = await axios.get(`${API_BASE}/charities/${id}`);
    return res.data;
};

export const fetchCampaignsByCharityId = async (id) => {
    const res = await axios.get(`${API_BASE}/charities/${id}/campaigns`);
    return res.data;
};

export const fetchPagedContentByCharityId = async (
    charityId,
    page = 1,
    pageSize = 12
) => {
    const params = new URLSearchParams({ page, pageSize });
    const res = await axios.get(
        `${API_BASE}/charities/charity/${charityId}/paged?${params}`
    );
    return res.data;
};

export const fetchFollowersCount = async (charityId) => {
    const res = await axios.get(
        `${API_BASE}/charities/${charityId}/followers/count`
    );
    return res.data;
};