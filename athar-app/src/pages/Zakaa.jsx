import { motion } from "framer-motion";
import { useState } from "react";
import { Link } from "react-router-dom";

export default function Zakaa() {
    const safeNumber = (v) => (isNaN(Number(v)) ? 0 : Number(v));

    const [money, setMoney] = useState(0);
    const [stocks, setStocks] = useState(0);
    const [bonds, setBonds] = useState(0);
    const [profits, setProfits] = useState(0);
    const [gold18, setGold18] = useState(0);
    const [gold21, setGold21] = useState(0);
    const [rent, setRent] = useState(0);

    // أسعار الذهب (ثابت مؤقتًا)
    const gold18Price = 3968.5;
    const gold21Price = 4630;

    // الحسابات
    const zakatMoney = safeNumber(money) * 0.025;

    const zakatTrade =
        (safeNumber(stocks) + safeNumber(bonds) + safeNumber(profits)) * 0.025;

    const zakatGold =
        (safeNumber(gold18) * gold18Price + safeNumber(gold21) * gold21Price) *
        0.025;

    const zakatRent = safeNumber(rent) * 0.025;

    const total = zakatMoney + zakatTrade + zakatGold + zakatRent;

    return (
        <motion.div
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            <div className="zakaa-wrapper">
                {/* فورم */}
                <div className="zakaa-forms">
                    {/* زكاة المال */}
                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة المال</h3>

                        <div className="form-row">
                            <div className="zakaa-input-group">
                                <input
                                    type="number"
                                    value={money}
                                    onChange={(e) =>
                                        setMoney(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>
                    </div>

                    {/* زكاة التجارة */}
                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">
                            زكاة عروض التجارة
                        </h3>

                        <div className="form-row">
                            <div className="zakaa-input-group">
                                <input
                                    type="number"
                                    value={stocks}
                                    onChange={(e) =>
                                        setStocks(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="zakaa-input-group">
                                <input
                                    type="number"
                                    value={bonds}
                                    onChange={(e) =>
                                        setBonds(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="zakaa-input-group">
                                <input
                                    type="number"
                                    value={profits}
                                    onChange={(e) =>
                                        setProfits(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>
                    </div>

                    {/* زكاة الذهب */}
                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة الذهب</h3>

                        <div className="form-row">
                            <div className="zakaa-input-group gold">
                                <label>
                                    وزن الذهب عيار 18؟
                                    <span>
                                        *سعر الجرام اليوم {gold18Price} جنيه*
                                    </span>
                                </label>
                                <input
                                    type="number"
                                    value={gold18}
                                    onChange={(e) =>
                                        setGold18(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>

                            <div className="currency">
                                <div className="currency-select">جرام</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="zakaa-input-group gold">
                                <label>
                                    وزن الذهب عيار 21؟
                                    <span>
                                        *سعر الجرام اليوم {gold21Price} جنيه*
                                    </span>
                                </label>
                                <input
                                    type="number"
                                    value={gold21}
                                    onChange={(e) =>
                                        setGold21(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>

                            <div className="currency">
                                <div className="currency-select">جرام</div>
                            </div>
                        </div>
                    </div>

                    {/* زكاة العقارات */}
                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة العقارات</h3>

                        <div className="form-row">
                            <div className="zakaa-input-group">
                                <label>قيمة الإيجار الشهري؟</label>
                                <input
                                    type="number"
                                    value={rent}
                                    onChange={(e) =>
                                        setRent(safeNumber(e.target.value))
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>

                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* النتائج */}
                <div className="zakaa-result">
                    <h3 className="result-title">قيمة الزكاة</h3>

                    <div className="result-item">
                        <span>زكاة المال:</span>
                        <p>{zakatMoney.toFixed(2)} جنيه</p>
                    </div>

                    <div className="result-item">
                        <span>زكاة عروض التجارة:</span>
                        <p>{zakatTrade.toFixed(2)} جنيه</p>
                    </div>

                    <div className="result-item">
                        <span>زكاة الذهب:</span>
                        <p>{zakatGold.toFixed(2)} جنيه</p>
                    </div>

                    <div className="result-item">
                        <span>زكاة العقارات:</span>
                        <p>{zakatRent.toFixed(2)} جنيه</p>
                    </div>

                    <div className="total-result">
                        <span>الإجمالي المستحق:</span>
                        <p className="total-amount">{total.toFixed(2)} جنيه</p>
                    </div>
                    <Link to={`/campaigns`}>
                        <button className="donate-btn">تبرع الآن</button>
                    </Link>
                </div>
            </div>
        </motion.div>
    );
}
