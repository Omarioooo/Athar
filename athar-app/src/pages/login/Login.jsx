import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { UseAuth } from "../../Auth/Auth";
import { loginUser } from "../../services/accountService";

export default function Login() {
    const auth = UseAuth();
    const navigate = useNavigate();
    const [user, setUser] = useState({ email: "", password: "" });
    const [error, setError] = useState({});

    const handleChange = (ev) => {
        const { name, value } = ev.target;
        setUser((prev) => ({ ...prev, [name]: value }));
    };

    const handleLogin = async (ev) => {
        ev.preventDefault();
        setError({});
        console.log("user is :-", user);

        const result = await loginUser(user);

        if (result.success) {
            auth.login(result.data);
            navigate("/", { replace: true });
        } else {
            setError(result.errors);
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
                        />
                        <small className="text-danger">
                            {error.passworderror}
                        </small>
                    </div>

                    <button type="submit" className="login-btn">
                        تسجيل الدخول
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
