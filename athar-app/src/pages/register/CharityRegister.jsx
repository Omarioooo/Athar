import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { registerCharity } from "../../services/charityService";

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
        documentation: null,
        img: null,
    });
    const [error, setError] = useState({});

    const handleChange = (ev) => {
        const { name, value, files } = ev.target;
        if (files) setUser((prev) => ({ ...prev, [name]: files[0] }));
        else setUser((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (ev) => {
        ev.preventDefault();
        setError({});
        const result = await registerCharity(user);
        if (result.success) navigate("/login");
        else setError(result.errors);
    };

    return (
        <div className="charityregister-section">
            <div className="register-idiv">
                <i className="fa-regular fa-building register-i"></i>
            </div>
            <div className="charityregister-media">
                <h3>تسجيل حساب جمعية خيرية</h3>
                <p>ادخل معلومات الجمعية لانشاء حساب جديد</p>
            </div>
            <div className="charityregister-smallsection">
                {error.serverError && (
                    <div className="text-danger mb-3">{error.serverError}</div>
                )}
                <form onSubmit={handleSubmit}>
                    <div className="mt-4">
                        <label htmlFor="email" className="form-label">
                            البريد الالكترونى*
                        </label>
                        <input
                            type="email"
                            style={{ direction: "rtl" }}
                            className={`form-control login-input ${
                                error.emailerror ? "border-danger" : ""
                            }`}
                            name="email"
                            value={user.email}
                            onChange={handleChange}
                            placeholder="ادخل الايميل"
                        />
                        <small className="text-danger">
                            {error.emailerror}
                        </small>
                    </div>

                    <div className="mt-4">
                        <label htmlFor="charityname" className="form-label">
                            اسم الجمعية*
                        </label>
                        <input
                            type="text"
                            style={{ direction: "rtl" }}
                            className={`form-control login-input ${
                                error.charitynameerror ? "border-danger" : ""
                            }`}
                            name="charityname"
                            value={user.charityname}
                            onChange={handleChange}
                            placeholder="ادخل اسم الجمعية"
                        />
                        <small className="text-danger">
                            {error.charitynameerror}
                        </small>
                    </div>

                    <div className="row">
                        <div className="col-md-6 mt-4">
                            <label htmlFor="password" className="form-label">
                                كلمة المرور*
                            </label>
                            <input
                                type="password"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${
                                    error.passworderror ? "border-danger" : ""
                                }`}
                                name="password"
                                value={user.password}
                                onChange={handleChange}
                                placeholder="......."
                            />
                            <small className="text-danger">
                                {error.passworderror}
                            </small>
                        </div>

                        <div className="col-md-6 mt-4">
                            <label
                                htmlFor="confirmpassword"
                                className="form-label"
                            >
                                تاكيد كلمة المرور*
                            </label>
                            <input
                                type="password"
                                style={{ direction: "rtl" }}
                                className={`form-control login-input ${
                                    error.confirmpassworderror
                                        ? "border-danger"
                                        : ""
                                }`}
                                name="confirmpassword"
                                value={user.confirmpassword}
                                onChange={handleChange}
                                placeholder="........"
                            />
                            <small className="text-danger">
                                {error.confirmpassworderror}
                            </small>
                        </div>
                    </div>

                    <div className="mt-4">
                        <label
                            htmlFor="charitydescription"
                            className="form-label"
                        >
                            وصف الجمعية*
                        </label>
                        <textarea
                            style={{ direction: "rtl" }}
                            className={`form-control login-input ${
                                error.charitydescriptionerror
                                    ? "border-danger"
                                    : ""
                            }`}
                            name="charitydescription"
                            value={user.charitydescription}
                            onChange={handleChange}
                            placeholder="ادخل وصف للجمعية"
                            rows={4}
                        />
                        <small className="text-danger">
                            {error.charitydescriptionerror}
                        </small>
                    </div>

                    <div className="row">
                        <div className="col-md-6 mt-4">
                            <label htmlFor="country" className="form-label">
                                الدولة*
                            </label>
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
                            <label htmlFor="city" className="form-label">
                                المدينة*
                            </label>
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

                    <div className="mt-4">
                        <label htmlFor="documentation" className="form-label">
                            مستند لتحقق من صحة الجمعية*
                        </label>
                        <input
                            type="file"
                            style={{ direction: "rtl" }}
                            className="form-control login-input"
                            name="documentation"
                            onChange={handleChange}
                        />
                        {user.documentation && (
                            <small>
                                الملف المختار: {user.documentation.name}
                            </small>
                        )}
                    </div>

                    <div className="mt-4">
                        <label htmlFor="img" className="form-label">
                            صورة شعار للجمعية*
                        </label>
                        <input
                            type="file"
                            style={{ direction: "rtl" }}
                            className="form-control login-input"
                            name="img"
                            onChange={handleChange}
                        />
                        {user.img && (
                            <small>الملف المختار: {user.img.name}</small>
                        )}
                    </div>

                    <button type="submit" className="login-btn mt-4">
                        انشاء حساب
                    </button>
                </form>
            </div>
        </div>
    );
}
