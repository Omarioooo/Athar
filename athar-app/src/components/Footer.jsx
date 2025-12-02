export default function Footer() {
    return (
        <footer className="footer text-center text-lg-start bg-light text-muted">
            <section className="d-flex justify-content-center justify-content-lg-between p-4 border-bottom">
                <div className="me-5 d-none d-lg-block">
                    <span>تواصل معنا عبر منصات التواصل الاجتماعى:</span>
                </div>
                <div>
                    <a href="#" className="me-4 text-reset">
                        <i className="fab fa-facebook-f footericon"></i>
                    </a>
                    <a href="#" className="me-4 text-reset">
                        <i className="fab fa-twitter footericon"></i>
                    </a>
                    <a href="#" className="me-4 text-reset">
                        <i className="fab fa-instagram footericon"></i>
                    </a>
                    <a href="#" className="me-4 text-reset">
                        <i className="fab fa-linkedin footericon"></i>
                    </a>
                    <a href="#" className="me-4 text-reset">
                        <i className="fab fa-github footericon"></i>
                    </a>
                </div>
            </section>

            <section>
                <div className="container text-center text-md-start mt-5">
                    <div className="row mt-3">
                        <div className="col-md-3 col-lg-4 col-xl-3 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <i className="fas fa-gem me-3"></i>موقع أثر
                            </h6>
                            <p>
                                منصة تفاعلية تهدف لدعم الخير والعمل التطوعي من
                                خلال حملات موثوقة وتسهيل توصيل المساعدات
                                لمستحقيها بطرق آمنة وفعّالة.
                            </p>
                        </div>
                        <div className="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                روابط سريعة
                            </h6>
                            <p>
                                <a href="#" className="text-reset">
                                    عن أثر
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    آخر الحملات
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    كيفية التبرع
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    فرص التطوع
                                </a>
                            </p>
                        </div>
                        <div className="col-md-3 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                مساعدة
                            </h6>
                            <p>
                                <a href="#" className="text-reset">
                                    سياسة الخصوصية
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    الشروط والأحكام
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    الأسئلة الشائعة
                                </a>
                            </p>
                            <p>
                                <a href="#" className="text-reset">
                                    تواصل معنا
                                </a>
                            </p>
                        </div>
                        <div className="col-md-4 col-lg-3 col-xl-3 mx-auto mb-md-0 mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                تواصل معنا
                            </h6>
                            <p>
                                <i className="fas fa-home me-3"></i> القاهرة،
                                مصر
                            </p>
                            <p>
                                <i className="fas fa-envelope me-3"></i>{" "}
                                info@athar.com
                            </p>
                            <p>
                                <i className="fas fa-phone me-3"></i> 0100 000
                                0000
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            <div
                className="text-center p-4"
                style={{ backgroundColor: "rgba(78, 182, 230, 0.927)" }}
            >
                © {new Date().getFullYear()} حقوق النشر محفوظة:{" "}
                <span className="text-reset fw-bold">أثر</span>
            </div>
        </footer>
    );
}
