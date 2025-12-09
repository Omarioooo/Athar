import { Link } from "react-router-dom";
import {
    XCircle,
    RefreshCw,
    Home,
    MessageCircle,
    HeartHandshake,
} from "lucide-react";

export default function PaymentFailed() {
    return (
        <div className="payment-failed-page">
            <div className="payment-decoratives">
                <span className="payment-blur-circle payment-blur1"></span>
                <span className="payment-blur-circle payment-blur2"></span>
            </div>

            <div className="payment-page-content">
                <div className="payment-main">
                    <div className="payment-card payment-animate-show">
                        <div className="payment-card-content">
                            {/* Error Icon */}
                            <div className="payment-icon-wrapper payment-icon-error">
                                <XCircle className="payment-icon-x" strokeWidth={1.5} />
                            </div>

                            {/* Error Message */}
                            <h1 className="payment-error-title">للأسف!</h1>
                            <p className="payment-error-subtitle">لم تكتمل عملية الدفع</p>
                            <p className="payment-error-description">
                                لا تقلق، يمكنك المحاولة مرة أخرى أو التواصل معنا
                                للمساعدة.
                            </p>

                            {/* Encouragement Box */}
                            <div className="payment-encouragement">
                                <HeartHandshake className="payment-icon-heart" />
                                <p className="payment-encouragement-text">
                                    نيتك الطيبة محسوبة لك. جرب مرة أخرى وسنكون
                                    معك.
                                </p>
                            </div>

                            {/* Action Buttons */}
                            <div className="payment-actions">
                                <Link
                                    to="/checkout"
                                    className="payment-btn payment-btn-primary"
                                >
                                    <RefreshCw className="payment-icon-btn" />
                                    حاول مرة أخرى
                                </Link>
                                <div className="payment-flex-buttons">
                                    <Link to="/" className="payment-btn payment-btn-outline">
                                        <Home className="payment-icon-btn" />
                                        الرئيسية
                                    </Link>
                                </div>
                            </div>

                            <p className="payment-help-text">
                                إذا استمرت المشكلة، يرجى التحقق من بيانات بطاقتك
                                أو التواصل مع البنك
                            </p>
                        </div>
                    </div>
                </div>

                <div className="payment-footer">
                    <p>فريق أثر هنا لمساعدتك على مدار الساعة</p>
                </div>
            </div>
        </div>
    );
}
