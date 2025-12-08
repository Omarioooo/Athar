import React, { useEffect, useState } from "react";
import { ReacttoMedia, getcountofreaction, deletecontent } from "../../Repository/contentRepository";

function ContentCard({ cnt, arabicDate, showCampaignInfo = true, user, onEdit }) {
  const [isLiked, setIsLiked] = useState(false);
  const [reactionCount, setReactionCount] = useState(0);
  const [loading, setLoading] = useState(false);

  // ============================
  // 🔥 Delete Content
  // ============================
  const handleDelete = async () => {
    if (!cnt?.id) return;

    const confirmDelete = window.confirm("هل تريد ازالة الميديا؟");
    if (!confirmDelete) return;

    try {
      setLoading(true);
      await deletecontent(cnt.id);
      // إنتي هتعملي تحديث للقائمة من الصفحة الأب
    } catch (error) {
      console.log(error.message);
    } finally {
      setLoading(false);
    }
  };

  // ============================
  // 💙 Toggle Reaction
  // ============================
  const toggleReact = async () => {
    if (!user) return alert("يجب تسجيل الدخول للتفاعل");

    try {
      setLoading(true);

      await ReacttoMedia(cnt.id, !isLiked);

      setIsLiked(!isLiked);
      
      const count = await getcountofreaction(cnt.id);
      setReactionCount(count);

    } catch (err) {
      console.error(err);
      alert(err.response?.data?.message || "حدث خطأ أثناء التفاعل");
    } finally {
      setLoading(false);
    }
  };

  // ============================
  // 📌 Fetch Reaction Count on Load
  // ============================
  useEffect(() => {
    if (!cnt?.id) return;

    const fetchCount = async () => {
      try {
        const count = await getcountofreaction(cnt.id);
        setReactionCount(count);
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
          <span
            style={{ cursor: "pointer" }}
            onClick={toggleReact}
            className={isLiked ? "text-danger" : "text-muted"}
          >
            <i className={`fa-sharp fa-heart ${isLiked ? "fa-solid" : "fa-regular"} me-2 likkk`}></i>
            {reactionCount}
          </span>

          <span>{arabicDate}</span>
        </div>

        {/* 🔥 Buttons only for Charity Admin && not in campaign details */}
        {!showCampaignInfo && user && user.role === "CharityAdmin" && (
          <>
            <button
              className="btn btn-danger"
              style={{ margin: "auto", marginLeft: "9px" }}
              onClick={handleDelete}
            >
              حذف الميديا
            </button>

            <button
              className="btn btn-primary"
              style={{ margin: "auto", backgroundColor: "rgba(78, 182, 230, 0.927)" }}
              onClick={() => onEdit(cnt)}
            >
              تعديل الميديا
            </button>
          </>
        )}
      </div>
    </div>
  );
}

export default ContentCard;
