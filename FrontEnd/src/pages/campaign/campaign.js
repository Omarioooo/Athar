import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

import { getAllCampaigns, getCampaignsByType, searchCampaigns, computeDaysLeft, computeProgressPercentage } from "../../service/compaignservice";

export default function Campaign() {
  const [refFirstcampaign, inViewFirst] = useInView({ triggerOnce: true, threshold: 0.2 });
  const [campaigns, setCampaigns] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPage, setTotalPage] = useState(1);
  const [keyword, setKeyword] = useState("");
  const [errorMsg, setErrorMsg] = useState("");
  const [loading, setLoading] = useState(true);
  const pageSize = 12;

  const categories = [
    { id: 0, name: "تعليم" },
    { id: 1, name: "صحة" },
    { id: 2, name: "أيتام" },
    { id: 3, name: "غذاء" },
    { id: 4, name: "مأوى" },
    { id: 99, name: "أخرى" },
  ];

  const loadCampaigns = async (page) => {
    setLoading(true);
    setErrorMsg("");
    try {
      const data = await getAllCampaigns(page, pageSize);
      setCampaigns(data.campaigns);
      setTotalPage(data.totalPages);
    } catch (error) {
      setErrorMsg(error.message);
      setCampaigns([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCampaigns(currentPage);
  }, [currentPage]);

  const handleSearch = async () => {
    if (!keyword) return;
    setLoading(true);
    setErrorMsg("");
    try {
      const data = await searchCampaigns(keyword);
      setCampaigns(data);
    } catch (error) {
      setCampaigns([]);
      setErrorMsg(error.message);
    } finally {
      setLoading(false);
    }
  };

  const handleCategoryClick = async (cat) => {
    setLoading(true);
    setErrorMsg("");
    try {
      const data = await getCampaignsByType(cat.id);
      setCampaigns(data);
    } catch (error) {
      setCampaigns([]);
      setErrorMsg(error.message);
    } finally {
      setLoading(false);
    }
  };

  const pages = Array.from({ length: totalPage }, (_, i) => i + 1);

  return (
    <motion.div
      ref={refFirstcampaign}
      className="first-section-campaign"
      initial={{ opacity: 0, y: -50 }}
      animate={inViewFirst ? { opacity: 1, y: 0 } : {}}
      transition={{ duration: 1, ease: "easeOut" }}
    >
      <div className="container mt-3 campaign-section">
        {/* Search */}
        <div className="serch-company">
          <input
            type="text"
            className="input-compaign"
            value={keyword}
            placeholder="ابحث عن حملة..."
            onChange={(e) => setKeyword(e.target.value)}
          />
          <button onClick={handleSearch} className="btn-compaign">بحث</button>
        </div>

        {/* Show error if any */}
        {/* {errorMsg && <p className="text-danger my-2">{errorMsg}</p>} */}

        {/* Categories */}
        <div className="category-buttons mb-4">
          {categories.map((cat) => (
            <button key={cat.id} className="btn btn-outline-info mx-2 category-btn" onClick={() => handleCategoryClick(cat)}>
              {cat.name}
            </button>
          ))}
        </div>

        {/* Loading Spinner */}
        {loading ? (
          <div className="d-flex justify-content-center py-5">
            <div className="spinner-border text-warning" style={{ width: "4rem", height: "4rem" }}></div>
          </div>
        ) : campaigns.length === 0 ? (
          <div className="text-center py-5">
            <h4>لا توجد حملات لعرضها</h4>
          </div>
        ) : (
          <>
            {/* Campaign Cards */}
            <div className="row">
              {campaigns.map((cmp) => {
                const daysLeft = computeDaysLeft(cmp.startDate, cmp.endDate);
                const progress = computeProgressPercentage(cmp.raisedAmount, cmp.goalAmount);

                return (
                  <div key={cmp.id} className="col-xl-4 col-lg-6 col-md-12 col-sm-12 mb-4">
                    <div className="card campaign-card" style={{ width: "400px" }}>
                      <div className="img-campaign">
                        <img className="card-img-top" src={cmp.imageUrl} alt="Card image" />
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

                        <Link to={`/campaign/${cmp.id}`} className="btn btn-warning campaign-btn">
                          <p className="p-btn">
                            تبرع الان <i className="fa-solid fa-heart first-section-i"></i>
                          </p>
                        </Link>
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
                  <li key={page} className={`page-item ${currentPage === page ? "active" : ""}`}>
                    <button className="page-link" onClick={() => setCurrentPage(page)}>
                      {page}
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          </>
        )}
      </div>
    </motion.div>
  );
}
