import axios from "axios";

const API_URL = "https://localhost:44389/api/Campaign";

export const fetchAllCampaigns = async (page = 1, pageSize = 12) => {
  try {
    const response = await axios.get(`${API_URL}/GetAll`, { params: { page, pageSize } });
    return {
      campaigns: response.data.items,
      totalPages: response.data.totalPages,
    };
  } catch (error) {
    throw new Error("API_ERROR_FETCH_ALL: " + (error.response?.data?.message || error.message));
  }
};

export const fetchCampaignsByType = async (type) => {
  try {
    const response = await axios.get(`${API_URL}/GetByType`, { params: { type } });
    return response.data.items || response.data;
  } catch (error) {
    throw new Error("API_ERROR_FETCH_BY_TYPE: " + (error.response?.data?.message || error.message));
  }
};

export const searchCampaign = async (keyword) => {
  try {
    const response = await axios.get(`${API_URL}/Search`, { params: { keyword } });
    return response.data.items || response.data;
  } catch (error) {
    throw new Error("API_ERROR_SEARCH: لا توجد حملات مطابقة للكلمة المدخلة.");
  }
};

export const fetchCampaignById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/GetCampaign/${id}`, { params: { inProgress: true } });
    return response.data;
  } catch (error) {
    throw new Error("API_ERROR_FETCH_BY_ID: " + (error.response?.data?.message || error.message));
  }
};
