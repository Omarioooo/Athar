import { X } from "lucide-react";
import { useState } from "react";

export default function VolunteerModalMenu({ closeModal }) {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        age: "",
        phoneNumber: "",
        country: "",
        city: "",
        isFirstTime: true,
        charityVolunteerId: "",
    });

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === "checkbox" ? checked : value,
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log("Volunteer Form Submitted:", formData);
        closeModal();
    };

    return (
        <div className="modal-overlay" onClick={closeModal}>
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <button
                    className="modal-close"
                    onClick={closeModal}
                    aria-label="إغلاق"
                >
                    <X size={22} />
                </button>

                <div className="modal-header">
                    <h2 className="modal-title">تطوع معنا</h2>
                </div>

                <form className="modal-form" onSubmit={handleSubmit}>
                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label htmlFor="firstName">
                                الاسم الأول{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="firstName"
                                name="firstName"
                                required
                                placeholder="أحمد"
                                value={formData.firstName}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="lastName">
                                اسم العائلة{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                id="lastName"
                                name="lastName"
                                required
                                placeholder="محمد"
                                value={formData.lastName}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label htmlFor="age">
                            العمر <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="number"
                            id="age"
                            name="age"
                            required
                            min="16"
                            max="100"
                            placeholder="25"
                            value={formData.age}
                            onChange={handleChange}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="phoneNumber">
                            رقم الجوال <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="tel"
                            id="phoneNumber"
                            name="phoneNumber"
                            required
                            placeholder="05xxxxxxxxx"
                            pattern="^05[0-9]{8}$"
                            value={formData.phoneNumber}
                            onChange={handleChange}
                        />
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
                                placeholder="الرياض"
                                value={formData.city}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label className="flex items-center gap-3 cursor-pointer">
                            <input
                                type="checkbox"
                                name="isFirstTime"
                                checked={formData.isFirstTime}
                                onChange={handleChange}
                                className="w-5 h-5 text-yellow-500 rounded focus:ring-yellow-400"
                            />
                            <span className="select-none">
                                هذه أول مرة أتطوع فيها
                            </span>
                        </label>
                    </div>

                    <input
                        type="hidden"
                        name="charityVolunteerId"
                        value={formData.charityVolunteerId}
                    />

                    <button type="submit" className="submit-btn">
                        إرسال الطلب
                    </button>
                </form>
            </div>
        </div>
    );
}
