import { TrendingUp, ChevronLeft } from "lucide-react";
import CampaignCard from "../charity-campaigns/CampaignCard";
import { useState } from "react";

export default function CharityCampaigns({ campaigns }) {
    const [showAll, setShowAll] = useState(false);

    const visibleCampaigns = showAll ? campaigns : campaigns.slice(0, 3);
    return (
        <div className="charity-profile-section">
            <div className="charity-section-header">
                <div className="charity-section-header-tittle">
                    <div className="section-icon gradient-icon">
                        <TrendingUp size={24} />
                    </div>
                    <h2>الحملات الأخيرة</h2>
                </div>

                {campaigns.length > 3 && (
                    <div className="section-header-more">
                        <button
                            className="view-more-btn"
                            onClick={() => setShowAll(!showAll)}
                        >
                            {showAll ? "إظهار أقل" : "مشاهدة المزيد"}
                            <ChevronLeft size={20} />
                        </button>
                    </div>
                )}
            </div>

            <div className="campaigns-grid">
                {visibleCampaigns.map((campaign) => (
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
        </div>
    );
}
