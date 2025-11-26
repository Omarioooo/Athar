import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import React, { useState, useEffect } from "react";

import { fetchAllCampaigns, fetchCampaignsByType } from "../../repository/compaignyrepository";
import { computeDaysLeft, computeProgressPercentage } from "../../service/compaignservice";
import { SearchCompaign } from "../../repository/compaignyrepository";
export default function Campaign() {
  const [refFirstcampaign, inViewFirst] = useInView({ triggerOnce: true, threshold: 0.2 });
  const [campaigns, setCampaigns] = useState([]);
  const[Currentpage,setCurrentpage]=useState(1);
 const[totalpage,settotalpage]=useState(1);
 const[keywoard,setkeyword]=useState("");
 const pagesize=12;
  const categories = [
    { id: 0, name: "تعليم" },
    { id: 1, name: "صحة" },
    { id: 2, name: "أيتام" },
    { id: 3, name: "غذاء" },
    { id: 4, name: "مأوى" },
    { id: 99, name: "أخرى" },
  ];
 const loadcompaigns = async (page) => {
  const response = await fetchAllCampaigns(page, pagesize);
  setCampaigns(response.compaign);
  settotalpage(response.totalpage); 
}
const handlesearch=async()=>{
   if (!keywoard) return; 
  const response= await SearchCompaign(keywoard)
  setCampaigns(response);
}
useEffect(() => {
 loadcompaigns(Currentpage);
}, [Currentpage]);

  const handleCategoryClick = async (cat) => {
    const result = await fetchCampaignsByType(cat.id);
    setCampaigns(result);
  };
const pages = []
for(var i=1;i<=totalpage;i++)pages.push(i);
console.log("Pages array:", pages);

  return (

    <motion.div
      ref={refFirstcampaign}
      className="first-section-campaign"
      initial={{ opacity: 0, y: -50 }}
      animate={inViewFirst ? { opacity: 1, y: 0 } : {}}
      transition={{ duration: 1, ease: "easeOut" }}
    >
      <div className="container mt-3 campaign-section">
        <div className="serch-company">
<input
type="text"
className="input-compaign"
value={keywoard}
  placeholder="ابحث عن حملة..."
onChange={(e)=>setkeyword(e.target.value)}/>
<button onClick={handlesearch}className="btn-compaign">بحث</button>
</div>
        {/* Categories */}

        <div className="category-buttons mb-4">
          {categories.map((cat) => (
            <button
              key={cat.id}
              className="btn btn-outline-info mx-2 category-btn "
              onClick={() => handleCategoryClick(cat)}
            >
              {cat.name}
            </button>
          ))}
        </div>

        {/* Campaign Cards */}
        <div className="row">
          {campaigns.map((cmp) => {
               let imageSrc = null;
             if (cmp.imageUrl) {
           imageSrc = cmp.imageUrl;
           } else if (cmp.image) {
           imageSrc = `data:image/jpeg;base64,${cmp.image}`;
             }
            const daysLeft = computeDaysLeft(cmp.startDate, cmp.endDate);
            const progress = computeProgressPercentage(cmp.raisedAmount, cmp.goalAmount);

            return (
              <div key={cmp.id} className="col-xl-4 col-lg-6 col-md-12 col-sm-12 mb-4">
                <div className="card campaign-card" style={{ width: "400px" }}>
                  <div className="img-campaign">
       <img className="card-img-top" src={imageSrc} alt="Card image" />
                  </div>
                  <div className="card-body">
                    <h4 className="card-title cmp-title">{cmp.title}</h4>
                    <p className="card-text campaign-description">{cmp.description}</p>

                    <div className="progress" style={{ backgroundColor: "rgba(78, 182, 230, 0.927)", height: "10px" }}>
                      <div className="progress-bar bg-warning progress-bar-striped" style={{ width: `${progress}%` }}></div>
                    </div>

                    <div className="p-underprogress">
                      <span className="p-complete">مكتمل {progress}%</span>
                      <span className="time-left">متبقي: {daysLeft} يوم</span>
                    </div>

                    <a href="#" className="btn btn-warning campaign-btn">
                     <p className="p-btn"> تبرع الان<i className="fa-solid fa-heart first-section-i"></i></p>
                    </a>
                  </div>
                </div>
              </div>
            );
          })}
        </div>

        {/* Pagination */}
        
    <div style={{ display: "flex", justifyContent: "center", marginTop: "20px" }}>
  <ul className="pagination">
    {pages.map((page) => (
      <li key={page} className={`page-item ${Currentpage === page ? "active" : ""}`}>
        <button className="page-link" onClick={() => setCurrentpage(page)}>
          {page}
        </button>
      </li>
    ))}
  </ul>
</div>
      </div>
    </motion.div>
  );
}
