import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import image1 from "../../assets/images/sec3-img1.jpg";
import image2 from "../../assets/images/sec3-img2.jpg";
import image3 from "../../assets/images/sec3-img3.jpg";

export default function ThirdSection() {
    const [ref, inView] = useInView({ triggerOnce: true, threshold: 0.2 });

    return (
        <motion.div
            ref={ref}
            className="third-section"
            initial={{ opacity: 0, y: -50 }}
            animate={inView ? { opacity: 1, y: 0 } : {}}
            transition={{ duration: 1, ease: "easeOut" }}
        >
            <div className="container bg-3 text-center">
                <h1>لماذا تختار منصة اثر</h1>
                <br />
                <div className="row">
                    <div className="col-sm-10 col-lg-3 third-section-div">
                        <h3>الشفافية الكاملة</h3>
                        <p>
                            منصة أثر تضمن لكل متبرع متابعة كل جنيه يتم التبرع
                            به، مع تقارير دقيقة وشفافة عن استخدام الأموال، لضمان
                            وصول الدعم لمن يحتاجه فعلاً.
                        </p>
                        <img
                            src={image1}
                            alt="Image1"
                            width="300"
                            height="320"
                        />
                    </div>
                    <div className="col-sm-10 col-lg-3 third-section-div">
                        <h3>جمعيات موثوقة</h3>
                        <p>
                            نحن نعمل فقط مع الجمعيات الخيرية الموثوقة والمعتمدة،
                            لضمان أن كل تبرع يذهب لمن يستحق، وأن كل مشروع يتم
                            تنفيذه بكفاءة واحترافية
                        </p>
                        <img
                            src={image2}
                            alt="Image2"
                            width="300"
                            height="300"
                        />
                    </div>
                    <div className="col-sm-10 col-lg-3 third-section-div">
                        <h3>تاثير حقيقى</h3>
                        <p>
                            هدفنا أن نخلق فرق ملموس في حياة الناس. كل حملة وكل
                            تبرع يسهم في تحسين الظروف المعيشية ودعم المبادرات
                            التي تحدث فرقاً حقيقياً
                        </p>
                        <img
                            src={image3}
                            alt="Image3"
                            width="300"
                            height="300"
                        />
                    </div>
                </div>
            </div>
        </motion.div>
    );
}
