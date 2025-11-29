import axios from 'axios';
import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";

export default function CharityRegister() {
  const navigate = useNavigate();
  const [user, setUser] = useState({
    email: "",
    charityname: "",
    password: "",
    confirmpassword: "",
    charitydescription: "",
    country: "",
    city: "",
    documentation: "",
    img: ""
  });

  const [error, seterror] = useState({
    emailerror: null,
    passworderror: null,
    charitynameerror: null,
    confirmpassworderror: null,
    charitydescriptionerror: null,
  });

  const [serverError, setServerError] = useState(""); // <-- state لرسائل السيرفر

  const handleChange = (ev) => {
    const fieldname = ev.target.name;
    const fieldvalue = ev.target.value;

    if (fieldname === "password") {
      setUser({ ...user, password: fieldvalue });
      let errors = '';
      if (fieldvalue.length === 0) errors = 'هذا الحقل مطلوب';
      seterror({ ...error, passworderror: errors });

      if (user.confirmpassword && fieldvalue !== user.confirmpassword) {
        seterror(prev => ({ ...prev, confirmpassworderror: "كلمة المرور غير متطابقة" }));
      } else {
        seterror(prev => ({ ...prev, confirmpassworderror: "" }));
      }
    }

    if (fieldname === "charityname") {
      setUser({ ...user, charityname: fieldvalue });
      let errors = '';
      if (fieldvalue.length === 0) errors = 'هذا الحقل مطلوب';

      seterror({ ...error, charitynameerror: errors });
    }

    if (fieldname === "document") setUser({ ...user, documentation: ev.target.files[0] });
    if (fieldname === "img") setUser({ ...user, img: ev.target.files[0] });

    if (fieldname === "confirmpassword") {
      setUser({ ...user, confirmpassword: fieldvalue });
      if (fieldvalue !== user.password) {
        seterror(prev => ({ ...prev, confirmpassworderror: "كلمة المرور غير متطابقة" }));
      } else {
        seterror(prev => ({ ...prev, confirmpassworderror: "" }));
      }
    }

    if (fieldname === "charitydescription") setUser({ ...user, charitydescription: fieldvalue });
    if (fieldname === "country") setUser({ ...user, country: fieldvalue });
    if (fieldname === "city") setUser({ ...user, city: fieldvalue });

    if (fieldname === "email") {
      setUser({ ...user, email: fieldvalue });
      let errors = '';
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (fieldvalue.length === 0) errors = 'هذا الحقل مطلوب';
      else if (!emailRegex.test(fieldvalue)) errors = 'الايميل غير صالح';
      seterror({ ...error, emailerror: errors });
    }
  }

  const isFormValid = () => {
    return (
      user.charitydescription &&
      user.charityname &&
      user.city &&
      user.confirmpassword &&
      user.country &&
      user.documentation &&
      user.email &&
      user.password &&
      !error.charitydescriptionerror &&
      !error.charitynameerror &&
      !error.confirmpassworderror &&
      !error.emailerror &&
      !error.passworderror
    );
  }

  const handleSubmit = async (ev) => {
    ev.preventDefault();
    setServerError(""); // مسح أي رسالة خطأ سابقة

    if (!isFormValid()) {
      setServerError("الرجاء تصحيح الأخطاء قبل الإرسال");
      return;
    }

    if (!user.img || !user.documentation) {
      setServerError("يرجى اختيار الملفات المطلوبة!");
      return;
    }

    const formdata = new FormData();
    formdata.append("Email", user.email);
    formdata.append("CharityName", user.charityname);
    formdata.append("Password", user.password);
    formdata.append("Description", user.charitydescription);
    formdata.append("Country", user.country);
    formdata.append("City", user.city);
    if (user.img) formdata.append("ProfileImage", user.img);
    if (user.documentation) formdata.append("VerificationDocument", user.documentation);
    formdata.append("Role", "Charity");

    try {
      await axios.post('https://localhost:44389/api/Account/CharityRegister', formdata);
      navigate("/login");
    } catch (err) {
      console.log(err);
      if (err.response && err.response.data && err.response.data.message) {
        setServerError(err.response.data.message); // <-- رسالة السيرفر
      } else {
        setServerError("حدث خطأ أثناء التسجيل");
      }
    }
  }

  return (
    <div className='charityregister-section'>
      <div className="register-idiv">
        <i className="fa-regular fa-building register-i"></i>
      </div>
      <div className='charityregister-media'>
        <h3>تسجيل حساب جمعية خيرية</h3>
        <p>ادخل معلومات الجمعية لانشاء حساب جديد</p>
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

          <div className='mt-4'>
            <label htmlFor="charityname" className="form-label">اسم الجمعية*</label>
            <input
              type="text"
              style={{ direction: "rtl" }}
              className={`form-control login-input ${error.charitynameerror ? 'border-danger' : ''}`}
              name="charityname"
              value={user.charityname}
              onChange={handleChange}
              placeholder="ادخل اسم الجمعية"
            />
            <small className="text-danger">{error.charitynameerror}</small>
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

          <div className="mt-4">
            <label htmlFor="charitydescription" className="form-label">وصف الجمعية*</label>
            <textarea
              style={{ direction: "rtl" }}
              className={`form-control login-input ${error.charitydescriptionerror ? 'border-danger' : ''}`}
              name="charitydescription"
              value={user.charitydescription}
              onChange={handleChange}
              placeholder="ادخل وصف للجمعية"
              rows={4}
            />
            <small className="text-danger">{error.charitydescriptionerror}</small>
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
            <label htmlFor="document" className="form-label">مستند لتحقق من صحة الجمعية*</label>
            <input
              type="file"
              style={{ direction: "rtl" }}
              className="form-control login-input"
              name="document"
              onChange={handleChange}
            />
            {user.documentation && <small>الملف المختار: {user.documentation.name}</small>}
          </div>

          <div className='mt-4'>
            <label htmlFor="img" className="form-label">صورة شعار للجمعية*</label>
            <input
              type="file"
              style={{ direction: "rtl" }}
              className="form-control login-input"
              name="img"
              onChange={handleChange}
            />
            {user.img && <small>الملف المختار: {user.img.name}</small>}
          </div>

          <button type="submit" className="login-btn mt-4">انشاء حساب</button>
        </form>
      </div>
    </div>
  );
}
