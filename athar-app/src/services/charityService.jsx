import {
    charityRegisterRequest as repoRegister,
    fetchCharities,
    fetchCharityProfileById,
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

import {
    fetchCharityViewById
} from "../Repository/charityRepository";

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

export const getCharityView = async (charityId) => {
    try {
        const fetchedData = await fetchCharityViewById(charityId);
        return fetchedData;
    } catch (error) {
        console.error("Failed to load charity profile:", error);
        throw error;
    }
};

export async function getCharityProfile({ id }) {
    const profile = await fetchCharityProfileById(id);
    return profile;
}
