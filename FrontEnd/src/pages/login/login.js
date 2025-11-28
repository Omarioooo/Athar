import axios from 'axios';
import React, { useState } from 'react';
import { Link, useNavigate } from "react-router-dom";
import { UseAuth } from '../../Auth/Auth';
import {jwtDecode} from "jwt-decode";

const Login = () => {
  const auth = UseAuth();
  const navigate = useNavigate();
  const [user, setUser] = useState({
    email: "",
    password: ""
  });

  const [error, setError] = useState({
    emailerror: null,
    passworderror: null
  });

  const handleChange = (ev) => {
    const fieldname = ev.target.name;
    const fieldvalue = ev.target.value;

    if (fieldname === "password") {
      setUser({ ...user, password: fieldvalue });
      let errors = '';
      if (fieldvalue.length === 0) {
        errors = 'هذا الحقل مطلوب';
      } else if (fieldvalue.length > 10) {
        errors = 'طول الباسورد يجب أن يكون أقل من 10 حروف';
      }
      setError({ ...error, passworderror: errors });
    }

    if (fieldname === "email") {
      setUser({ ...user, email: fieldvalue });
      let errors = '';
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (fieldvalue.length === 0) {
        errors = 'هذا الحقل مطلوب';
      } else if (!emailRegex.test(fieldvalue)) {
        errors = 'الايميل غير صالح';
      }
      setError({ ...error, emailerror: errors });
    }
  };

  const handleLogin = async (ev) => {
    ev.preventDefault();
  
    if (error.emailerror || error.passworderror) {
      return;
    }

    try {
      const response = await axios.post(
        "https://localhost:44389/api/Account/Login",
        {
          UserNameOrEmail: user.email,
          Password: user.password
        },
        {
          headers: { "Content-Type": "application/json" }
        }
      );

      const token = response.data.token;
      const decoded = jwtDecode(token);
      const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      document.cookie = `token=${token}; path=/`;
      document.cookie = `role=${role}; path=/`;

      auth.login({
        email: user.email,
        token: token,
        role: role
      });

      navigate("/", { replace: true });
    } catch (err) {
      console.error("Login error:", err);

      // تنظيف الأخطاء السابقة
      setError({ emailerror: null, passworderror: null });

      if (err.response && err.response.data) {
        const msg = err.response.data.message || err.response.data.error || "خطأ غير معروف";

        if (msg.toLowerCase().includes("password")) {
          setError({ emailerror: null, passworderror: "كلمة المرور غير صحيحة" });
        } else if (msg.toLowerCase().includes("email")) {
          setError({ emailerror: "الايميل غير صحيح", passworderror: null });
        } else {
          console.error("Server message:", msg);
        }
      } else {
        console.error("خطأ في الاتصال بالسيرفر");
      }
    }
  };

  return (
    <div className="login-section">
      <h1 className='login-h1'>منصة اثر</h1>
      <p className='login-p'>منصة العطاء والتبرعات</p>
      <div className='login-smallsection'>
        <form onSubmit={handleLogin}>
          <h4 className='login-h4'>تسجيل الدخول</h4>
          <p className='login-p2'>ادخل بياناتك للوصول الى حسابك</p>

          <div className="mb-3">
            <label htmlFor="email" className="form-label">الايميل</label>
            <input
              type="text"
              style={{ direction: "rtl" }}
              className={`form-control login-input ${error.emailerror ? 'border-danger' : ''}`}
              name="email"
              value={user.email}
              onChange={handleChange}
              placeholder="ادخل الايميل"
            />
            <small className="text-danger">{error.emailerror}</small>
          </div>

          <div className="mb-3">
            <label htmlFor="password" className="form-label">كلمة المرور</label>
            <input
              type="password"
              style={{ direction: "rtl" }}
              className={`form-control login-input ${error.passworderror ? 'border-danger' : ''}`}
              name="password"
              value={user.password}
              onChange={handleChange}
              placeholder="ادخل كلمة المرور"
            />
            <small className="text-danger">{error.passworderror}</small>
          </div>

          <button type="submit" className="login-btn">تسجيل الدخول</button>
          <hr />

          <div className='login-div'>
            <span>ليس لديك حساب؟</span>
            <Link to="/register" className="a-login">انشاء حساب جديد</Link>
          </div>

        </form>
      </div>
    </div>
  );
};

export default Login;
