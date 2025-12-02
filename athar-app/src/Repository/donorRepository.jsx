import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:5192",
    withCredentials: true,
});

export function donorRegister(formData) {
    return api.post("/api/Account/DonorRegister", formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
}

export const fetchDonorByIdFromApi = async (id) => {
    const res = await api.get(`/donor/${id}`);
    return res.data;
};