import api from "../Auth/AxiosInstance";

export const CreatePayment = async (formData) => {
    const res = await api.post(`/Payments/create`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
    return res.data;
};


export const PaymentCallback = async (formData) => {
    const res = await api.post(`/Payments/callback`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
    return res.data;
};