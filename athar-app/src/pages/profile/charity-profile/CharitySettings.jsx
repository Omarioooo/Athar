import { useEffect, useState } from "react";
import { AnimatePresence, motion } from "framer-motion";
import { IoCameraOutline } from "react-icons/io5";
import ProfileImg from "../../../assets/images/profile.png";
import {
    CharityStatus,
    deleteCharity,
    getCharityProfile,
    UpdateCharityData,
} from "../../../services/charityService";
import { UseAuth } from "../../../Auth/Auth";
import { Navigate } from "react-router-dom";

export default function CharitySettings() {
    const { user, logout } = UseAuth();
    const [charity, setCharity] = useState(null);
    const [loading, setLoading] = useState(true);
    const [previewImage, setPreviewImage] = useState(ProfileImg);
    const [isEditing, setIsEditing] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);

    const [formData, setFormData] = useState({
        name: "",
        description: "",
        country: "",
        city: "",
        profileImage: null,
    });

    const statusArabicById = {
        1: "قيد المراجعة",
        2: "مفعلة",
        3: "مرفوضة",
    };

    useEffect(() => {
        if (!user) return;

        const fetchData = async () => {
            try {
                const data = await getCharityProfile(user.id);
                setCharity(data);

                const [country, city] = data.address
                    ? data.address.split(",").map((s) => s.trim())
                    : ["", ""];

                setFormData({
                    name: data.name || "",
                    description: data.description || "",
                    country,
                    city,
                    profileImage: null,
                });

                setPreviewImage(data.imageUrl || ProfileImg);
            } catch (err) {
                console.error("Failed to fetch charity profile", err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [user]);


    const handleChange = (e) => {
        const { name, value, files } = e.target;
        if (name === "profileImage" && files?.[0]) {
            setFormData((prev) => ({ ...prev, profileImage: files[0] }));
            setPreviewImage(URL.createObjectURL(files[0]));
        } else {
            setFormData((prev) => ({ ...prev, [name]: value }));
        }
    };

    const handleSave = async () => {
        if (!charity) return;
        try {
            setLoading(true);
            await UpdateCharityData(user.id, formData);
            setIsEditing(false);
        } catch (err) {
            console.error(err);
            alert("فشل تحديث البيانات");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        try {
            setLoading(true);
            await deleteCharity(user.id);

            logout();
            Navigate("/login", { replace: true });
        } catch (err) {
            console.error(err);
            alert("فشل حذف الحساب");
        } finally {
            setShowConfirm(false);
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="d-flex justify-content-center py-5">
                <div
                    className="spinner-border text-warning"
                    style={{ width: "4rem", height: "4rem" }}
                ></div>
            </div>
        );
    }

    return (
        <>
            {charity.status === 1 && (
                <div className="pending-overlay">
                    <div className="overlay-box">
                        <h2>🚧 الجمعية قيد المراجعة</h2>
                        <p>
                            طلب انضمامك تحت المراجعة الآن. سيتم التواصل معك عند
                            الانتهاء.
                        </p>
                        <button
                            className="overlay-button"
                            onClick={() => Navigate("/")}
                        >
                            الذهاب للصفحة الرئيسية
                        </button>
                    </div>
                </div>
            )}

            {charity.status === 3 && (
                <div className="rejected-overlay">
                    <div className="overlay-box">
                        <h2>❌ تم رفض الطلب</h2>
                        <p>
                            نأسف، تم رفض طلب تسجيل الجمعية. يمكنك التواصل معنا
                            لمزيد من التفاصيل.
                        </p>
                        <button
                            className="overlay-button"
                            onClick={() => Navigate("/")}
                        >
                            الذهاب للصفحة الرئيسية
                        </button>
                    </div>
                </div>
            )}

            <motion.div
                className="settings-wrapper"
                animate={{ opacity: 1 }}
                initial={{ opacity: 0 }}
                exit={{ opacity: 0 }}
                transition={{ duration: 0.25 }}
            >
                <div className="settings-card">
                    {/* HEADER */}
                    <div className="settings-header">
                        <div className="profile">
                            <div className="image">
                                <img
                                    src={previewImage}
                                    alt="صورة الجمعية"
                                    className="avatar"
                                />
                                {isEditing && (
                                    <label>
                                        <IoCameraOutline />
                                        <input
                                            type="file"
                                            name="profileImage"
                                            onChange={handleChange}
                                            style={{ display: "none" }}
                                        />
                                    </label>
                                )}
                            </div>
                            <div className="profile-info">
                                <h2 className="name">{formData.name}</h2>
                            </div>
                        </div>
                        <div className="profile-btns">
                            <button
                                className="edit-btn profile-btn"
                                onClick={() => setIsEditing(true)}
                                disabled={isEditing}
                            >
                                تعديل
                            </button>
                            <button
                                className="save-btn profile-btn"
                                onClick={handleSave}
                                disabled={!isEditing || loading}
                            >
                                {loading ? "جارٍ الحفظ..." : "حفظ"}
                            </button>
                        </div>
                    </div>

                    {/* MAIN */}
                    <div className="settings-main">
                        <div className="form full-row">
                            <label>اسم الجمعية</label>
                            <input
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder="اكتب اسم الجمعية"
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form full-row">
                            <label>الوصف</label>
                            <textarea
                                className="description-textarea"
                                name="description"
                                value={formData.description}
                                onChange={handleChange}
                                placeholder="وصف الجمعية"
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>الدولة</label>
                            <input
                                name="country"
                                value={formData.country}
                                onChange={handleChange}
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>المدينة</label>
                            <input
                                name="city"
                                value={formData.city}
                                onChange={handleChange}
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>الرصيد</label>
                            <input
                                value={`${
                                    charity.totalRaised?.toString() || 0
                                } ج.م`}
                                disabled
                            />
                        </div>

                        <div className="form">
                            <label>عدد المتابعين</label>
                            <input
                                value={charity.followersCount || 0}
                                disabled
                            />
                        </div>

                        <div className="form">
                            <label>عدد الحملات</label>
                            <input
                                value={charity.campaignsCount || 0}
                                disabled
                            />
                        </div>

                        <div className="form">
                            <label>الحالة</label>
                            <input
                                value={statusArabicById[charity.status]}
                                disabled
                            />
                        </div>
                    </div>
                </div>
            </motion.div>
            {/* DELETE BUTTON
                    <button
                        onClick={() => setShowConfirm(true)}
                        className="rm-btn btn"
                    >
                        حذف الجمعية
                    </button>
            <AnimatePresence>
                {showConfirm && (
                    <motion.div
                        className="confirm-overlay"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                    >
                        <motion.div
                            className="confirm-box"
                            initial={{ scale: 0.8, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            exit={{ scale: 0.8, opacity: 0 }}
                            transition={{ duration: 0.2 }}
                        >
                            <h3>هل أنت متأكد؟</h3>
                            <p>
                                سيتم حذف الحساب نهائيًا ولا يمكن التراجع عن هذا
                                الإجراء.
                            </p>

                            <div className="confirm-btns">
                                <button
                                    className="cancel-btn"
                                    onClick={() => setShowConfirm(false)}
                                >
                                    إلغاء
                                </button>

                                <button
                                    className="delete-btn"
                                    onClick={handleDelete}
                                    disabled={loading}
                                >
                                    {loading ? "جاري الحذف..." : "حذف"}
                                </button>
                            </div>
                        </motion.div>
                        )}
                        </motion.div>
                </AnimatePresence> 
                        */}
        </>
    );
}
