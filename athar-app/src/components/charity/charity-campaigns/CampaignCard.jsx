import { GrInProgress } from "react-icons/gr";
import imageSrc from "../../../assets/images/campaign.jpg";
import { FaCalendarAlt } from "react-icons/fa";
import {
    computeDaysLeftForCampaigns,
    computeProgressPercentageForCampaigns,
} from "../../../utils/HelpersUtils";

export default function CampaignCard({
    title,
    description,
    raisedAmount,
    goalAmount,
    startDate,
    endDate,
    charityName,
}) {
    const daysLeft = computeDaysLeftForCampaigns(startDate, endDate);
    const progress = computeProgressPercentageForCampaigns(
        raisedAmount,
        goalAmount
    );
    return (
        <div className="card campaign-card">
            {/* IMAGE */}
            <div className="img-campaign">
                <img className="card-img-top" src={imageSrc} alt={title} />
            </div>

            {/* CONTENT */}
            <div className="card-body">
                <h4 className="card-title">{title}</h4>

                <p className="card-text campaign-description">{description}</p>

                <div className="progress">
                    <div
                        className="progress-bar bg-warning progress-bar-striped"
                        style={{ width: `${progress}%` }}
                    ></div>
                </div>

                <div className="progress-info">
                    <span className="p-complete">
                        <GrInProgress />
                        مكتمل {progress}%
                    </span>
                    <span className="time-left">
                        <FaCalendarAlt />
                        متبقي: {daysLeft} يوم
                    </span>
                </div>

                <button className="cmp-donate-btn">
                    <p className="p-btn">تبرع الآن</p>
                </button>
            </div>
        </div>
    );
}
