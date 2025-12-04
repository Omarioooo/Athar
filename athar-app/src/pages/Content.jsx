import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import { useEffect, useState } from "react";
import 'bootstrap/dist/css/bootstrap.rtl.min.css';
import Pagination from "../components/Pagination";
import {
    getAllContentsService,
    searchMediaService,
} from "../services/contentService";

export default function Content() {
    const [refFirstContent, inViewFirst] = useInView({
        triggerOnce: true,
        threshold: 0.2,
    });
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [contents, setContents] = useState([]);
    const [searchWord, setSearchWord] = useState("");
    const [loading, setLoading] = useState(true);
    const [errorMessage, setErrorMessage] = useState("");
    const pageSize = 12;

    const loadContents = async (page) => {
        setLoading(true);
        setErrorMessage("");
        const data = await getAllContentsService(page, pageSize);
        setContents(data.contents);
        setTotalPages(data.totalPages || 1);
        if (data.error) setErrorMessage(data.error);
        setLoading(false);
    };

    const handleSearch = async () => {
        if (!searchWord) return;
        setLoading(true);
        setErrorMessage("");
        const data = await searchMediaService(searchWord);
        setContents(data.contents);
        setTotalPages(data.totalPages || 1);
        if (data.error) setErrorMessage(data.error);
        setLoading(false);
    };

    useEffect(() => {
        loadContents(currentPage);
    }, [currentPage]);

    const pages = Array.from({ length: totalPages }, (_, i) => i + 1);

    return (
        <motion.div
      ref={refFirstContent}
      className="first-section-content"
      initial={{ opacity: 0, y: -50 }}
      animate={inViewFirst ? { opacity: 1, y: 0 } : {}}
      transition={{ duration: 1, ease: "easeOut" }}
    >
      <div className="container mt-3 content-section">
        {/* Search */}
        <div className="serch-company mb-4">
          <input
            type="text"
            className="input-compaign"
            value={searchWord}
            placeholder="ابحث عن ميديا..."
            onChange={(e) => setSearchWord(e.target.value)}
          />
          <button onClick={handleSearch} className="btn-compaign">بحث</button>
        </div>

        {loading ? (
          <div className="d-flex justify-content-center py-5">
            <div className="spinner-border text-warning" style={{ width: "4rem", height: "4rem" }}></div>
          </div>
        ) : errorMessage ? (
          <div className="text-center py-5 text-danger">
            <h4>{errorMessage}</h4>
          </div>
        ) : contents.length === 0 ? (
          <div className="text-center py-5">
            <h4>لا توجد ميديا لعرضها</h4>
          </div>
        ) : (
          <>
            <div className="row">
              {contents.map((cnt) => {
                const date = new Date(cnt.createdAt);
                const arabicDate = date.toLocaleDateString("ar-EG", {
                  weekday: "long", year: "numeric", month: "long", day: "numeric"
                });

                return (
                  <div key={cnt.id} className="col-xl-4 col-lg-6 col-md-12 col-sm-12 mb-4">
                    <div className="card content-card" style={{ width: "400px" }}>
                      <div className="img-campaign">
                        <div className="overlay-info">
                          <h6 className="cmpcnt-title">{cnt.campaignTitle}</h6>
                          <p className="charity-gihad">{cnt.charityName}</p>
                        </div>
                        <img className="card-img-top" src={cnt.imageUrl} alt="Card image" style={{ width: "400px" }} />
                      </div>
                      <div className="card-body">
                        <div className="content-body">
                          <h4 className="card-title cnt-title">{cnt.title}</h4>
                          <p className="card-text content-description">{cnt.description}</p>
                        </div>
                        <hr />
                        <div className="reaction d-flex justify-content-between">
                          <span><i className="fa-sharp fa-regular fa-heart ireaction"></i>تفاعل1</span>
                          <span>{arabicDate}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>

            {/* Pagination */}
             <Pagination
                                             page={currentPage}
                                             totalPages={totalPages}
                                            onPageChange={setCurrentPage}
                                                 />
          </>
        )}
      </div>
    </motion.div>
    );
}
