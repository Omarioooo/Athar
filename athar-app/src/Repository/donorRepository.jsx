import api from "../Auth/AxiosInstance";

// register donor
export function donorRegister(formData) {
    return api.post("/Account/DonorRegister", formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
}


// delete donor account
export async function deleteDonorById(id){
    try {
        const res = await api.delete(`/Donor/delete/${id}`);
        return res.data;
    } catch (error) {
        const msg =
            error.response?.data?.message || "فشل حذف الملف الشخصي";
        throw new Error(msg);
    }
}


// get donor profile
export async function fetchDonorByIdFromApi(id) {
    try {
        const res = await api.get(`/Donor/donorProfile/${id}`);

        return res.data;
    } catch (error) {
        const msg =
            error.response?.data?.message || "فشل جلب بيانات الملف الشخصي";
        throw new Error(msg);
    }
}

// get donor athar
export async function fetchDonorAtharById(id) {
    try {
        const res = await api.get(`/Donor/athar/${id}`);
        return res.data;
    } catch (error) {
        const msg =
            error.response?.data?.message || "فشل جلب بيانات المتبرع";
        throw new Error(msg);
    }

}
// get donor athar
export async function fetchDonorInfoById(id) {
    try {
        const res = await api.get(`/Donor/info/${id}`);
        return res.data;
    } catch (error) {
        const msg =
            error.response?.data?.message || "فشل جلب بيانات المتبرع";
        throw new Error(msg);
    }
}

// update the user profile
export const updateDonor = async (id, data) => {
    const formData = new FormData();

    if (data.name !== undefined && data.name !== null) {
        formData.append("FirstName", data.name);
    }
    if (data.description !== undefined && data.description !== null) {
        formData.append("LastName", data.description);
    }
    if (data.country !== undefined && data.country !== null) {
        formData.append("Country", data.country);
    }
    if (data.city !== undefined && data.city !== null) {
        formData.append("City", data.city);
    }
    if (data.profileImage) {
        formData.append("Image", data.profileImage);
    }

    Object.keys(data).forEach((key) => {
        if (data[key] !== undefined && data[key] !== null) {
            formData.append(key, data[key]);
        }
    });

    const res = await api.put(`/Donor/update/${id}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });

    return res.data;
};
