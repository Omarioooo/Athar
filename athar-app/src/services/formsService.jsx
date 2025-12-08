import {
    createVendorOffer,
    createVolunteerOffer,
    getCharityApplications,
    getOneCharityApplication,
} from "../Repository/formsRepository";

export async function submitVendorOffer(offerData) {
    try {
        const response = await createVendorOffer(offerData);
        return response.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error occurred" };
    }
}

export async function submitVolunteerOffer(offerData) {
    try {
        const response = await createVolunteerOffer(offerData);
        return response.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error occurred" };
    }
}

export async function fetchCharityApplications(id) {
    try {
        const response = await getCharityApplications(id);
        return response.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error occurred" };
    }
}

export async function fetchApplicationDetails(id, type) {
    try {
        const response = await getOneCharityApplication(id, type);
        return response.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error occurred" };
    }
}
