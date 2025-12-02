import { motion } from "framer-motion";
import { IoCameraOutline } from "react-icons/io5";
import ProfileImg from "../../../assets/images/profile.png";
import { charityInfo } from "../../../utils/data";

export default function CharitySettings() {
    return (
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
                                src={charityInfo.profileImage || ProfileImg}
                                alt="صورة الجمعية"
                                className="avatar"
                            />
                            <IoCameraOutline />
                        </div>

                        <div className="profile-info">
                            <h2 className="name">{charityInfo.name}</h2>
                            <p className="email">{charityInfo.status}</p>
                        </div>
                    </div>

                    <div className="profile-btns">
                        <button className="edit-btn profile-btn">تعديل</button>
                        <button className="save-btn profile-btn">حفظ</button>
                    </div>
                </div>

                {/* MAIN */}
                <div className="settings-main">

                    {/* اسم الجمعية - صف منفصل */}
                    <div className="form full-row">
                        <label>اسم الجمعية</label>
                        <input
                            value={charityInfo.name}
                            placeholder="اكتب اسم الجمعية"
                            disabled
                        />
                    </div>

                    {/* الوصف - صف منفصل */}
                    <div className="form full-row">
                        <label>الوصف</label>
                        <textarea
                            className="description-textarea"
                            value={charityInfo.description}
                            placeholder="وصف الجمعية"
                            disabled
                        />
                    </div>

                    {/* باقي الداتا - شبكية */}
                    <div className="form">
                        <label>الدولة</label>
                        <input value={charityInfo.country} disabled />
                    </div>

                    <div className="form">
                        <label>المدينة</label>
                        <input value={charityInfo.city} disabled />
                    </div>

                    <div className="form">
                        <label>الرصيد</label>
                        <input value={`${charityInfo.balance} ج.م`} disabled />
                    </div>

                    <div className="form">
                        <label>عدد المتابعين</label>
                        <input value={charityInfo.followers} disabled />
                    </div>

                    <div className="form">
                        <label>عدد الحملات</label>
                        <input value={charityInfo.campaigns} disabled />
                    </div>

                    <div className="form">
                        <label>الحالة</label>
                        <input value={charityInfo.status} disabled />
                    </div>
                </div>

                <button className="rm-btn btn">حذف الجمعية</button>
            </div>
        </motion.div>
    );
}
