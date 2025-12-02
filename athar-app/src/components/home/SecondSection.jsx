import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import descriptonImg from "../../assets/images/section2-img2.jpg";
import descriptonImg2 from "../../assets/images/section2-img11.jpg";

export default function SecondSection() {
    const [ref, inView] = useInView({ triggerOnce: true, threshold: 0.2 });

    return (
        <motion.div
            ref={ref}
            className="container second-section"
            initial={{ opacity: 0, y: -50 }}
            animate={inView ? { opacity: 1, y: 0 } : {}}
            transition={{ duration: 1, ease: "easeOut" }}
        >
            <div className="row">
                <div className="second-section-text col-lg-8 col-sm-12">
                    <p className="second-section-p1">ماذا عنا</p>
                    <h1 className="second-section-h1">
                        أثرنا في دعم الجمعيات الخيرية الصغيرة
                    </h1>
                    <p className="second-section-p2">
                        في "أثر"، إحنا مؤمنين إن كل جمعية، مهما كانت صغيرة، تقدر
                        تعمل فرق حقيقي في حياة الناس. عشان كده دورنا إننا نساعد
                        الجمعيات الخيرية الصغيرة إنها توصل صوتها، وتزيد قدرتها
                        على التأثير، وتنتشر في مجتمعات أكتر. بنوفّر ليهم الدعم
                        الرقمي والتقني، وبنساعدهم في بناء الثقة والوصول
                        للمتبرعين بسهولة. هدفنا إن الخير يوصل لكل مكان، وإن كل
                        جمعية يكون ليها أثر حقيقي
                    </p>
                    <button className="second-section-btn1">
                        اعرف اكتر عنا{" "}
                        <i className="fa-solid fa-arrow-left first-section-i"></i>
                    </button>
                </div>
                <div className="col-lg-4 col-sm-12 second-section-images">
                    <img
                        className="second-section-img rounded mx-auto d-block collage-img"
                        src={descriptonImg}
                        alt="description"
                    />
                    <img
                        className="second-section-img2 rounded mx-auto d-block collage-img"
                        src={descriptonImg2}
                        alt="description"
                    />
                </div>
            </div>
        </motion.div>
    );
}
