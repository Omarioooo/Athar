import {
    X,
    UserCheck,
    Phone,
    Calendar,
    Clock,
    MessageSquare,
} from "lucide-react";
import { getRelativeTimeForNotification as getRelativeTime } from "../../utils/HelpersUtils";
import { useEffect, useState } from "react";
import { fetchApplicationDetails } from "../../services/formsService";

export default function VolunteerModal({ id, onClose }) {
    const [item, setItem] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadApplications() {
            try {
                const data = await fetchApplicationDetails(id, "Volunteer");
                console.log(data);

                setItem(data);
            } catch (err) {
                console.error("Failed to fetch applications:", err);
            } finally {
                setLoading(false);
            }
        }
        if (id) loadApplications();
    }, [id]);

    return (
        <div className="application-modal-overlay" onClick={onClose}>
            <div className="application-modal-content" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={onClose}>
                    <X size={24} />
                </button>

                <div className="application-header">
                    <div className="application-modal-icon Volunteer">
                        <UserCheck size={24} />
                    </div>
                    <div className="application-modal-header-text">
                        <h2>{item.firstName + " " + item.lastName}</h2>
                        <span className="type-badge Volunteer">متطوع</span>
                    </div>
                </div>

                <div className="application-modal-body">
                    <div className="application-modal-section">
                        <div className="application-modal-section-header">
                            <MessageSquare size={18} />
                            <h3>الوصف</h3>
                        </div>
                        <p>{item.description}</p>
                    </div>

                    <div className="application-modal-info-grid">
                        <div className="application-modal-info-item">
                            <Phone size={18} />
                            <div className="item">
                                <span className="info-value">
                                    {item.phoneNumber}
                                </span>
                            </div>
                        </div>

                        <div className="application-modal-info-item">
                            <Calendar size={18} />
                            <div className="item">
                                <span className="info-value">
                                    {new Date(item.date).toLocaleDateString(
                                        "ar-SA"
                                    )}
                                </span>
                            </div>
                        </div>

                        <div className="application-modal-info-item">
                            <Clock size={18} />
                            <div className="item">
                                <span className="info-value">
                                    {getRelativeTime(item.date)}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
