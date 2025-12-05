import { motion } from "framer-motion";
import { useInView } from "react-intersection-observer";
import { useEffect, useState } from "react";
import 'bootstrap/dist/css/bootstrap.rtl.min.css';
import Pagination from "../components/Pagination";
import ContentCard from "../components/content/content";
import { UseAuth } from "../Auth/Auth";
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
 const {user } = UseAuth(); 
    const loadContents = async (page) => {
        setLoading(true);
        setErrorMessage("");
        const data = await getAllContentsService(page, pageSize);
        setContents(data.contents);
        setTotalPages(data.totalPages || 1);
        if (data.error) setErrorMessage(data.error);
        setLoading(false);
         
  console.log("User ID:", user.id);
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
                    <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 mb-4">
         <ContentCard key={cnt.id} cnt={cnt} arabicDate={arabicDate} user={user} />
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
    
