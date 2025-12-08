import { motion } from "framer-motion";
import { FaUsers, FaFolderOpen, FaWallet, FaInfoCircle } from "react-icons/fa";
import defaultImg from "../../../assets/images/athar5.png";
import { UseAuth } from "../../../Auth/Auth";
import { useEffect, useState } from "react";
import { getCharityProfile } from "../../../services/charityService";

export default function CharityHomeInfo() {
    const { user } = UseAuth();

    const [charity, setCharity] = useState(null);
    const [loading, setLoading] = useState(true);

    const statusColor = {
        1: "#f4b400",
        2: "#4caf50",
        3: "#e53935",
    };

    useEffect(() => {
        if (!user) return;

        const fetchData = async () => {
            try {
                const data = await getCharityProfile(user.id);
                setCharity(data);
            } catch (err) {
                console.error("Failed to fetch charity profile", err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [user]);

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

    const [country, city] = charity.address.split(",").map((s) => s.trim());
    const statusArabicById = {
        1: "قيد المراجعة",
        2: "مفعلة",
        3: "مرفوضة",
    };

    return (
        <motion.div
            className="charity-info-wrapper"
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            {/* HEADER */}
            <div className="charity-header">
                <div className="charity-avatar">
                    <img
                        src={charity.imageUrl || defaultImg}
                        alt={charity.name}
                    />
                </div>

                <div className="charity-main-info">
                    <h1 className="charity-name">{charity.name}</h1>
                    <p className="charity-location">
                        {country} - {city}
                    </p>
                    <span
                        className="charity-status"
                        style={{ background: statusColor[charity.status] }}
                    >
                        {statusArabicById[charity.status]}
                    </span>
                </div>
            </div>

            {/* ABOUT */}
            <div className="charity-description-box">
                <h2>
                    <FaInfoCircle /> عن الجمعية
                </h2>
                <p>{charity.description}</p>
            </div>

            {/* STATS */}
            <div className="charity-stats">
                <div className="stat-card">
                    <FaUsers className="icon" />
                    <span className="stat-number">
                        {charity.followersCount}
                    </span>
                    <span className="stat-label">متابعين</span>
                </div>

                <div className="stat-card">
                    <FaFolderOpen className="icon" />
                    <span className="stat-number">
                        {charity.campaignsCount}
                    </span>
                    <span className="stat-label">عدد الحملات</span>
                </div>

                <div className="stat-card">
                    <FaWallet className="icon" />
                    <span className="stat-number">
                        {charity.totalRaised?.toString()} جنيه
                    </span>
                    <span className="stat-label">الرصيد</span>
                </div>
            </div>
        </motion.div>
    );
}
