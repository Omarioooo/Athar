import api from "../Auth/AxiosInstance";

export function createVendorOffer(data) {
    return api.post("/VendorOffers/apply", data);
}

export function createVolunteerOffer(data) {
    return api.post("/VolunteerApplications/apply", data);
}