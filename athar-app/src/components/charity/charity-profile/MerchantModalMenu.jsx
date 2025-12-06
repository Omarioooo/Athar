import { X } from "lucide-react";
import { useState } from "react";
import { submitVendorOffer } from "../../../services/formsService";

export default function MerchantModalMenu({ closeModal, id }) {
    const [formData, setFormData] = useState({
        vendorName: "",
        phoneNumber: "",
        country: "",
        city: "",
        itemName: "",
        quantity: "",
        description: "",
        priceBeforeDiscount: "",
        priceAfterDiscount: "",
        charityId: id,
    });

    const [loading, setLoading] = useState(false);
    const [errorMsg, setErrorMsg] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg("");

        // Validations
        if (!formData.vendorName.trim()) {
            setErrorMsg("اسم التاجر / المتجر مطلوب");
            return;
        }
        if (!formData.phoneNumber.trim()) {
            setErrorMsg("رقم الجوال مطلوب");
            return;
        }
        if (!formData.country.trim()) {
            setErrorMsg("الدولة مطلوبة");
            return;
        }
        if (!formData.city.trim()) {
            setErrorMsg("المدينة مطلوبة");
            return;
        }
        if (!formData.itemName.trim()) {
            setErrorMsg("اسم الصنف / المنتج مطلوب");
            return;
        }
        if (!formData.quantity || parseInt(formData.quantity, 10) <= 0) {
            setErrorMsg("الكمية يجب أن تكون أكبر من 0");
            return;
        }
        if (
            !formData.priceBeforeDiscount ||
            parseFloat(formData.priceBeforeDiscount) < 0
        ) {
            setErrorMsg("السعر قبل الخصم مطلوب ويجب أن يكون ≥ 0");
            return;
        }
        if (
            !formData.priceAfterDiscount ||
            parseFloat(formData.priceAfterDiscount) < 0
        ) {
            setErrorMsg("السعر بعد الخصم مطلوب ويجب أن يكون ≥ 0");
            return;
        }
        if (!formData.description.trim()) {
            setErrorMsg("معلومات إضافية مطلوبة");
            return;
        }

        setLoading(true);

        try {
            const submittedData = {
                vendorName: formData.vendorName.trim(),
                phoneNumber: formData.phoneNumber.trim(),
                country: formData.country.trim(),
                city: formData.city.trim(),
                itemName: formData.itemName.trim(),
                quantity: parseInt(formData.quantity, 10),
                description: formData.description.trim(),
                priceBeforeDiscount: parseFloat(formData.priceBeforeDiscount),
                priceAfterDiscount: parseFloat(formData.priceAfterDiscount),
                charityId: id,
            };

            await submitVendorOffer(submittedData);

            setFormData({
                vendorName: "",
                phoneNumber: "",
                country: "",
                city: "",
                itemName: "",
                quantity: "",
                description: "",
                priceBeforeDiscount: "",
                priceAfterDiscount: "",
                charityId: id,
            });

            closeModal();
        } catch (err) {
            console.error(err);
            setErrorMsg("حدث خطأ أثناء إرسال العرض. حاول مرة أخرى.");
        }

        setLoading(false);
    };

    return (
        <div className="modal-overlay" onClick={closeModal}>
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <button
                    className="modal-close"
                    onClick={closeModal}
                    aria-label="إغلاق النموذج"
                >
                    <X size={22} />
                </button>

                <div className="modal-header">
                    <h2 className="modal-title">عرض تبرع من تاجر</h2>
                </div>

                {errorMsg && (
                    <p className="text-red-600 bg-red-100 p-3 rounded mb-4">
                        {errorMsg}
                    </p>
                )}

                <form className="modal-form" onSubmit={handleSubmit}>
                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="vendorName">
                                اسم التاجر / المتجر{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="vendorName"
                                name="vendorName"
                                required
                                placeholder=""
                                value={formData.vendorName}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="phoneNumber">
                                رقم الجوال{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="tel"
                                id="phoneNumber"
                                name="phoneNumber"
                                required
                                value={formData.phoneNumber}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="country">
                                الدولة <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="country"
                                name="country"
                                required
                                value={formData.country}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="city">
                                المدينة <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="city"
                                name="city"
                                required
                                value={formData.city}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="itemName">
                                اسم الصنف / المنتج{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="itemName"
                                name="itemName"
                                required
                                value={formData.itemName}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="quantity">
                                الكمية <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="number"
                                id="quantity"
                                name="quantity"
                                required
                                min="1"
                                value={formData.quantity}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="priceBeforeDiscount">
                                السعر قبل الخصم
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                id="priceBeforeDiscount"
                                name="priceBeforeDiscount"
                                required
                                min="0"
                                value={formData.priceBeforeDiscount}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="priceAfterDiscount">
                                السعر بعد الخصم
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                id="priceAfterDiscount"
                                name="priceAfterDiscount"
                                required
                                min="0"
                                value={formData.priceAfterDiscount}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label htmlFor="description">
                            معلومات اضافية
                            <span className="text-red-500">*</span>
                        </label>
                        <textarea
                            id="description"
                            name="description"
                            required
                            rows="4"
                            value={formData.description}
                            onChange={handleChange}
                        />
                    </div>

                    <button
                        type="submit"
                        className="submit-btn"
                        disabled={loading}
                    >
                        {loading ? "جاري الإرسال..." : "إرسال عرض التبرع"}
                    </button>
                </form>
            </div>
        </div>
    );
}
