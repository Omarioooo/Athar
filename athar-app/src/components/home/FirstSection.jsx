import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import { useNavigate } from "react-router-dom";

export default function FirstSection() {
    const [ref, inView] = useInView({ triggerOnce: true, threshold: 0.2 });
  const navigate = useNavigate();

  const goToCharities = () => {
    navigate("/charities"); 
   
  };
    return (
        <motion.div
            ref={ref}
            className="first-section"
            initial={{ opacity: 0, y: -50 }}
            animate={inView ? { opacity: 1, y: 0 } : {}}
            transition={{ duration: 1, ease: "easeOut" }}
        >
            <div className="first-section-text">
                <span className="first-section-h1">صنع </span>
                <span className="first-section-h2">اثر حقيقى</span>
                <h1 className="first-section-h1">فى حياة المحتاجين</h1>
                <p className="first-section-p">
                    منصة رقمية شفافة تربط بين الجمعيات الخيرية الموثوقة
                    والمتبرعين، لتحقيق أقصى استفادة من كل جنيه يتم التبرع به
                </p>
                <button className="first-section-btn1">
                    <i className="fa-solid fa-heart first-section-i"></i> ابدا
                    بالتبرع الان
                </button>
                <button className="first-section-btn2" onClick={goToCharities}>
               تصفح الجمعيات{" "}
            <i className="fa-solid fa-arrow-left first-section-i"></i>
           </button>
            </div>
        </motion.div>
    );
}
