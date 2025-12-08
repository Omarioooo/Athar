import { motion } from "framer-motion";
import MyAtharSlide from "../../../components/donor-myathar/MyAtharSlide";
import MyAtharSlider from "../../../components/donor-myathar/MyAtharSlider";
import { useEffect, useState } from "react";
import { getFilteredDonations } from "../../../utils/HelpersUtils";
import { getTotalPages, paginate } from "../../../utils/PaginationHelper";
import Pagination from "../../../components/Pagination";
import DonationCard from "../../../components/donor-myathar/DonationCard";
import { UseAuth } from "../../../Auth/Auth";
import { getDonorAthar } from "../../../services/donorService";

export default function MyAthar() {
    const { user } = UseAuth();

    const [athar, setAthar] = useState();
    const [totalDonationsAmount, setTotal] = useState(0);
    const [follows, setFollows] = useState([]);
    const [donations, setDonations] = useState([]);
    const [loading, setLoading] = useState();
    const [page, setPage] = useState(1);
    const [filter, setFilter] = useState("all");

    useEffect(() => {
        async function loadAtharData() {
            try {
                const data = await getDonorAthar(user.id);
                console.log("athar data ", data);
                setAthar(data);

                setTotal(data.amount);
                setFollows(data.follows);
                setDonations(data.donations);
            } catch (err) {
                console.error("Failed to fetch athar data: ", err);
            } finally {
                setLoading(false);
            }
        }

        loadAtharData();
    }, [user.id]);

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

    // Pagination
    const itemsPerPage = 6;
    const currentItems = paginate(donations, page, itemsPerPage);
    const totalPages = getTotalPages(donations, itemsPerPage);

    // Filter
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
                                className="follow-card"
                                key={follow.id}
                                id={follow.charityId}
                                imgSrc={follow.charityImageUrl}
                                name={follow.charityName}
                            />
                        ))}
                    </MyAtharSlider>
                </div>

                {/* Total donations box */}
                <div className="total-donation-box">
                    <h2>إجمالي تبرعاتك حتى الآن</h2>
                    <div className="big-amount">
                        {totalDonationsAmount.toString()} <span>جنيه مصري</span>
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
                                        key={don.DonationId}
                                        id={don.DonationId}
                                        imgSrc={don.CampaignImgUrl}
                                        campaign={don.CampaignName}
                                        amount={don.Amount}
                                        date={don.Date}
                                        charity={don.CharityName}
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
