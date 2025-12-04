import { X } from "lucide-react";
import { useState } from "react";

export default function MerchantModalMenu({ closeModal }) {
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
        charityVendorOfferId: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const submittedData = {
            ...formData,
            quantity: parseInt(formData.quantity, 10),
            priceBeforeDiscount: parseFloat(formData.priceBeforeDiscount),
            priceAfterDiscount: parseFloat(formData.priceAfterDiscount),
            status: "Pending",
        };

        console.log("Merchant Offer Submitted:", submittedData);
        closeModal();
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
                                placeholder="متجر الخير"
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
                                placeholder="05xxxxxxxxx"
                                pattern="^05[0-9]{8}$"
                                title="رقم الجوال يجب أن يبدأ بـ 05 ويتكون من 10 أرقام"
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
                                placeholder="المملكة العربية السعودية"
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
                                placeholder="جدة"
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
                                placeholder="أرز، تمور، ملابس..."
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
                                placeholder="50"
                                value={formData.quantity}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="priceBeforeDiscount">
                                السعر قبل الخصم (ريال){" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                id="priceBeforeDiscount"
                                name="priceBeforeDiscount"
                                required
                                min="0"
                                placeholder="1000.00"
                                value={formData.priceBeforeDiscount}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="priceAfterDiscount">
                                السعر بعد الخصم (ريال){" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="number"
                                step="0.01"
                                id="priceAfterDiscount"
                                name="priceAfterDiscount"
                                required
                                min="0"
                                placeholder="750.00"
                                value={formData.priceAfterDiscount}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label htmlFor="description">
                            وصف العرض (اختياري لكن مطلوب حسب DTO){" "}
                            <span className="text-red-500">*</span>
                        </label>
                        <textarea
                            id="description"
                            name="description"
                            required
                            rows="4"
                            placeholder="تفاصيل العرض، حالة المنتج، مدة الصلاحية إن وجدت..."
                            value={formData.description}
                            onChange={handleChange}
                        />
                    </div>

                    <input
                        type="hidden"
                        name="charityVendorOfferId"
                        value={formData.charityVendorOfferId}
                    />

                    <button type="submit" className="submit-btn">
                        إرسال عرض التبرع
                    </button>
                </form>
            </div>
        </div>
    );
}
