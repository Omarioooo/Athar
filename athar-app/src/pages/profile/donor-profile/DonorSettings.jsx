import { motion } from "framer-motion";
import { IoCameraOutline } from "react-icons/io5";
import ProfileImg from "../../../assets/images/profile.png";
import { info } from "../../../utils/data";

export default function DonorSettings() {
    return (
        <motion.div
            className="settings-wrapper"
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            <div className="settings-card">
                <div className="settings-header">
                    <div className="profile">
                        <div className="image">
                            <img
                                src={ProfileImg}
                                alt="الصورة الشخصية"
                                className="avatar"
                            />
                            <IoCameraOutline />
                        </div>
                        <div className="profile-info">
                            <h2 className="name">عمر محمد</h2>
                            <p className="email">omar@gmail.com</p>
                        </div>
                    </div>
                    <div className="profile-btns">
                        <button className="edit-btn profile-btn">تعديل</button>
                        <button className="save-btn profile-btn">حفظ</button>
                    </div>
                </div>

                <div className="settings-main">
                    <div className="form">
                        <label>الاسم الأول</label>
                        <input
                            value={info.first_name}
                            placeholder="اكتب الاسم الأول"
                            disabled
                        />
                    </div>

                    <div className="form">
                        <label>الاسم الثاني</label>
                        <input
                            value={info.last_name}
                            placeholder="اكتب الاسم الثاني"
                            disabled
                        />
                    </div>

                    <div className="form">
                        <label>الدولة</label>
                        <input
                            value={info.country}
                            placeholder="اكتب دولتك"
                            disabled
                        />
                    </div>

                    <div className="form">
                        <label>المدينة</label>
                        <input
                            value={info.city}
                            placeholder="اكتب مدينتك"
                            disabled
                        />
                    </div>

                    <div className="form">
                        <label>البريد الإلكتروني</label>
                        <input
                            value={info.email}
                            placeholder="اكتب بريدك الإلكتروني"
                            disabled
                        />
                    </div>

                    <div className="form">
                        <label>كلمة المرور</label>
                        <input
                            value={info.password}
                            placeholder="اكتب كلمة المرور"
                            disabled
                        />
                    </div>
                </div>
                <button className="rm-btn btn">حذف الحساب</button>
            </div>
        </motion.div>
    );
}
