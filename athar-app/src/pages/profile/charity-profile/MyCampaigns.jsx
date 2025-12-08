import { useEffect, useState } from "react";
import CampaignCard from "../../../components/charity/charity-campaigns/CampaignCard";
import Pagination from "../../../components/Pagination";
import { getTotalPages, paginate } from "../../../utils/PaginationHelper";
import AddCampaignButton from "../../../components/charity/charity-campaigns/AddCampaignButton";
import { UseAuth } from "../../../Auth/Auth";
import { getCharityCampaigns } from "../../../services/charityService";
import AddCampaignModal from "../../../components/modals/AddCampaignModal";

export default function MyCampaigns() {
    const { user } = UseAuth();

    const [campaigns, setCampaigns] = useState([]);
    const [loading, setLoading] = useState(true);
    const [open, setOpen] = useState(false);

    const itemsPerPage = 6;
    const [page, setPage] = useState(1);

    const [filter, setFilter] = useState("all");

    const mapStatusToFilter = (statusId) => {
        return statusId === 2 ? "completed" : "notCompleted";
    };

    useEffect(() => {
        async function loadCampaigns() {
            try {
                const data = await getCharityCampaigns(user.id);
                setCampaigns(data);
            } catch (err) {
                console.error("Failed to fetch campaigns:", err);
            } finally {
                setOpen(false);
                setLoading(false);
            }
        }

        if (user?.id) loadCampaigns();
    }, [user?.id]);

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

    const currentItems = paginate(campaigns, page, itemsPerPage);
    const filteredCampaigns =
        filter === "all"
            ? currentItems
            : currentItems.filter(
                  (c) => mapStatusToFilter(c.status) === filter
              );

    const totalPages = getTotalPages(campaigns.length, itemsPerPage);

    return (
        <>
            <div className="campaigns-wrapper">
                {/* Filter buttons */}
                <div className="campaigns-filter">
                    {["all", "completed", "notCompleted"].map((f) => (
                        <button
                            key={f}
                            className={`filter-btn ${
                                filter === f ? "active" : ""
                            }`}
                            onClick={() => setFilter(f)}
                        >
                            {f === "all" && "كل الحملات"}
                            {f === "completed" && "مكتملة"}
                            {f === "notCompleted" && "غير مكتملة"}
                        </button>
                    ))}
                </div>

                {filteredCampaigns.length > 0 ? (
                    <>
                        {/* Campaigns Cards */}
                        <div className="cards">
                            {filteredCampaigns.map((campaign) => (
                                <CampaignCard
                                    key={campaign.id}
                                    id={campaign.id}
                                    title={campaign.title}
                                    description={campaign.description}
                                    raisedAmount={campaign.raisedAmount}
                                    goalAmount={campaign.goalAmount}
                                    progress={campaign.progress}
                                    startDate={campaign.startDate}
                                    endDate={campaign.endDate}
                                    charityName={campaign.charity_name}
                                    imageUrl={campaign.imageUrl}
                                    status={campaign.statusText}
                                    statusArabic={campaign.statusArabic}
                                />
                            ))}
                        </div>

                        {/* Pagination */}
                        <Pagination
                            page={page}
                            totalPages={totalPages}
                            onPageChange={setPage}
                        />
                    </>
                ) : (
                    <div className="no-campaigns">
                        <h3>لا توجد حملات</h3>
                    </div>
                )}

                <AddCampaignButton open={open} setOpen={setOpen} />

                {open && (
                    <div className="add-campaign-overlay">
                        <div
                            className="add-campaign-modal"
                        >
                            <AddCampaignModal open={open} setOpen={setOpen} id={user.id} />
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}
