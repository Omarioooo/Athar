import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";

export default function Home() {

  const [refFirst, inViewFirst] = useInView({ triggerOnce: true, threshold: 0.2 });
  const [refSecond, inViewSecond] = useInView({ triggerOnce: true, threshold: 0.2 });
  const [refthird, inViewthird] = useInView({ triggerOnce: true, threshold: 0.2 });

  return (
    <>
      {/* First Section */}
      <motion.div
        ref={refFirst}
        className="first-section"
        initial={{ opacity: 0, y: -50 }}
        animate={inViewFirst ? { opacity: 1, y: 0 } : {}}
        transition={{ duration: 1, ease: "easeOut" }}
      >
        <div className="first-section-text">
          <span className="first-section-h1">صنع </span>
          <span className="first-section-h2">اثر حقيقى</span> 
          <h1 className="first-section-h1">فى حياة المحتاجين</h1>
          <p className="first-section-p">
            منصة رقمية شفافة تربط بين الجمعيات الخيرية الموثوقة والمتبرعين، لتحقيق أقصى استفادة من كل جنيه يتم التبرع به
          </p>
          <button className="first-section-btn1">
            <i className="fa-solid fa-heart first-section-i"></i>ابدا بالتبرع الان
          </button>
          <button className="first-section-btn2">
            تصفح الجمعيات<i className="fa-solid fa-arrow-left first-section-i"></i>
          </button>
        </div>
      </motion.div>

      {/* Second Section */}
      <motion.div
        ref={refSecond}
        className="container second-section"
        initial={{ opacity: 0, y: -50 }}
        animate={inViewSecond ? { opacity: 1, y: 0 } : {}}
        transition={{ duration: 1, ease: "easeOut" }}
      >
        <div className="row">
          <div className="second-section-text col-lg-8 col-sm-12">
            <p className="second-section-p1">ماذا عنا</p>
            <h1 className="second-section-h1">أثرنا في دعم الجمعيات الخيرية الصغيرة</h1>
            <p className="second-section-p2">
              في "أثر"، إحنا مؤمنين إن كل جمعية، مهما كانت صغيرة، تقدر تعمل فرق حقيقي في حياة الناس.
              عشان كده دورنا إننا نساعد الجمعيات الخيرية الصغيرة إنها توصل صوتها، وتزيد قدرتها على التأثير، وتنتشر في مجتمعات أكتر.
              بنوفّر ليهم الدعم الرقمي والتقني، وبنساعدهم في بناء الثقة والوصول للمتبرعين بسهولة.
              هدفنا إن الخير يوصل لكل مكان، وإن كل جمعية يكون ليها أثر حقيقي
            </p>
            <button className="second-section-btn1">
              اعرف اكتر عنا<i className="fa-solid fa-arrow-left first-section-i"></i>
            </button>
          </div>
          <div className="col-lg-4 col-sm-12 second-section-images">
            <img className="second-section-img rounded mx-auto d-block collage-img" src="./images/section2-img2.jpg" alt="description" />
            <img className="second-section-img2 rounded mx-auto d-block collage-img" src="./images/section2-img11.jpg" alt="description" />
          </div>
        </div>
      </motion.div>
      {/* third-section */}
        <motion.div
        ref={refthird}
        className="third-section"
        initial={{ opacity: 0, y: -50 }}
        animate={inViewthird ? { opacity: 1, y: 0 } : {}}
        transition={{ duration: 1, ease: "easeOut" }}
      >
      <div class="container bg-3 text-center third-section">    
  <h1>لماذا تختار منصة اثر</h1><br/>
  <div class="row">
    <div class="col-sm-10 col-lg-3 third-section-div">
        <h3>الشفافية الكاملة</h3>
      <p>منصة أثر تضمن لكل متبرع متابعة كل جنيه يتم التبرع به، مع تقارير دقيقة وشفافة عن استخدام الأموال، لضمان وصول الدعم لمن يحتاجه فعلاً."</p>
      <img src="./images/sec3-img1.jpg" alt="Image" width="300" height="320"/>
    </div>
    <div class="col-sm-10 col-lg-3 third-section-div"> 
         <h3>جمعيات موثوقة</h3>
      <p>نحن نعمل فقط مع الجمعيات الخيرية الموثوقة والمعتمدة، لضمان أن كل تبرع يذهب لمن يستحق، وأن كل مشروع يتم تنفيذه بكفاءة واحترافية</p>
      <img src="./images/sec3-img2.jpg" alt="Image" width="300" height="300"/>
    </div>
    <div class="col-sm-10 col-lg-3 third-section-div"> 
         <h3>تاثير حقيقى</h3>
      <p>هدفنا أن نخلق فرق ملموس في حياة الناس. كل حملة وكل تبرع يسهم في تحسين الظروف المعيشية ودعم المبادرات التي تحدث فرقاً حقيقياً</p>
      <img src="./images/sec3-img3.jpg" alt="Image" width="300" height="300"/>
    </div>
  </div>
</div>
 </motion.div>
{/* footer */}
<footer className="text-center text-lg-start bg-light text-muted">

  <section className="d-flex justify-content-center justify-content-lg-between p-4 border-bottom">

    <div className="me-5 d-none d-lg-block">
      <span>تواصل معنا عبر منصات التواصل الاجتماعى:</span>
    </div>

    <div>
      <a href="#" className="me-4 text-reset"><i className="fab fa-facebook-f footericon"></i></a>
      <a href="#" className="me-4 text-reset"><i className="fab fa-twitter footericon"></i></a>
      <a href="#" className="me-4 text-reset"><i className="fab fa-instagram footericon"></i></a>
      <a href="#" className="me-4 text-reset"><i className="fab fa-linkedin footericon"></i></a>
      <a href="#" className="me-4 text-reset"><i className="fab fa-github footericon"></i></a>
    </div>

  </section>

  <section>
    <div className="container text-center text-md-start mt-5">

      <div className="row mt-3">

        {/* وصف الموقع */}
        <div className="col-md-3 col-lg-4 col-xl-3 mx-auto mb-4">
          <h6 className="text-uppercase fw-bold mb-4">
            <i className="fas fa-gem me-3"></i>موقع أثر
          </h6>
          <p>
            منصة تفاعلية تهدف لدعم الخير والعمل التطوعي من خلال حملات موثوقة
            وتسهيل توصيل المساعدات لمستحقيها بطرق آمنة وفعّالة.
          </p>
        </div>

        {/* روابط مهمة */}
        <div className="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
          <h6 className="text-uppercase fw-bold mb-4">روابط سريعة</h6>
          <p><a href="#" className="text-reset">عن أثر</a></p>
          <p><a href="#" className="text-reset">آخر الحملات</a></p>
          <p><a href="#" className="text-reset">كيفية التبرع</a></p>
          <p><a href="#" className="text-reset">فرص التطوع</a></p>
        </div>

        {/* روابط مساعدة */}
        <div className="col-md-3 col-lg-2 col-xl-2 mx-auto mb-4">
          <h6 className="text-uppercase fw-bold mb-4">مساعدة</h6>
          <p><a href="#" className="text-reset">سياسة الخصوصية</a></p>
          <p><a href="#" className="text-reset">الشروط والأحكام</a></p>
          <p><a href="#" className="text-reset">الأسئلة الشائعة</a></p>
          <p><a href="#" className="text-reset">تواصل معنا</a></p>
        </div>

        {/* تواصل معنا */}
        <div className="col-md-4 col-lg-3 col-xl-3 mx-auto mb-md-0 mb-4">
          <h6 className="text-uppercase fw-bold mb-4">تواصل معنا</h6>
          <p><i className="fas fa-home me-3"></i> القاهرة، مصر</p>
          <p><i className="fas fa-envelope me-3"></i> info@athar.com</p>
          <p><i className="fas fa-phone me-3"></i> 0100 000 0000</p>
        </div>

      </div>
    </div>
  </section>

  <div
    className="text-center p-4"
   style={{ backgroundColor: "rgba(78, 182, 230, 0.927)" }}
  >
    © {new Date().getFullYear()} حقوق النشر محفوظة:
    <span className="text-reset fw-bold"> أثر</span>
  </div>

</footer>

    </>
  );
}