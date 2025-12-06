import { X } from "lucide-react";
import { useEffect, useState } from "react";
import { submitVolunteerOffer } from "../../../services/formsService";
import { UseAuth } from "../../../Auth/Auth";

export default function VolunteerModalMenu({ closeModal, id }) {
    const { user } = UseAuth();

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

    // ⭐⭐ أضف هذه الدالة المفقودة ⭐⭐
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === "checkbox" ? checked : value,
        });
    };

    // تحقق من البيانات عند فتح المودال
    useEffect(() => {
        console.log("🔍 فتح مودال التطوع:");
        console.log("- معرف المستخدم:", user?.id);
        console.log("- معرف الجمعية:", id);
        console.log("- نوع id:", typeof id);

        // تحقق من id الجمعية
        if (!id || isNaN(Number(id)) || Number(id) <= 0) {
            setErrorMsg("خطأ: معرف الجمعية غير صالح");
        }

        // تحقق من تسجيل الدخول
        if (!user || !user.id) {
            setErrorMsg("يجب تسجيل الدخول أولاً لتقديم طلب التطوع");
        }
    }, [id, user]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        // تحقق من تسجيل الدخول
        if (!user || !user.id) {
            setErrorMsg("يجب تسجيل الدخول أولاً لتقديم طلب التطوع");
            return;
        }

        // تحقق من id الجمعية
        const charityIdNum = Number(id);
        if (isNaN(charityIdNum) || charityIdNum <= 0) {
            setErrorMsg("معرف الجمعية غير صالح");
            return;
        }

        setLoading(true);
        setErrorMsg("");

        try {
            // التحقق من البيانات المطلوبة
            if (!formData.firstName.trim() || !formData.lastName.trim()) {
                throw new Error("الاسم الأول واسم العائلة مطلوبان");
            }

            if (!formData.phoneNumber.trim()) {
                throw new Error("رقم الهاتف مطلوب");
            }

            if (!formData.country.trim() || !formData.city.trim()) {
                throw new Error("الدولة والمدينة مطلوبان");
            }

            const ageNum = Number(formData.age);
            if (isNaN(ageNum) || ageNum < 16 || ageNum > 100) {
                throw new Error("العمر يجب أن يكون بين 16 و 100 سنة");
            }

            const dataToSend = {
                id: user.id, // معرف المستخدم
                firstName: formData.firstName.trim(),
                lastName: formData.lastName.trim(),
                age: ageNum,
                phoneNumber: formData.phoneNumber.trim(),
                country: formData.country.trim(),
                city: formData.city.trim(),
                isFirstTime: formData.isFirstTime,
                charityId: charityIdNum, // معرف الجمعية
                date: new Date().toISOString(),
            };

            console.log("📤 البيانات المرسلة إلى الخادم:");
            console.log(JSON.stringify(dataToSend, null, 2));

            await submitVolunteerOffer(dataToSend);

            // نجاح - إعادة تعيين الفورم
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
            console.error("❌ خطأ في الإرسال:", err);

            // عرض رسالة خطأ منظمة
            if (err.response?.data) {
                const errorData = err.response.data;

                if (errorData.errors) {
                    // أخطاء ModelState من الخادم
                    const errorMessages = Object.values(errorData.errors)
                        .flat()
                        .join("\n");
                    setErrorMsg(`أخطاء في البيانات:\n${errorMessages}`);
                } else if (errorData.Message) {
                    // رسالة خطأ مخصصة
                    setErrorMsg(errorData.Message);
                } else {
                    setErrorMsg("حدث خطأ في الخادم");
                }
            } else if (err.message) {
                // خطأ من throw new Error
                setErrorMsg(err.message);
            } else {
                setErrorMsg("حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى");
            }
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
                    <p className="text-sm text-gray-600 mt-1">
                        تقديم طلب التطوع للجمعية #{id}
                    </p>
                </div>

                {errorMsg && (
                    <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded mb-4">
                        <p className="font-medium">خطأ!</p>
                        <p className="text-sm mt-1 whitespace-pre-line">
                            {errorMsg}
                        </p>
                    </div>
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
                                minLength={2}
                                maxLength={50}
                                placeholder="أدخل الاسم الأول"
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
                                minLength={2}
                                maxLength={50}
                                placeholder="أدخل اسم العائلة"
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
                            placeholder="أدخل عمرك"
                        />
                        <small className="text-gray-500 text-xs">
                            يجب أن يكون العمر بين 16 و 100 سنة
                        </small>
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
                            pattern="[0-9]{10,15}"
                            placeholder="مثال: 0123456789"
                            title="الرجاء إدخال رقم هاتف صحيح (10-15 رقم)"
                        />
                        <small className="text-gray-500 text-xs">
                            أدخل رقم هاتفك (10-15 رقماً)
                        </small>
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
                                minLength={2}
                                maxLength={50}
                                placeholder="أدخل اسم الدولة"
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
                                minLength={2}
                                maxLength={50}
                                placeholder="أدخل اسم المدينة"
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
                                className="rounded"
                            />
                            <span className="text-gray-700">
                                هذه أول مرة أتطوع فيها
                            </span>
                        </label>
                    </div>

                    <div className="mt-6 pt-4 border-t border-gray-200">
                        <button
                            type="submit"
                            className="submit-btn w-full py-3"
                            disabled={loading}
                        >
                            {loading ? (
                                <>
                                    <span className="inline-block animate-spin rounded-full h-4 w-4 border-t-2 border-b-2 border-white mr-2"></span>
                                    جاري الإرسال...
                                </>
                            ) : (
                                "إرسال طلب التطوع"
                            )}
                        </button>

                        <p className="text-xs text-gray-500 mt-3 text-center">
                            بالتسجيل، أنت توافق على شروط التطوع وسياسة الخصوصية
                        </p>
                    </div>
                </form>
            </div>
        </div>
    );
}
