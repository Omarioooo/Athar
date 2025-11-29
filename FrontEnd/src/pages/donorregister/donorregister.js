import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from "react-router-dom";

export default function DonorRegister() {
    const navigate = useNavigate(); 
    const [user, setUser] = useState({
        email: "",
        firstname: "",
        lastname: "",
        password: "",
        confirmpassword: "",
        country: "",
        city: "",
        img: null
    });

    const [error, setError] = useState({
        emailerror: null,
        passworderror: null,
        firstnamerror: null,
        lastnameerror: null,
        confirmpassworderror: null
    });

    const [serverError, setServerError] = useState(""); // <-- state لرسائل السيرفر

    const handleChange = (ev) => {
        const fieldname = ev.target.name;
        const fieldvalue = fieldname === "img" ? ev.target.files[0] : ev.target.value;

        setUser({ ...user, [fieldname]: fieldvalue });

        let errors = '';

        if (fieldname === "email") {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!fieldvalue) errors = 'هذا الحقل مطلوب';
            else if (!emailRegex.test(fieldvalue)) errors = 'الايميل غير صالح';
            setError({ ...error, emailerror: errors });
        }

        if (fieldname === "firstname") {
            if (!fieldvalue) errors = 'هذا الحقل مطلوب';
            else if (fieldvalue.length > 10) errors = 'طول الاسم يجب ان يكون اقل من 10 حروف';
            setError({ ...error, firstnamerror: errors });
        }

        if (fieldname === "lastname") {
            if (!fieldvalue) errors = 'هذا الحقل مطلوب';
            else if (fieldvalue.length > 10) errors = 'طول الاسم يجب ان يكون اقل من 10 حروف';
            setError({ ...error, lastnameerror: errors });
        }

        if (fieldname === "password") {
            if (!fieldvalue) errors = 'هذا الحقل مطلوب';
            else if (fieldvalue.length > 10) errors = 'طول كلمة المرور يجب ان يكون اقل من 10 حروف';
            setError({ ...error, passworderror: errors });

            if (user.confirmpassword && fieldvalue !== user.confirmpassword) {
                setError(prev => ({ ...prev, confirmpassworderror: "كلمة المرور غير متطابقة" }));
            } else {
                setError(prev => ({ ...prev, confirmpassworderror: "" }));
            }
        }

        if (fieldname === "confirmpassword") {
            if (fieldvalue !== user.password) errors = "كلمة المرور غير متطابقة";
            setError({ ...error, confirmpassworderror: errors });
        }
    };

    const isFormValid = () => {
        return (
            user.email &&
            user.firstname &&
            user.lastname &&
            user.password &&
            user.confirmpassword &&
            !error.emailerror &&
            !error.firstnamerror &&
            !error.lastnameerror &&
            !error.passworderror &&
            !error.confirmpassworderror
        );
    };

    const handleSubmit = async (ev) => {
        ev.preventDefault();
        setServerError(""); // مسح أي رسالة خطأ سابقة

        if (!isFormValid()) {
            setServerError("الرجاء تصحيح الأخطاء قبل الإرسال");
            return;
        }

        const formData = new FormData();
        formData.append("Email", user.email);
        formData.append("FirstName", user.firstname);
        formData.append("LastName", user.lastname);
        formData.append("Password", user.password);
        formData.append("Country", user.country);
        formData.append("City", user.city);
        if (user.img) formData.append("ProfileImage", user.img);
        formData.append("Role", "Donor");

        try {
            await axios.post(
                "https://localhost:44389/api/Account/DonorRegister",
                formData,
                { headers: { "Content-Type": "multipart/form-data" } }
            );
            navigate("/login");

        } catch (err) {
            console.log(err);
            if (err.response && err.response.data && err.response.data.message) {
                setServerError(err.response.data.message); // <-- رسالة السيرفر
            } else {
                setServerError("حدث خطأ أثناء التسجيل");
            }
        }
    };

    return (
        <div className='charityregister-section'>
            <div className="register-idiv2">
                <i className="fa-solid fa-circle-user register-i"></i>
            </div>
            <div className='charityregister-media'>
                <h3>تسجيل حساب متبرع</h3>
                <p>ادخل معلوماتك لانشاء حساب جديد</p>
            </div>
            <div className='charityregister-smallsection'>
                {/* عرض رسالة السيرفر */}
                {serverError && <div className="text-danger mb-3">{serverError}</div>}

                <form onSubmit={handleSubmit}>

                    <div className='mt-4'>
                        <label htmlFor="email" className="form-label">البريد الالكترونى*</label>
                        <input
                            type="email"
                            style={{ direction: "rtl" }}
                            className={`form-control login-input ${error.emailerror ? 'border-danger' : ''}`}
                            name="email"
                            value={user.email}
                            onChange={handleChange}
                            placeholder="ادخل الايميل"
                        />
                        <small className="text-danger">{error.emailerror}</small>
                    </div>

                    <div className='row'>
                        <div className="col-md-6 mt-4">
                            <label htmlFor="firstname" className="form-label">الاسم الاول*</label>
                            <input
                                type="text"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${error.firstnamerror ? 'border-danger' : ''}`}
                                name="firstname"
                                value={user.firstname}
                                onChange={handleChange}
                                placeholder="اكتب اسمك"
                            />
                            <small className="text-danger">{error.firstnamerror}</small>
                        </div>

                        <div className="col-md-6 mt-4">
                            <label htmlFor="lastname" className="form-label">الاسم الاخير*</label>
                            <input
                                type="text"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${error.lastnameerror ? 'border-danger' : ''}`}
                                name="lastname"
                                value={user.lastname}
                                onChange={handleChange}
                                placeholder="اكتب اسمك الاخير"
                            />
                            <small className="text-danger">{error.lastnameerror}</small>
                        </div>
                    </div>

                    <div className='row'>
                        <div className="col-md-6 mt-4">
                            <label htmlFor="password" className="form-label">كلمة المرور*</label>
                            <input
                                type="password"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${error.passworderror ? 'border-danger' : ''}`}
                                name="password"
                                value={user.password}
                                onChange={handleChange}
                                placeholder="......."
                            />
                            <small className="text-danger">{error.passworderror}</small>
                        </div>

                        <div className="col-md-6 mt-4">
                            <label htmlFor="confirmpassword" className="form-label">تاكيد كلمة المرور*</label>
                            <input
                                type="password"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${error.confirmpassworderror ? 'border-danger' : ''}`}
                                name="confirmpassword"
                                value={user.confirmpassword}
                                onChange={handleChange}
                                placeholder="........"
                            />
                            <small className="text-danger">{error.confirmpassworderror}</small>
                        </div>
                    </div>

                    <div className='row'>
                        <div className="col-md-6 mt-4">
                            <label htmlFor="country" className="form-label">الدولة*</label>
                            <input
                                type="text"
                                style={{ direction: "rtl" }}
                                className="form-control login-input"
                                name="country"
                                value={user.country}
                                onChange={handleChange}
                                placeholder="مصر"
                            />
                        </div>

                        <div className="col-md-6 mt-4">
                            <label htmlFor="city" className="form-label">المدينة*</label>
                            <input
                                type="text"
                                style={{ direction: "rtl" }}
                                className="form-control login-input"
                                name="city"
                                value={user.city}
                                onChange={handleChange}
                                placeholder="القاهرة"
                            />
                        </div>
                    </div>

                    <div className='mt-4'>
                        <label htmlFor="img" className="form-label">صورة شخصية(اختيارى)</label>
                        <input
                            type="file"
                            style={{ direction: "rtl" }}
                            className="form-control login-input"
                            name="img"
                            onChange={handleChange}
                        />
                        {user.img && <small>الملف المختار: {user.img.name}</small>}
                    </div>

                    <button type="submit" className="login-btn mt-3">انشاء حساب</button>

                </form>
            </div>
        </div>
    );
}
