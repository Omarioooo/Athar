import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { CheckCircle2, Heart, Home, Sparkles } from "lucide-react";

export default function PaymentSuccess() {
    const [showContent, setShowContent] = useState(false);

    useEffect(() => {
        const timer = setTimeout(() => setShowContent(true), 300);
        return () => clearTimeout(timer);
    }, []);

    const handleShare = () => {
        if (navigator.share) {
            navigator.share({
                title: "أثر - منصة التبرع الخيري",
                text: "ساهمت في صنع أثر إيجابي! انضم إلينا وكن جزءًا من التغيير.",
                url: window.location.origin,
            });
        } else {
            navigator.clipboard.writeText(window.location.origin);
            alert("تم النسخ! تم نسخ الرابط إلى الحافظة");
        }
    };

    return (
        <div className="payment-success-page">
            {/* Decorative Elements */}
            <div className="payment-decoratives">
                <span className="payment-dot payment-dot1" />
                <span className="payment-dot payment-dot2" />
                <span className="payment-dot payment-dot3" />
                <span className="payment-dot payment-dot4" />
                <span className="payment-blur-circle payment-blur1" />
                <span className="payment-blur-circle payment-blur2" />
            </div>

            <div className="payment-page-content">
                <div className="payment-main">
                    <div
                        className={`payment-card ${
                            showContent ? "payment-show" : ""
                        }`}
                    >
                        <div className="payment-card-content">
                            {/* Success Icon */}
                            <div className="payment-icon-wrapper">
                                <CheckCircle2
                                    className="payment-icon-check"
                                    strokeWidth={1.5}
                                />
                            </div>

                            {/* Success Message */}
                            <h1 className="payment-success-title">شكرًا لك!</h1>
                            <p className="payment-success-subtitle">
                                لقد تركت أثرًا جميلًا
                            </p>
                            <p className="payment-success-description">
                                تمت عملية التبرع بنجاح. جزاك الله خيرًا على
                                كرمك.
                            </p>

                            {/* Inspirational Quote */}
                            <div className="payment-quote">
                                <div className="payment-quote-title">
                                    <Sparkles className="payment-spark" />
                                    <span>ما نقصت صدقة من مال</span>
                                    <Sparkles className="payment-spark" />
                                </div>
                            </div>

                            {/* Action Buttons */}
                            <div className="payment-actions">
                                <Link
                                    to="/checkout"
                                    className="payment-btn payment-btn-primary"
                                >
                                    <Heart className="payment-icon-btn" />
                                    تبرع مرة أخرى
                                </Link>
                                <Link
                                    to="/"
                                    className="payment-btn payment-btn-outline"
                                >
                                    <Home className="payment-icon-btn" />
                                    الرئيسية
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>

                <footer className="payment-footer">
                    <p>تبرعك يصل إلى المحتاجين بكل شفافية وأمانة</p>
                </footer>
            </div>
        </div>
    );
}
