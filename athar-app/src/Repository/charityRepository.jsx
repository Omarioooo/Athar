import api from "../Auth/AxiosInstance";

export async function charityRegisterRequest(formData) {
    return api.post(`/Account/CharityRegister`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
}

export const fetchCharityViewById = async (id) => {
    const res = await api.get(`/charities/charityview/${id}`);
    return res.data;
};


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

    const response = await api.get(`/charities/GetAll?${params}`);
    return response.data;
};

export const fetchCharityByIdFromApi = async (id) => {
    const res = await api.get(`/charities/${id}`);
    return res.data;
};

export const fetchCharityProfileById = async (id) => {
    try {
        const res = await api.get(`/charities/charityProfile/${id}`);
        console.log("res", res);
        return res.data;
    } catch (error) {
        console.error("فشل جلب بيانات الجمعية:", error);
        throw new Error(
            error.response?.data?.message || "تعذر تحميل بيانات الجمعية"
        );
    }
};

export const fetchCampaignsByCharityId = async (id) => {
    const res = await api.get(`/charities/${id}/campaigns`);
    return res.data;
};

export const fetchPagedContentByCharityId = async (
    charityId,
    page = 1,
    pageSize = 12
) => {
    const params = new URLSearchParams({ page, pageSize });
    const res = await api.get(
        `/charities/charity/${charityId}/paged?${params}`
    );
    return res.data;
};

export const fetchFollowersCount = async (charityId) => {
    const res = await api.get(`/charities/${charityId}/followers/count`);
    return res.data;
};
