import {
    fetchAllContents,
    searchContents,
    getMediaOfCampaign,
} from "../Repository/contentRepository";
import { formatArabicDate } from "../utils/helper/formateUtil";

const enrichContents = (contents) =>
    contents.map((c) => ({
        ...c,
        arabicDate: formatArabicDate(c.createdAt),
    }));

export const getAllContentsService = async (page = 1, pageSize = 12) => {
    try {
        const data = await fetchAllContents(page, pageSize);
        return { ...data, contents: enrichContents(data.contents) };
    } catch (error) {
        console.error("Service Error - getAllContents:", error.message);
        return { contents: [], totalPages: 1, error: error.message };
    }
};

export const searchMediaService = async (word) => {
    try {
        const data = await searchContents(word);
        return { contents: enrichContents(data), totalPages: 1 };
    } catch (error) {
        console.error("Service Error - searchMedia:", error.message);
        return { contents: [], totalPages: 1, error: error.message };
    }
};

export const getCampaignMediaService = async (id, page = 1, pageSize = 12) => {
    try {
        const data = await getMediaOfCampaign(id, page, pageSize);
        return { ...data, contents: enrichContents(data.contents) };
    } catch (error) {
        console.error("Service Error - getCampaignMedia:", error.message);
        return { contents: [], totalPages: 0, error: error.message };
    }
};
