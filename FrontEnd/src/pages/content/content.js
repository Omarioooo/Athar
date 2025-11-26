import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
//this removed when  api put
import contents from "../../data/content.json";

export default function Content(){
    const [refFirstcontent, inViewFirst] = useInView({ triggerOnce: true, threshold: 0.2 });

    return(
        <>
         <motion.div
        ref={refFirstcontent}
        className="first-section-content"
        initial={{ opacity: 0, y: -50 }}
        animate={inViewFirst ? { opacity: 1, y: 0 } : {}}
        transition={{ duration: 1, ease: "easeOut" }}
        >
             <div class="container mt-3 content-section">
               <div className="row">
     {contents.map((cnt) => {
const date = new Date(cnt.createdAt);
const arabicDate = date.toLocaleDateString("ar-EG", {
  weekday: "long",
  year: "numeric",
  month: "long",     
  day: "numeric"
});

  return (
    <div key={cnt.id} className="col-xl-4 col-lg-6 col-md-12 col-sm-12 mb-4">

      <div className="card content-card" style={{ width: "400px" }}>
       <div className="img-campaign">
        <img className="card-img-top" src={cnt.postImage} alt="Card image" style={{ width: "400px" }} />
        </div>
        <div className="card-body">
            <div className="content-body"> 
          <h4 className="card-title cnt-title">{cnt.title}</h4>
          <p className="card-text content-description">{cnt.description}</p>
          </div>
          <hr/>
          <div className="reaction">
           
            <span><i class="fa-sharp fa-regular fa-heart ireaction"></i> {cnt.reactions.length}تفاعل</span>
      
            <span>{arabicDate}</span>
          </div>
        </div>

      </div>

    </div>
  );

})}
</div>
 </div>
        </motion.div>
        </>
    )
}