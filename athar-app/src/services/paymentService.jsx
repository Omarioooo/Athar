import { CreatePayment, PaymentCallback } from "../Repository/paymentRepository";

export const CreatePaymentService = async (formData) => {
    try {
        const campaign = await CreatePayment(formData);
        return campaign;
    } catch (error) {
        throw new Error("حدث خطأ أثناء الدفع: " + error.message);
    }
};


export const CreatePaymentCallBack = async (formData) => {
    try {
        const campaign = await PaymentCallback(formData);
        return campaign;
    } catch (error) {
        throw new Error("حدث خطأ أثناء الدفع: " + error.message);
    }
};
