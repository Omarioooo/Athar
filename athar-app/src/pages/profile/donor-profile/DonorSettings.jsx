import { motion, AnimatePresence } from "framer-motion";
import { IoCameraOutline } from "react-icons/io5";
import ProfileImg from "../../../assets/images/profile.png";
import { UseAuth } from "../../../Auth/Auth";
import { useEffect, useState } from "react";
import {
    deleteDonor,
    getDonorInfo,
    UpdateDonorData,
} from "../../../services/donorService";
import { useNavigate } from "react-router-dom";

export default function DonorSettings() {
    const { user, LogOut } = UseAuth();
    const navigate = useNavigate();

    const [donor, setDonor] = useState(null);
    const [loading, setLoading] = useState(true);
    const [previewImage, setPreviewImage] = useState(ProfileImg);
    const [isEditing, setIsEditing] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);

    const [formData, setFormData] = useState({
        FirstName: "",
        LastName: "",
        Country: "",
        City: "",
        profileImage: null,
    });

    useEffect(() => {
        if (!user) return;

        const fetchData = async () => {
            try {
                const data = await getDonorInfo(user.id);
                setDonor(data);

                setFormData({
                    FirstName: data.firstName || "",
                    LastName: data.lastName || "",
                    Country: data.country || "",
                    City: data.city || "",
                    profileImage: null,
                });

                setPreviewImage(data.imageUrl || ProfileImg);
            } catch (err) {
                console.error("Failed to fetch donor info", err);
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
        try {
            setLoading(true);
            await UpdateDonorData(user.id, formData);
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
            await deleteDonor(user.id);

            LogOut();
            navigate("/login", { replace: true });
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

    const name = `${formData.FirstName} ${formData.LastName}`;

    return (
        <>
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
                                    alt="الصورة الشخصية"
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
                                <h2 className="name">{name}</h2>
                                <p className="email">{donor.email}</p>
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

                    {/* FORM */}
                    <div className="settings-main">
                        <div className="form">
                            <label>الاسم الأول</label>
                            <input
                                name="FirstName"
                                value={formData.FirstName}
                                onChange={handleChange}
                                placeholder="اكتب الاسم الأول"
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>الاسم الثاني</label>
                            <input
                                name="LastName"
                                value={formData.LastName}
                                onChange={handleChange}
                                placeholder="اكتب الاسم الثاني"
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>الدولة</label>
                            <input
                                name="Country"
                                value={formData.Country}
                                onChange={handleChange}
                                placeholder="اكتب دولتك"
                                disabled={!isEditing}
                            />
                        </div>

                        <div className="form">
                            <label>المدينة</label>
                            <input
                                name="City"
                                value={formData.City}
                                onChange={handleChange}
                                placeholder="اكتب مدينتك"
                                disabled={!isEditing}
                            />
                        </div>
                    </div>
                </div>
            </motion.div>
            {/* DELETE BUTTON */}
            {/* <button
                        onClick={() => setShowConfirm(true)}
                        className="rm-btn btn"
                    >
                        حذف الحساب
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
                            <p>سيتم حذف الحساب نهائيًا ولا يمكن التراجع عن هذا الإجراء.</p>

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
                    </motion.div>
                )}
            </AnimatePresence> */}
        </>
    );
}
