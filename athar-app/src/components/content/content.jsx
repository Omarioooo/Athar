import React, { useEffect, useState } from "react";
import { ReacttoMedia } from "../../Repository/contentRepository";
import { getcountofreaction } from "../../Repository/contentRepository";
function ContentCard({ cnt, arabicDate ,showCampaignInfo = true,user}) {
  const[isliked,setisliked]=useState(false);
  const[reactioncount,setreactioncount]=useState(0);
  const [loading, setLoading] = useState(false);
  
    const togglereact=async()=>{
      if(!user) return alert("يجب تسجيل الدخول للتفاعل");
      setLoading(true);
      try{
          await ReacttoMedia(cnt.id,!isliked);
          setisliked(!isliked);
          const count=await getcountofreaction(cnt.id);
           setreactioncount(count);
          
      }
       catch (err) { 
        console.error(err); alert(err.response?.data?.message || "حدث خطأ أثناء التفاعل"); 
      } finally {
         setLoading(false); 
        }
    }
    useEffect(() => {
  if (!cnt || !cnt.id) return;

  const fetchCount = async () => {
    try {
      const count = await getcountofreaction(cnt.id);
      setreactioncount(count);
    } catch (err) {
      console.error(err);
    }
  };

  fetchCount();
}, [cnt]);
  return (
   
      <div className="card content-card" style={{ width: "400px" }}>
        <div className="img-campaign">
         {showCampaignInfo && (
            <div className="overlay-info">
              <h6 className="cmpcnt-title">{cnt.campaignTitle}</h6>
              <p className="charity-gihad">{cnt.charityName}</p>
            </div>
          )}

          <img
            className="card-img-top"
            src={cnt.imageUrl}
            alt="Card image"
            style={{ width: "400px" }}
          />
        </div>

        <div className="card-body">
          <div className="content-body">
            <h4 className="card-title cnt-title">{cnt.title}</h4>
            <p className="card-text content-description">{cnt.description}</p>
          </div>

          <hr />

          <div className="reaction d-flex justify-content-between">
             <span style={{ cursor: "pointer" }} onClick={togglereact} className={isliked ? "text-danger" : "text-muted"}>
            <i className={`fa-sharp fa-heart ${isliked ? "fa-solid" : "fa-regular"} me-2 likkk`}></i>
            {reactioncount}
          </span>
            <span>{arabicDate}</span>
          </div>
        </div>
      </div>
 
  );
}

export default ContentCard;