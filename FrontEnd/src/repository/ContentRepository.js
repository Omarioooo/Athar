import axios from "axios";

const API_URL = "https://localhost:44389/api/Content";

// Fetch all contents
export const fetchAllContents = async (page = 1, pageSize = 12) => {
  try {
    const response = await axios.get(`${API_URL}/GetAll`, {
      params: { page, pageSize },
    });
    return {
      contents: response.data.items,
      totalPages: response.data.totalPages,
    };
  } catch (error) {
    console.error("Repository Error - fetchAllContents:", error.message);
    throw new Error("فشل في جلب الميديا من السيرفر");
  }
};

// Search contents by keyword
export const searchContents = async (word) => {
  try {
    const response = await axios.get(`${API_URL}/search`, { params: { word } });
    return response.data.items || [];
  } catch (error) {
    console.error("Repository Error - searchContents:", error.message);
    return []; 
  }
};

// Get media for a specific campaign
export const getMediaOfCampaign = async (id, page = 1, pageSize = 12) => {
  try {
    const response = await axios.get(`${API_URL}/campaign/${id}/paged`, {
      params: { page, pageSize },
    });
    return {
      contents: response.data.items,
      totalPages: response.data.totalPages,
    };
  } catch (error) {
    console.error("Repository Error - getMediaOfCampaign:", error.message);
    return { contents: [], totalPages: 0 }; // بدل alert
  }
};
