import { Lock, ArrowLeft } from "lucide-react";
import { Link } from "react-router-dom";

export default function NotAuthorized() {
    return (
        <div className="notauth-wrapper">
            <div className="notauth-container">
                <div className="notauth-icon">
                    <Lock size={70} />
                </div>

                <h1 className="notauth-title">الوصول مقيّد</h1>

                <p className="notauth-message">
                    عذراً، لا تملك الصلاحية للوصول إلى لوحة التحكم هذه
                </p>

                <div className="notauth-actions">
                    <Link to="/" className="back-home-btn">
                        <ArrowLeft size={20} />
                        العودة للصفحة الرئيسية
                    </Link>
                </div>
            </div>
        </div>
    );
}
