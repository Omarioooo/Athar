import { motion } from "framer-motion";
import { getAllCharities } from "../../services/charityService";

import CharityCard from "../../components/charity/CharityCard";
import { useEffect, useState } from "react";
import { useInView } from "react-intersection-observer";
import SearchBar from "../../components/SearchBar";
import Pagination from "../../components/Pagination";
import { getTotalPages } from "../../utils/PaginationHelper";

export default function Charities() {
    const [ref, inView] = useInView({ triggerOnce: true, threshold: 0.2 });
    const [loading, setLoading] = useState(true);
    const [charities, setCharities] = useState([]);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        setLoading(true);
        getAllCharities("", page)
            .then((response) => {
                console.log(response);
                setCharities(response.charities || []);

                setTotalPages(getTotalPages(response.total, 9) || 1);
            })
            .catch((err) => {
                console.error("Error fetching charities:", err);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [page]);

    const handleSearch = (query) => {
        setPage(1);
        getAllCharities(query, 1).then((res) => {
            setCharities(res.charities || []);
            setTotalPages(getTotalPages(res.total, 9) || 1);
        });
    };

    if (loading) {
        return (
            <>
                <div className="d-flex justify-content-center py-5">
                    <div
                        className="spinner-border text-warning"
                        style={{ width: "4rem", height: "4rem" }}
                    ></div>
                </div>
                ;
            </>
        );
    }

    return (
        <motion.div
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            <div className="charities-wrapper">
                <SearchBar title={"أبحث عن أثرك..."} onSearch={handleSearch} />
                <motion.div
                    className="charities-cards"
                    ref={ref}
                    initial={{ opacity: 0, y: -50 }}
                    animate={inView ? { opacity: 1, y: 0 } : {}}
                    transition={{ duration: 1, ease: "easeOut" }}
                >
                    {charities.map(
                        ({
                            id,
                            name,
                            description,
                            campaignsCount,
                            imageUrl,
                        }) => (
                            <CharityCard
                                key={id}
                                id={id}
                                name={name}
                                description={description}
                                campaignsCount={campaignsCount}
                                img={imageUrl}
                            />
                        )
                    )}
                </motion.div>
                <Pagination
                    page={page}
                    totalPages={totalPages}
                    onPageChange={setPage}
                />
            </div>
        </motion.div>
    );
}
