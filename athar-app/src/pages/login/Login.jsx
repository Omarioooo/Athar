import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { UseAuth } from "../../Auth/Auth";
import { loginUser } from "../../services/accountService";

export default function Login() {
    const { login } = UseAuth();
    const navigate = useNavigate();

    const [user, setUser] = useState({ email: "", password: "" });
    const [error, setError] = useState({});
    const [loading, setLoading] = useState(false);

    const handleChange = (ev) => {
        const { name, value } = ev.target;
        setUser((prev) => ({ ...prev, [name]: value }));

        if (error[name + "error"]) {
            setError((prev) => ({ ...prev, [name + "error"]: null }));
        }
    };

    const handleLogin = async (ev) => {
        ev.preventDefault();
        setError({});
        setLoading(true);
        const result = await loginUser(user);

        try {
            const result = await loginUser(user);

            if (result.success) {
                login(result.data.token, {
                    id: result.data.id,
                    email: result.data.email,
                    userName: result.data.userName,
                    role: result.data.role,
                });

                navigate("/", { replace: true });
            }
        } catch (err) {
            setError(
                err.errors || { general: "فشل تسجيل الدخول، حاول مرة أخرى" }
            );
        } finally {
            setLoading(false);
        }
    };
    return (
        <div className="login-section">
            <h1 className="login-h1">منصة اثر</h1>
            <p className="login-p">منصة العطاء والتبرعات</p>

            <div className="login-smallsection">
                <form onSubmit={handleLogin}>
                    <h4 className="login-h4">تسجيل الدخول</h4>
                    <p className="login-p2">ادخل بياناتك للوصول الى حسابك</p>

                    {/* error message */}
                    {error.general && (
                        <div className="alert alert-danger text-center">
                            {error.general}
                        </div>
                    )}

                    <div className="mb-3">
                        <label htmlFor="email" className="form-label">
                            الايميل
                        </label>
                        <input
                            type="text"
                            style={{ direction: "rtl" }}
                            className={`form-control login-input ${
                                error.emailerror ? "border-danger" : ""
                            }`}
                            name="email"
                            value={user.email}
                            onChange={handleChange}
                            placeholder="ادخل الايميل"
                            required
                            disabled={loading}
                        />
                        <small className="text-danger">
                            {error.emailerror}
                        </small>
                    </div>

                    <div className="mb-3">
                        <label htmlFor="password" className="form-label">
                            كلمة المرور
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
                            placeholder="ادخل كلمة المرور"
                            required
                            disabled={loading}
                        />
                        <small className="text-danger">
                            {error.passworderror}
                        </small>
                    </div>

                    <button
                        type="submit"
                        className="login-btn"
                        disabled={loading}
                    >
                        {loading ? (
                            <>
                                <span className="spinner-border spinner-border-sm me-2"></span>
                                جاري تسجيل الدخول...
                            </>
                        ) : (
                            "تسجيل الدخول"
                        )}
                    </button>
                    <hr />

                    <div className="login-div">
                        <span>ليس لديك حساب؟</span>
                        <Link to="/signup" className="a-login">
                            انشاء حساب جديد
                        </Link>
                    </div>
                </form>
            </div>
        </div>
    );
}
