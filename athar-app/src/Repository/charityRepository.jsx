import api from "../Auth/AxiosInstance";

// register as a charity
export async function charityRegisterRequest(formData) {
    return api.post(`/Account/CharityRegister`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
}

// get all charities for display in cards
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

// get a charity data page from the card
export const fetchCharityViewById = async (id) => {
    const res = await api.get(`/charities/charityview/${id}`);
    return res.data;
};

// i do not need it
export const fetchCharityByIdFromApi = async (id) => {
    const res = await api.get(`/charities/${id}`);
    return res.data;
};

// get the account image for header
export const fetchCharityImageById = async (id) => {
    const res = await api.get(`/charities/image/${id}`);
    return res.data;
};

// get the charity profile for personal account
export const fetchCharityProfileById = async (id) => {
    const res = await api.get(`/charities/charityProfile/${id}`);
    return res.data;
};

// get all campaigns for the charity
export const fetchCampaignsByCharityId = async (id) => {
    const res = await api.get(`/charities/${id}/campaigns`);
    return res.data;
};

// get content
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

// get all charity followers count
export const fetchFollowersCount = async (charityId) => {
    const res = await api.get(`/charities/${charityId}/followers/count`);
    return res.data;
};

// update the charity profile
export const updateCharity = async (id, data) => {
    const formData = new FormData();

    if (data.name !== undefined && data.name !== null) {
        formData.append("CharityName", data.name);
    }
    if (data.description !== undefined && data.description !== null) {
        formData.append("Description", data.description);
    }
    if (data.country !== undefined && data.country !== null) {
        formData.append("Country", data.country);
    }
    if (data.city !== undefined && data.city !== null) {
        formData.append("City", data.city);
    }
    if (data.profileImage) {
        formData.append("ProfileImage", data.profileImage);
    }

    Object.keys(data).forEach((key) => {
        if (data[key] !== undefined && data[key] !== null) {
            formData.append(key, data[key]);
        }
    });

    const res = await api.put(`/charities/update/${id}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });

    return res.data;
};


// delete charity
export const deleteCharityApi = async (id) => {
    const res = await api.delete(`/charities/${id}`);
    return res.data;
};


// get charities with status pending
export const fetchPendingCharities = async (id) => {
    const res = await api.get(`/charities/join`);
    return res.data;
};