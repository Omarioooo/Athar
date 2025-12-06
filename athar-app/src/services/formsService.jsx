import {
    createVendorOffer,
    createVolunteerOffer,
    getAllOffers,
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

export async function getAllCharityOffers(id) {
    try {
        const response = await getAllOffers(id);
        return response.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error occurred" };
    }
}
