import { useState } from "react";
import CampaignCard from "../../../components/charity/charity-campaigns/CampaignCard";
import Pagination from "../../../components/Pagination";
import { campaigns } from "../../../utils/data";
import { getTotalPages, paginate } from "../../../utils/PaginationHelper";
import { getFilteredCampaignsByProgress } from "../../../utils/HelpersUtils";
import AddCampaignButton from "../../../components/charity/charity-campaigns/AddCampaignButton";

export default function MyCampaigns() {
    // Pagination
    const itemsPerPage = 6;
    const [page, setPage] = useState(1);
    const currentItems = paginate(campaigns, page, itemsPerPage);
    const totalPages = getTotalPages(campaigns.length, itemsPerPage);

    // Filter
    const [filter, setFilter] = useState("all");
    const filteredCampaigns = getFilteredCampaignsByProgress(
        currentItems,
        filter
    );

    return (
        <>
            <div className="campaigns-wrapper">
                {/* Filter buttons */}
                <div className="campaigns-filter">
                    {["all", "inprogress", "completed"].map((f) => (
                        <button
                            key={f}
                            className={`filter-btn ${
                                filter === f ? "active" : ""
                            }`}
                            onClick={() => setFilter(f)}
                        >
                            {f === "all" && "كل الحملات"}
                            {f === "inprogress" && "غير مكتملة"}
                            {f === "completed" && "مكتملة"}
                        </button>
                    ))}
                </div>
                {currentItems.length > 0 ? (
                    <>
                        {filteredCampaigns.length > 0 ? (
                            <>
                                {/* Campaigns Cards */}
                                <div className="cards">
                                    {filteredCampaigns.map((campaign) => (
                                        <CampaignCard
                                            key={campaign.id}
                                            title={campaign.title}
                                            description={campaign.description}
                                            raisedAmount={campaign.raisedAmount}
                                            goalAmount={campaign.goalAmount}
                                            startDate={campaign.startDate}
                                            endDate={campaign.endDate}
                                            charityName={campaign.charityName}
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
                    </>
                ) : (
                    <div className="no-campaigns">
                        <h3>لا توجد حملات حتى الآن</h3>
                        <p>ابدأ حملتك الأولى وساعدنا في إحداث أثر حقيقي</p>
                    </div>
                )}
                <AddCampaignButton />
            </div>
        </>
    );
}
