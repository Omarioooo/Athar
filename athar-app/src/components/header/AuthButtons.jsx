import { Link } from "react-router-dom";

export default function AuthButtons() {
    return (
        <div className="auth-btns">
            <Link to="/login" className="btn-login">
                تسجيل الدخول
            </Link>
            <Link to="/signup" className="btn-signup">
                إنشاء حساب
            </Link>
        </div>
    );
}
