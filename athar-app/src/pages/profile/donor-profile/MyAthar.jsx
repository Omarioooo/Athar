import { motion } from "framer-motion";
import { atharData } from "../../../utils/data";
import MyAtharSlide from "../../../components/donor-myathar/MyAtharSlide";
import MyAtharSlider from "../../../components/donor-myathar/MyAtharSlider";
import athar1 from "../../../assets/images/athar1.png";
import athar5 from "../../../assets/images/athar5.png";
import { useState } from "react";
import { getFilteredDonations } from "../../../utils/HelpersUtils";
import { getTotalPages, paginate } from "../../../utils/PaginationHelper";
import Pagination from "../../../components/Pagination";
import DonationCard from "../../../components/donor-myathar/DonationCard";

export default function MyAthar() {
    const follows = atharData.follows;
    const donations = atharData.donations;
    const totalAmount = 5000;

    // Pagination
    const itemsPerPage = 6;
    const [page, setPage] = useState(1);

    const currentItems = paginate(donations, page, itemsPerPage);
    const totalPages = getTotalPages(donations.length, itemsPerPage);

    // Filter
    const [filter, setFilter] = useState("all");

    const filteredDonations = getFilteredDonations(currentItems, filter);
    return (
        <motion.div
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
            className="my-athar-page"
        >
            <div className="section-container">
                {/* Follows slider */}
                <div className="follows athar-section">
                    <MyAtharSlider key="follow-slider">
                        {follows.map((follow) => (
                            <MyAtharSlide
                                key={follow.id}
                                className="follow-card"
                                id={follow.id}
                                imgSrc={athar5}
                                name={follow.name}
                            />
                        ))}
                    </MyAtharSlider>
                </div>

                {/* Total donations box */}
                <div className="total-donation-box">
                    <h2>إجمالي تبرعاتك حتى الآن</h2>
                    <div className="big-amount">
                        {totalAmount.toString()} <span>جنيه مصري</span>
                    </div>
                </div>

                {/* Filter buttons */}
                <div className="donations-filter">
                    {["all", "day", "week", "month", "year"].map((f) => (
                        <button
                            key={f}
                            className={`filter-btn ${
                                filter === f ? "active" : ""
                            }`}
                            onClick={() => setFilter(f)}
                        >
                            {f === "all" && "كل التبرعات"}
                            {f === "day" && "اليوم"}
                            {f === "week" && "آخر أسبوع"}
                            {f === "month" && "هذا الشهر"}
                            {f === "year" && "هذه السنة"}
                        </button>
                    ))}
                </div>

                {/* Donations */}
                <>
                    {filteredDonations.length ? (
                        <div className="donations-wrapper">
                            <div className="donations-list">
                                {filteredDonations.map((don) => (
                                    <DonationCard
                                        key={don.id}
                                        id={don.id}
                                        imgSrc={athar1}
                                        campaign={don.campaign}
                                        amount={don.amount}
                                        date={don.date}
                                        charity={don.charity}
                                    />
                                ))}
                            </div>
                            <Pagination
                                page={page}
                                totalPages={totalPages}
                                onPageChange={setPage}
                            />
                        </div>
                    ) : (
                        <div className="empty-donations">
                            لا توجد تبرعات في هذه الفترة
                        </div>
                    )}
                </>
            </div>
        </motion.div>
    );
}
