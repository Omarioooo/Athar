import { motion } from "framer-motion";
import { getAllCharities } from "../../services/charityService";

import CharityCard from "../../components/charity/CharityCard";
import { useEffect, useState } from "react";
import { useInView } from "react-intersection-observer";
import SearchBar from "../../components/SearchBar";
import Pagination from "../../components/Pagination";
import { getTotalPages } from "../../utils/PaginationHelper";
import { isCharityFollowed } from "../../services/followService";

export default function Charities() {
    const [ref, inView] = useInView({ triggerOnce: true, threshold: 0.2 });
    const [loading, setLoading] = useState(true);
    const [charities, setCharities] = useState([]);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const handleSearch = (query) => {
        setPage(1);
        getAllCharities(query, 1).then((res) => {
            setCharities(res.charities || []);
            setTotalPages(getTotalPages(res.total, 9) || 1);
        });
    };

    useEffect(() => {
        setLoading(true);
        getAllCharities("", page)
            .then(async (response) => {
                const charitiesData = response.charities || [];

                const withFollowState = await Promise.all(
                    charitiesData.map(async (c) => {
                        const followed = await isCharityFollowed(c.id);
                        return { ...c, isFollowed: followed };
                    })
                );

                setCharities(withFollowState);
                setTotalPages(getTotalPages(response.total, 9) || 1);
            })
            .finally(() => setLoading(false));
    }, [page]);

    if (loading) {
        return (
            <div className="d-flex justify-content-center py-5">
                <div
                    className="spinner-border text-warning"
                    style={{ width: "4rem", height: "4rem" }}
                ></div>
            </div>
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
                    {charities.map((c) => (
                        <CharityCard
                            key={c.id}
                            id={c.id}
                            name={c.name}
                            description={c.description}
                            campaignsCount={c.campaignsCount}
                            img={c.imageUrl}
                            isFollowed={c.isFollowed}
                        />
                    ))}
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
