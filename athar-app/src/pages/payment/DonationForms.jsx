import { useState } from "react";
import { Heart, Loader2 } from "lucide-react";
import { CreatePaymentService } from "../../services/paymentService";
import { useNavigate } from "react-router-dom";

const donationAmounts = [50, 100, 250, 500, 1000, 1200, 1500];

export default function DonationForms() {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        amount: "",
    });
    const [isLoading, setIsLoading] = useState(false);
    const [selectedPreset, setSelectedPreset] = useState(null);

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
        if (name === "amount") setSelectedPreset(null);
    };

    const handlePresetAmount = (amount) => {
        setSelectedPreset(amount);
        setFormData((prev) => ({ ...prev, amount: amount.toString() }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validation بسيطة
        if (
            !formData.firstName ||
            !formData.lastName ||
            !formData.email ||
            !formData.amount
        ) {
            alert("يرجى ملء جميع الحقول المطلوبة");
            return;
        }

        if (isNaN(formData.amount) || Number(formData.amount) <= 0) {
            alert("يرجى إدخال مبلغ صالح");
            return;
        }

        setIsLoading(true);

        try {
            const response = await CreatePaymentService(formData);

            // إذا استلمت رابط الدفع من الخدمة، حول المستخدم إليه
            if (response.paymentUrl) {
                window.location.href = response.paymentUrl;
            } else {
                // إذا تم الدفع مباشرة بدون رابط، نفترض النجاح
                navigate("/success");
            }
        } catch (error) {
            console.error(error);
            navigate("/failed");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="payment-donation-card">
            <div className="payment-donation-card-header">
                <h2>ساهم في صنع الأثر</h2>
                <p>تبرعك اليوم قد يغير حياة شخص غدًا</p>
            </div>
            <form onSubmit={handleSubmit} className="payment-donation-form">
                <div className="payment-name-field">
                    <div className="payment-input-group name">
                        <label>الاسم الأول</label>
                        <input
                            name="firstName"
                            value={formData.firstName}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="payment-input-group name">
                        <label>اسم العائلة</label>
                        <input
                            name="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                            required
                        />
                    </div>
                </div>

                <div className="payment-input-group">
                    <label>البريد الإلكتروني</label>
                    <input
                        name="email"
                        type="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                        dir="ltr"
                    />
                </div>

                <div className="payment-input-group">
                    <label>مبلغ التبرع (جنيه)</label>
                    <div className="preset-buttons">
                        {donationAmounts.map((amount, index) => (
                            <button
                                key={amount}
                                type="button"
                                className={`preset-btn ${
                                    selectedPreset === amount ? "active" : ""
                                }`}
                                onClick={() => handlePresetAmount(amount)}
                            >
                                {index === donationAmounts.length - 1
                                    ? "أدخل مبلغًا آخر"
                                    : `${amount} جنيه`}
                            </button>
                        ))}
                    </div>
                    <input
                        name="amount"
                        type="number"
                        value={formData.amount}
                        onChange={handleChange}
                        min="1"
                        required
                        dir="ltr"
                    />
                </div>

                <button
                    type="submit"
                    className="donate-btn"
                    disabled={isLoading}
                >
                    {isLoading ? (
                        <>
                            <Loader2 className="loader" /> جاري المعالجة...
                        </>
                    ) : (
                        <>
                            <Heart
                                className="icon-heart-small"
                                fill="currentColor"
                            />
                            تبرع الآن
                        </>
                    )}
                </button>
            </form>
        </div>
    );
}
