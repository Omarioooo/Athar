import {
    charityRegisterRequest as repoRegister,
    fetchCharities,
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
    fetchCharityByIdFromApi,
    fetchCampaignsByCharityId,
    fetchPagedContentByCharityId,
    fetchFollowersCount,
} from "../Repository/charityRepository";

function validateCharityForm(user) {
    console.log("Charity USer is :- ", user);
    
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

export const getCharityProfileData = async (charityId) => {
    if (!charityId) throw new Error("Charity ID is required");

    try {
        const [charityData, campaignsData, contentData, followersCount] =
            await Promise.all([
                fetchCharityByIdFromApi(charityId),
                fetchCampaignsByCharityId(charityId).catch(() => []),
                fetchPagedContentByCharityId(charityId, 1, 30).catch(() => ({
                    items: [],
                })),
                fetchFollowersCount(charityId).catch(() => 0),
            ]);

        const realFollowersCount =
            typeof followersCount === "object"
                ? followersCount.count ?? followersCount.value ?? 0
                : followersCount;

        return {
            id: charityData.id,
            name: charityData.name,
            logo: charityData.imageUrl || "/fallback-logo.jpg",
            coverImage: charityData.imageUrl || "/fallback-cover.jpg",
            description: charityData.description || "لا يوجد وصف متاح",
            followersCount: realFollowersCount,
            campaignsCount:
                campaignsData.length || charityData.campaigns?.length || 0,
            campaigns: campaignsData.map((c) => ({
                id: c.id,
                title: c.title,
                description: c.description || "لا يوجد وصف",
                raisedAmount: c.raisedAmount || 0,
                goalAmount: c.goalAmount || 0,
                startDate: c.startDate,
                endDate: c.endDate,
                charityName: c.charityName || charityData.name,
                imageUrl: c.imageUrl,
            })),
            mediaPosts: contentData.items.map((post) => ({
                id: post.id || Date.now() + Math.random(),
                image: post.image || post.imageUrl || "/fallback-post.jpg",
                caption:
                    post.caption ||
                    post.title ||
                    post.description ||
                    "منشور من الجمعية",
                likes: post.likes || Math.floor(Math.random() * 600) + 100,
            })),
        };
    } catch (error) {
        console.error("Failed to load charity profile:", error);
        throw error;
    }
};
