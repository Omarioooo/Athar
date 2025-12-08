import api from "../Auth/AxiosInstance";

export function createVendorOffer(data) {
    return api.post("/VendorOffers/apply", data);
}

export function createVolunteerOffer(data) {
    return api.post("/VolunteerApplications/apply", data);
}

export function getCharityApplications(id) {
    return api.get(`/Charities/applications/${id}`);
}

export function getOneCharityApplication(id, type) {
    return api.get(`/Charities/application/${id}`, {
        params: { type },
    });
}
