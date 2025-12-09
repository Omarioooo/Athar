import {
    charityRegisterRequest as repoRegister,
    fetchCharities,
    fetchCharityProfileById,
    fetchCharityImageById,
    fetchCampaignsByCharityId,
    updateCharity,
    deleteCharityApi,
    fetchPendingCharities,
    fetchCharityStatsById,
    approveCharityById,
    rejectCharityById,
    fetchCharityStatusById,
} from "../Repository/charityRepository";
import { validateEmail } from "../utils/validation/validateEmail";
import {
    validatePassword,
    validateConfirmPassword,
} from "../utils/validation/validatePassword";
import {
    validateCharityName,
    validateCharityDescription,
} from "../utils/validation/validateCharityData";
import { parseRegisterError } from "../utils/errors/parseRegisterError";

import { fetchCharityViewById } from "../Repository/charityRepository";

function validateCharityForm(user) {
    return {
        emailerror: validateEmail(user.email),
        charitynameerror: validateCharityName(user.charityname),
        charitydescriptionerror: validateCharityDescription(
            user.charitydescription
        ),
        passworderror: validatePassword(user.password),
        confirmpassworderror: validateConfirmPassword(
            user.password,
            user.confirmpassword
        ),
    };
}

// register service
export async function registerCharity(user) {
    const formErrors = validateCharityForm(user);
    if (Object.values(formErrors).some((err) => err !== null)) {
        return { success: false, errors: formErrors };
    }

    console.log(
        "!user.img || !user.documentation :-",
        !user.img || !user.documentation
    );

    if (!user.img || !user.documentation) {
        return {
            success: false,
            errors: { serverError: "يرجى اختيار الملفات المطلوبة!" },
        };
    }

    const formdata = new FormData();
    formdata.append("Email", user.email);
    formdata.append("CharityName", user.charityname);
    formdata.append("Password", user.password);
    formdata.append("Description", user.charitydescription);
    formdata.append("Country", user.country);
    formdata.append("City", user.city);
    formdata.append("Role", "Charity");
    formdata.append("ProfileImage", user.img);
    formdata.append("VerificationDocument", user.documentation);

    try {
        await repoRegister(formdata);
        return { success: true };
    } catch (err) {
        return {
            success: false,
            errors: { serverError: parseRegisterError(err) },
        };
    }
}

// get all charities
export async function getAllCharities(
    searchQuery = "",
    currentPage = 1,
    pageSize = 9,
    includeCampaigns = false
) {
    const fetchedData = await fetchCharities({
        query: searchQuery,
        page: currentPage,
        pageSize: pageSize,
        includeCampaigns: includeCampaigns,
    });

    const data = {
        charities: fetchedData.items.map((item) => ({
            id: item.id,
            name: item.name,
            description: item.description || "",
            campaignsCount: item.campaignsCount || 0,
            imageUrl: item.imageUrl,
        })),
        total: fetchedData.total,
        page: fetchedData.page,
        pageSize: fetchedData.pageSize,
    };

    return data;
}

// get the charity view for donor
export const getCharityView = async (charityId) => {
    try {
        const fetchedData = await fetchCharityViewById(charityId);
        return fetchedData;
    } catch (error) {
        console.error("Failed to load charity view:", error);
        throw error;
    }
};

// get the charity profile for personal profile
export async function getCharityProfile(id) {
    try {
        const profile = await fetchCharityProfileById(id);
        return profile;
    } catch (error) {
        console.error("Failed to load charity profile:", error);
        throw error;
    }
}

// get charity image
export async function getCharityImage(id) {
    try {
        const image = await fetchCharityImageById(id);
        return image;
    } catch (err) {
        console.error("Failed to load charity image:", err);
        throw err;
    }
}

// get charity's campaign
export async function getCharityCampaigns(id) {
    try {
        const campaigns = await fetchCampaignsByCharityId(id);
        return campaigns;
    } catch (err) {
        console.error("Failed to load charity campaigns:", err);
        throw err;
    }
}

export async function UpdateCharityData(id, data) {
    try {
        const charity = await updateCharity(id, data);
        return charity;
    } catch (err) {
        console.error("Failed to update charity:", err);
        throw err;
    }
}


// delete charity service
export async function deleteCharity(id) {
    try {
        const image = await deleteCharityApi(id);
        return image;
    } catch (err) {
        console.error("Failed to remove charity:", err);
        throw err;
    }
}


// get charity's campaign
export async function getPendingCharity() {
    try {
        const charities = await fetchPendingCharities();
        return charities;
    } catch (err) {
        console.error("Failed to load charities:", err);
        throw err;
    }
}

// get the charity view for donor
export const getCharityStats = async (id) => {
    try {
        const fetchedData = await fetchCharityStatsById(id);
        return fetchedData;
    } catch (error) {
        console.error("Failed to load charity stats:", error);
        throw error;
    }
};

export const approveCharity = async (id) => {
    try {
        const result = await approveCharityById(id);
        return result;
    } catch (error) {
        console.error("Failed to approve charity:", error);
        throw error;
    }
};

export const rejectCharity = async (id) => {
    try {
        const result = await rejectCharityById(id);
        return result;
    } catch (error) {
        console.error("Failed to reject charity:", error);
        throw error;
    }
};

export const CharityStatus = async (id) => {
    try {
        const result = await fetchCharityStatusById(id);
        return result;
    } catch (error) {
        console.error("Failed to reject charity:", error);
        throw error;
    }
};