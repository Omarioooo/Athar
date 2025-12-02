import { motion } from "framer-motion";
import { useState } from "react";

export default function Zakaa() {
    const [money, setMoney] = useState(0);
    const [stocks, setStocks] = useState(0);
    const [bonds, setBonds] = useState(0);
    const [profits, setProfits] = useState(0);
    const [gold18, setGold18] = useState(0);
    const [gold21, setGold21] = useState(0);
    const [rent, setRent] = useState(0);

    // TEMP LOGIC — Just sum everything for now
    const gold18Price = 3968.5;
    const gold21Price = 4630;

    const zakatMoney = Number(money) * 0.025;
    const zakatTrade =
        (Number(stocks) + Number(bonds) + Number(profits)) * 0.025;
    const zakatGold =
        (Number(gold18) * gold18Price + Number(gold21) * gold21Price) * 0.025;
    const zakatRent = Number(rent) * 0.025;

    const total = zakatMoney + zakatTrade + zakatGold + zakatRent;

    return (
        <motion.div
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            <div className="zakaa-wrapper">
                <div className="zakaa-forms">
                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة المال</h3>
                        <div className="form-row">
                            <div className="input-group">
                                {/* <label>كم تملك من المال؟</label> */}
                                <input
                                    type="number"
                                    value={money}
                                    onChange={(e) =>
                                        setMoney(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>
                    </div>

                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">
                            زكاة عروض التجارة
                        </h3>
                        <div className="form-row">
                            <div className="input-group">
                                {/* <label>ما هي قيمة أسهمك في السوق؟</label> */}
                                <input
                                    type="number"
                                    value={stocks}
                                    onChange={(e) =>
                                        setStocks(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="input-group">
                                {/* <label>ما هي قيمة السندات التي تملكها؟</label> */}
                                <input
                                    type="number"
                                    value={bonds}
                                    onChange={(e) =>
                                        setBonds(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="input-group">
                                {/* <label>ما هي قيمة الأرباح المحققة؟</label> */}
                                <input
                                    type="number"
                                    value={profits}
                                    onChange={(e) =>
                                        setProfits(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جنيه مصري</div>
                            </div>
                        </div>
                    </div>

                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة الذهب</h3>

                        <div className="form-row">
                            <div className="input-group gold">
                                <label>
                                    وزن الذهب عيار 18؟
                                    <span>*سعر الجرام اليوم 3,968.5 جنيه*</span>
                                </label>
                                <input
                                    type="number"
                                    value={gold18}
                                    onChange={(e) =>
                                        setGold18(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جرام</div>
                            </div>
                        </div>

                        <div className="form-row">
                            <div className="input-group gold">
                                <label>
                                    وزن الذهب عيار 21؟
                                    <span>*سعر الجرام اليوم 4630 جنيه*</span>
                                </label>
                                <input
                                    type="number"
                                    value={gold21}
                                    onChange={(e) =>
                                        setGold21(e.target.valueAsNumber)
                                    }
                                    placeholder="القيمة هنا"
                                />
                            </div>
                            <div className="currency">
                                <div className="currency-select">جرام</div>
                            </div>
                        </div>
                    </div>

                    <div className="zakaa-section">
                        <h3 className="zakaa-section-title">زكاة العقارات</h3>
                        <div className="form-row">
                            <div className="input-group">
                                <label>قيمة الإيجار الشهري؟</label>
                                <input
                                    type="number"
                                    value={rent}
                                    onChange={(e) =>
                                        setRent(e.target.valueAsNumber)
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

                    <button className="donate-btn">تبرع الآن</button>
                </div>
            </div>
        </motion.div>
    );
}
