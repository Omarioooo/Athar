import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { registerDonor } from "../../services/donorService";

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
        img: null,
    });
    const [error, setError] = useState({});

    const handleChange = (ev) => {
        const { name, value, files } = ev.target;
        setUser((prev) => ({ ...prev, [name]: files ? files[0] : value }));
    };

    const handleSubmit = async (ev) => {
        ev.preventDefault();
        setError({});

        const result = await registerDonor(user);
        if (result.success) navigate("/login");
        else setError(result.errors);
    };

    return (
        <div className="charityregister-section">
            <div className="register-idiv2">
                <i className="fa-solid fa-circle-user register-i"></i>
            </div>

            <div className="charityregister-media">
                <h3>تسجيل حساب متبرع</h3>
                <p>ادخل معلوماتك لانشاء حساب جديد</p>
            </div>

            <div className="charityregister-smallsection">
                {error.serverError && (
                    <div className="text-danger mb-3">{error.serverError}</div>
                )}

                <form onSubmit={handleSubmit}>
                    {/* Email */}
                    <div className="mt-4">
                        <label className="form-label">البريد الالكترونى*</label>
                        <input
                            type="text"
                            name="email"
                            value={user.email}
                            onChange={handleChange}
                            className={`form-control login-input ${
                                error.emailerror ? "border-danger" : ""
                            }`}
                            placeholder="ادخل الايميل"
                            style={{ direction: "rtl" }}
                        />
                        <small className="text-danger">
                            {error.emailerror}
                        </small>
                    </div>

                    {/* First & Last Name */}
                    <div className="row">
                        <div className="col-md-6 mt-4">
                            <label className="form-label">الاسم الاول*</label>
                            <input
                                type="text"
                                name="firstname"
                                value={user.firstname}
                                onChange={handleChange}
                                className={`form-control login-input ${
                                    error.firstnamerror ? "border-danger" : ""
                                }`}
                                placeholder="اكتب اسمك"
                                style={{ direction: "rtl" }}
                            />
                            <small className="text-danger">
                                {error.firstnamerror}
                            </small>
                        </div>

                        <div className="col-md-6 mt-4">
                            <label className="form-label">الاسم الاخير*</label>
                            <input
                                type="text"
                                name="lastname"
                                value={user.lastname}
                                onChange={handleChange}
                                className={`form-control login-input ${
                                    error.lastnameerror ? "border-danger" : ""
                                }`}
                                placeholder="اكتب اسمك الاخير"
                                style={{ direction: "rtl" }}
                            />
                            <small className="text-danger">
                                {error.lastnameerror}
                            </small>
                        </div>
                    </div>

                    {/* Password */}
                    <div className="row">
                        <div className="col-md-6 mt-4">
                            <label className="form-label">كلمة المرور*</label>
                            <input
                                type="password"
                                name="password"
                                value={user.password}
                                onChange={handleChange}
                                className={`form-control login-input ${
                                    error.passworderror ? "border-danger" : ""
                                }`}
                                placeholder="......."
                                style={{ direction: "rtl" }}
                            />
                            <small className="text-danger">
                                {error.passworderror}
                            </small>
                        </div>

                        <div className="col-md-6 mt-4">
                            <label className="form-label">
                                تاكيد كلمة المرور*
                            </label>
                            <input
                                type="password"
                                name="confirmpassword"
                                value={user.confirmpassword}
                                onChange={handleChange}
                                className={`form-control login-input ${
                                    error.confirmpassworderror
                                        ? "border-danger"
                                        : ""
                                }`}
                                placeholder="........"
                                style={{ direction: "rtl" }}
                            />
                            <small className="text-danger">
                                {error.confirmpassworderror}
                            </small>
                        </div>
                    </div>

                    {/* Country / City */}
                    <div className="row">
                        <div className="col-md-6 mt-4">
                            <label className="form-label">الدولة*</label>
                            <input
                                type="text"
                                name="country"
                                value={user.country}
                                onChange={handleChange}
                                className="form-control login-input"
                                placeholder="مصر"
                                style={{ direction: "rtl" }}
                            />
                        </div>

                        <div className="col-md-6 mt-4">
                            <label className="form-label">المدينة*</label>
                            <input
                                type="text"
                                name="city"
                                value={user.city}
                                onChange={handleChange}
                                className="form-control login-input"
                                placeholder="القاهرة"
                                style={{ direction: "rtl" }}
                            />
                        </div>
                    </div>

                    {/* Image */}
                    <div className="mt-4">
                        <label className="form-label">
                            صورة شخصية (اختياري)
                        </label>
                        <input
                            type="file"
                            name="img"
                            onChange={handleChange}
                            className="form-control login-input"
                            style={{ direction: "rtl" }}
                        />
                        {user.img && (
                            <small>الملف المختار: {user.img.name}</small>
                        )}
                    </div>

                    <button type="submit" className="login-btn mt-3">
                        انشاء حساب
                    </button>
                </form>
            </div>
        </div>
    );
}
