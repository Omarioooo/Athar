import { fetchAllContents, searchContents, getMediaOfCampaign } from "../repository/ContentRepository";

// Business Layer: Fetch all contents
export const getAllContents = async (page = 1, pageSize = 12) => {
  try {
    return await fetchAllContents(page, pageSize);
  } catch (error) {
    console.error("Service Error - getAllContents:", error.message);
    return { contents: [], totalPages: 1, error: error.message };
  }
};

// Business Layer: Search contents
export const searchMedia = async (word) => {
  try {
    const data = await searchContents(word);
    return { contents: data, totalPages: 1 };
  } catch (error) {
    console.error("Service Error - searchMedia:", error.message);
    return { contents: [], totalPages: 1, error: error.message };
  }
};

// Business Layer: Get media of specific campaign
export const getCampaignMedia = async (id, page = 1, pageSize = 12) => {
  try {
    return await getMediaOfCampaign(id, page, pageSize);
  } catch (error) {
    console.error("Service Error - getCampaignMedia:", error.message);
    return { contents: [], totalPages: 0, error: error.message };
  }
};
