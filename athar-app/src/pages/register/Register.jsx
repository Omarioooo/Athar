import { useNavigate } from "react-router-dom";
export default function Register() {
    const navigate = useNavigate();
    return (
        <>
            <div className=" register-container">
                <h1 className="login-h1">منصة اثر</h1>
                <p className="login-p">اختر نوع الحساب المناسب لك</p>
                <div className="container">
                    <div className="row register-row">
                        <div
                            className=" col-lg-4 col-sm-12 register-card"
                            onClick={() => navigate("charityregister")}
                            style={{ cursor: "pointer" }}
                        >
                            <div className="register-idiv">
                                <i class="fa-regular fa-building register-i"></i>
                            </div>
                            <div className="register-concept">
                                <h2>حساب جمعية خيرية</h2>
                                <p>
                                    للمؤسسات والجمعيات الخيريةالتى ترغب فى
                                    استقبال التبرعات وادارة المشاريع الخيرية
                                </p>
                            </div>
                            <ul>
                                <li>انشاء وادارة الحملات والمشاريع الخيرية</li>
                                <li>استقبال التبرعات بشفافية كاملة</li>
                                <li>نشر التقارير والتحديثات للمتبرعين</li>
                            </ul>
                        </div>
                        <div
                            className=" col-lg-4 col-sm-12 register-card2"
                            onClick={() => navigate("donorregister")}
                            style={{ cursor: "pointer" }}
                        >
                            <div className="register-idiv2">
                                <i class="fa-solid fa-circle-user register-i"></i>
                            </div>
                            <div className="register-concept">
                                <h2>حساب متبرع</h2>

                                <p>
                                    للافراد الذين يرغبون فى دعم الجمعيات
                                    الخيريةوالمساهمة فى المشاريع الخيرية
                                </p>
                            </div>
                            <ul>
                                <li>تصفح المشاريع والحملات الخيرية</li>
                                <li>التبرع بسهولة وامان</li>
                                <li>متابعة اثر تبرعاتك</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}
