import api from "../Auth/AxiosInstance";

export function donorRegister(formData) {
    return api.post("/Account/DonorRegister", formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
}

export async function fetchDonorByIdFromApi(id) {
    try {
        const res = await api.get(`/donor-profile/${id}`);
        return res.data;
    } catch (error) {
        const msg =
            error.response?.data?.message || "فشل جلب بيانات الملف الشخصي";
        throw new Error(msg);
    }
}
