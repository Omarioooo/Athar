import { X } from "lucide-react";
import { useState } from "react";
import { submitVolunteerOffer } from "../../../services/formsService";

export default function VolunteerModalMenu({ closeModal, id }) {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        age: "",
        phoneNumber: "",
        country: "",
        city: "",
        isFirstTime: true,
        charityVolunteerId: id,
    });

    const [loading, setLoading] = useState(false);
    const [errorMsg, setErrorMsg] = useState("");

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === "checkbox" ? checked : value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setErrorMsg("");

        try {
            console.log("data form ,",formData);
            await submitVolunteerOffer(formData);
            
            closeModal();
        } catch (err) {
            setErrorMsg(err.message || "حدث خطأ، حاول لاحقًا");
        }

        setLoading(false);
    };

    return (
        <div className="modal-overlay" onClick={closeModal}>
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={closeModal}>
                    <X size={22} />
                </button>

                <div className="modal-header">
                    <h2 className="modal-title">تطوع معنا</h2>
                </div>

                {errorMsg && (
                    <p className="text-red-600 bg-red-100 p-3 rounded mb-4">
                        {errorMsg}
                    </p>
                )}

                <form className="modal-form" onSubmit={handleSubmit}>
                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label>
                                الاسم الأول{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                name="firstName"
                                required
                                value={formData.firstName}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label>
                                اسم العائلة{" "}
                                <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                name="lastName"
                                required
                                value={formData.lastName}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label>
                            العمر <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="number"
                            name="age"
                            min="16"
                            max="100"
                            required
                            value={formData.age}
                            onChange={handleChange}
                        />
                    </div>

                    <div className="form-group">
                        <label>
                            رقم الهاتف <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="tel"
                            name="phoneNumber"
                            required
                            value={formData.phoneNumber}
                            onChange={handleChange}
                        />
                    </div>

                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label>
                                الدولة <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                name="country"
                                required
                                value={formData.country}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-group">
                            <label>
                                المدينة <span className="text-red-500">*</span>
                            </label>
                            <input
                                type="text"
                                name="city"
                                required
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
                            />
                            <span>هذه أول مرة أتطوع فيها</span>
                        </label>
                    </div>
                    <div className="form-group">
                        <input
                            type="hidden"
                            name="charityVolunteerId"
                            value={formData.charityVolunteerId}
                        />
                    </div>

                    <button
                        type="submit"
                        className="submit-btn"
                        disabled={loading}
                    >
                        {loading ? "جاري الإرسال..." : "إرسال الطلب"}
                    </button>
                </form>
            </div>
        </div>
    );
}
