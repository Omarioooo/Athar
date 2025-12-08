import { GrInProgress } from "react-icons/gr";
import { FaCalendarAlt } from "react-icons/fa";
import {
    computeDaysLeftForCampaigns,
    computeProgressPercentageForCampaigns,
} from "../../../utils/HelpersUtils";

export default function CampaignCard({
    id,
    title,
    description,
    raisedAmount,
    goalAmount,
    startDate,
    endDate,
    charityName,
    imageUrl,
    progress,
    status,
    statusArabic,
}) {
    // progress لو جاية من الـ API استخدمها، لو مش موجودة احسبها
    const computedProgress =
        progress ?? computeProgressPercentageForCampaigns(raisedAmount, goalAmount);

    const daysLeft = computeDaysLeftForCampaigns(startDate, endDate);

    return (
        <div className="card campaign-card">

            {/* IMAGE */}
            <div className="img-campaign">
                <img
                    className="card-img-top"
                    src={imageUrl}
                    alt={title}
                    onError={(e) => (e.target.src = "/images/fallback.jpg")}
                />
            </div>

            {/* CONTENT */}
            <div className="card-body">
                <h4 className="card-title">{title}</h4>

                <p className="card-text campaign-description">{description}</p>

                {/* Status (Arabic) */}
                {statusArabic && (
                    <p className="campaign-status">
                        <strong>الحالة:</strong> {statusArabic}
                    </p>
                )}

                <div className="progress">
                    <div
                        className="progress-bar bg-warning progress-bar-striped"
                        style={{ width: `${computedProgress}%` }}
                    ></div>
                </div>

                <div className="progress-info">
                    <span className="p-complete">
                        <GrInProgress />
                        مكتمل {computedProgress}%
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
