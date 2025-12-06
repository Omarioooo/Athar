import { X } from "lucide-react";
import { useEffect, useState } from "react";
import { submitVolunteerOffer } from "../../../services/formsService";
import { UseAuth } from "../../../Auth/Auth";
import { useNavigate } from "react-router-dom";

export default function VolunteerModalMenu({ closeModal, id }) {
    const { user } = UseAuth();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [errorMsg, setErrorMsg] = useState("");

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        age: "",
        phoneNumber: "",
        country: "",
        city: "",
        isFirstTime: true,
    });

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === "checkbox" ? checked : value,
        });
    };

    useEffect(() => {
        if (!id || isNaN(Number(id)) || Number(id) <= 0) {
            setErrorMsg("خطأ: معرف الجمعية غير صالح");
        }

        if (!user || !user.id) {
            setErrorMsg("يجب تسجيل الدخول أولاً لتقديم طلب التطوع");
            navigate("/login", { replace: true });
        }
    }, [id, user, navigate]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg("");

        const ageNum = Number(formData.age);

        // Validations
        if (!formData.firstName.trim() || !formData.lastName.trim()) {
            setErrorMsg("الاسم الأول واسم العائلة مطلوبان");
            return;
        }
        if (!formData.phoneNumber.trim()) {
            setErrorMsg("رقم الهاتف مطلوب");
            return;
        }
        if (!formData.country.trim() || !formData.city.trim()) {
            setErrorMsg("الدولة والمدينة مطلوبان");
            return;
        }
        if (isNaN(ageNum) || ageNum < 16 || ageNum > 100) {
            setErrorMsg("العمر يجب أن يكون بين 16 و 100 سنة");
            return;
        }

        setLoading(true);

        try {
            const dataToSend = {
                id: user.id,
                firstName: formData.firstName.trim(),
                lastName: formData.lastName.trim(),
                age: ageNum,
                phoneNumber: formData.phoneNumber.trim(),
                country: formData.country.trim(),
                city: formData.city.trim(),
                isFirstTime: formData.isFirstTime,
                charityId: Number(id),
                date: new Date().toISOString(),
            };

            await submitVolunteerOffer(dataToSend);

            setFormData({
                firstName: "",
                lastName: "",
                age: "",
                phoneNumber: "",
                country: "",
                city: "",
                isFirstTime: true,
            });

            closeModal();
        } catch (err) {
            console.error(err);
            setErrorMsg("حدث خطأ أثناء إرسال الطلب. حاول مرة أخرى.");
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
                    <p className="text-red-600 error-msg">
                        {errorMsg}
                    </p>
                )}
                <form className="modal-form" onSubmit={handleSubmit}>
                    <div className="grid grid-cols-2 gap-5">
                        <div className="form-group">
                            <label>
                                الاسم الأول
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
                                اسم العائلة
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
