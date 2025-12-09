import DonationForm from "./DonationForms";
import { Sparkles } from "lucide-react";

export default function Checkout() {
    return (
        <div className="payment-checkout-wrapper">
            <div className="payment-content-container">
                <div className="payment-top-quote payment-fade-in payment-delay-1">
                    <div className="payment-quote-box">
                        <Sparkles className="payment-spark" />
                        <p>بصنائعكم نصنع أثرًا يبقى</p>
                        <Sparkles className="payment-spark" />
                    </div>
                </div>
                <div className="payment-main-section">
                    <div className="payment-grid">
                        <div className="payment-left-text payment-fade-in payment-delay-2">
                            <h1>
                                كُن جزءًا من
                                <br />
                                <span className="payment-highlight">
                                    التغيير الإيجابي
                                </span>
                            </h1>
                            <p>
                                مع كل تبرع، نزرع بذرة أمل في قلوب المحتاجين.
                                ساهم معنا في بناء مستقبل أفضل للجميع.
                            </p>
                        </div>

                        <div className="payment-form-wrapper payment-fade-in payment-delay-3">
                            <DonationForm />
                        </div>
                    </div>
                </div>

                <div className="payment-footer-quote payment-fade-in payment-delay-4">
                    <p>
                        "إِنْ تُبْدُوا الصَّدَقَاتِ فَنِعِمَّا هِيَ ۖ وَإِنْ
                        تُخْفُوهَا وَتُؤْتُوهَا الْفُقَرَاءَ فَهُوَ خَيْرٌ
                        لَكُمْ وَيُكَفِّرُ عَنْكُمْ مِنْ سَيِّئَاتِكُمْ
                        وَاللَّهُ بِمَا تَعْمَلُونَ خَبِيرٌ"
                    </p>
                </div>
            </div>
        </div>
    );
}
