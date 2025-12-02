import { motion } from "framer-motion";
import { FaUsers, FaFolderOpen, FaWallet, FaInfoCircle } from "react-icons/fa";
import defaultImg from "../../../assets/images/athar5.png";
import { charityInfo } from "../../../utils/data";

export default function CharityHomeInfo() {
    const statusColor = {
        Pending: "#f4b400",
        Active: "#4caf50",
        Rejected: "#e53935",
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
                    <img src={defaultImg} alt={charityInfo.name} />
                </div>

                <div className="charity-main-info">
                    <h1 className="charity-name">{charityInfo.name}</h1>
                    <p className="charity-location">
                        {charityInfo.country} - {charityInfo.city}
                    </p>
                    <span
                        className="charity-status"
                        style={{ background: statusColor[charityInfo.status] }}
                    >
                        {charityInfo.status === "Pending" && "قيد المراجعة"}
                        {charityInfo.status === "Active" && "مفعلة"}
                        {charityInfo.status === "Rejected" && "مرفوضة"}
                    </span>
                </div>
            </div>

            {/* ABOUT */}
            <div className="charity-description-box">
                <h2>
                    <FaInfoCircle /> عن الجمعية
                </h2>
                <p>{charityInfo.description}</p>
            </div>

            {/* STATS */}
            <div className="charity-stats">
                <div className="stat-card">
                    <FaUsers className="icon" />
                    <span className="stat-number">{charityInfo.followers}</span>
                    <span className="stat-label">متابعين</span>
                </div>

                <div className="stat-card">
                    <FaFolderOpen className="icon" />
                    <span className="stat-number">{charityInfo.campaigns}</span>
                    <span className="stat-label">عدد الحملات</span>
                </div>

                <div className="stat-card">
                    <FaWallet className="icon" />
                    <span className="stat-number">
                        {charityInfo.balance?.toString()} جنيه
                    </span>
                    <span className="stat-label">الرصيد</span>
                </div>
            </div>
        </motion.div>
    );
}
