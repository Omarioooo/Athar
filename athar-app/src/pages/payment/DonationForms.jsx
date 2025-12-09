import { useState } from "react";
import { Heart } from "lucide-react";
import { useNavigate } from "react-router-dom";

const donationAmounts = [50, 100, 250, 500, 1000, 1200, 1500];

export default function DonationForms() {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        amount: "",
    });
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

    const handleSubmit = (e) => {
        e.preventDefault();

        const { firstName, lastName, email, amount } = formData;

        // Client-side validation للأسماء و الإيميل
        if (!firstName.trim() || !lastName.trim() || !email.trim()) {
            alert("يرجى ملء جميع الحقول المطلوبة");
            return;
        }

        // Validation البريد الإلكتروني بشكل بسيط
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            alert("يرجى إدخال بريد إلكتروني صالح");
            return;
        }

        // Validation المبلغ
        const numericAmount = Number(amount);
        if (isNaN(numericAmount)) {
            alert("يرجى إدخال مبلغ صالح");
            return;
        }

        if (numericAmount > 0) {
            navigate("/success");
        } else {
            navigate("/failed");
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

                <button type="submit" className="donate-btn">
                    <Heart className="icon-heart-small" fill="currentColor" /> تبرع الآن
                </button>
            </form>
        </div>
    );
}
