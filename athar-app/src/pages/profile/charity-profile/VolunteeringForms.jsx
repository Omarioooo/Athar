import { useEffect, useState } from "react";
import {
    UserCheck,
    Store,
    X,
    Phone,
    MessageSquare,
    Calendar,
    Clock,
} from "lucide-react";
import { getRelativeTimeForNotification as getRelativeTime } from "../../../utils/HelpersUtils";
import { UseAuth } from "../../../Auth/Auth";
import { fetchCharityApplications } from "../../../services/formsService";
import VolunteerModal from "../../../components/modals/VolunteerModal";
import VendorModal from "../../../components/modals/VendorModal";

export default function VolunteeringForms() {
    const { user } = UseAuth();

    const [applications, setApplications] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedType, setSelectedType] = useState("all");
    const [selectedItem, setSelectedItem] = useState(null);

    const icons = {
        Volunteer: UserCheck,
        VendorOffer: Store,
    };

    const type = {
        Volunteer: "تطوع",
        VendorOffer: "تاجر",
    };

    useEffect(() => {
        async function loadApplications() {
            try {
                const data = await fetchCharityApplications(user.id);
                console.log("data apps are", data);

                setApplications(data);
            } catch (err) {
                console.error("Failed to fetch applications:", err);
            } finally {
                setLoading(false);
            }
        }
        if (user?.id) loadApplications();
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

    const filteredForms =
        selectedType === "all"
            ? applications
            : applications.filter((f) => f.type === selectedType);

    const counts = {
        all: applications.length,
        Volunteer: applications.filter((f) => f.type === "Volunteer").length,
        VendorOffer: applications.filter((f) => f.type === "VendorOffer")
            .length,
    };

    return (
        <div className="contact-requests-page">
            <div className="page-header">
                <h1 className="page-title">طلبات التواصل والتطوع</h1>
                <p className="page-subtitle">
                    إدارة ومتابعة طلبات المتطوعين والتجار والتبرعات النوعية
                </p>
            </div>

            {/* FILTER BAR */}
            <div className="filter-bar">
                {["all", "Volunteer", "VendorOffer"].map((t) => (
                    <button
                        key={t}
                        className={`filter-pill ${t} ${
                            selectedType === t ? "active" : ""
                        }`}
                        onClick={() => setSelectedType(t)}
                    >
                        <span>{t === "all" ? "الكل" : type[t]}</span>
                        <span className="pill-count">{counts[t]}</span>
                    </button>
                ))}
            </div>

            {/* volunteering */}
            <div className="volunteering-container">
                {filteredForms.map((item, index) => {
                    const Icon = icons[item.type];
                    return (
                        <div
                            key={item.id}
                            className={`volunteering-item ${item.type}`}
                            onClick={() => setSelectedItem(item)}
                        >
                            <div className="volunteering-line-wrapper">
                                <div
                                    className={`volunteering-circle ${item.type}`}
                                >
                                    <Icon size={20} />
                                </div>
                                {index < filteredForms.length - 1 && (
                                    <div
                                        className={`volunteering-line ${item.type}`}
                                    ></div>
                                )}
                            </div>

                            <div className={`volunteering-card`}>
                                <div className="card-header">
                                    <div className="card-name">
                                        <h3>{item.name}</h3>
                                    </div>
                                    <div className="card-badges">
                                        <span
                                            className={`type-badge ${item.type}`}
                                        >
                                            {type[item.type]}
                                        </span>
                                    </div>
                                </div>

                                <p className="card-description">
                                    {item.description}
                                </p>

                                <div className="card-footer">
                                    <span className="card-time">
                                        <Clock size={14} />
                                        {getRelativeTime(item.date)}
                                    </span>
                                    <span className="card-phone">
                                        <Phone size={14} />
                                        {item.phone}
                                    </span>
                                </div>
                            </div>
                        </div>
                    );
                })}
            </div>

            {/* MODAL */}
            {selectedItem && selectedItem.type === "Volunteer" && (
                <VolunteerModal
                    id={selectedItem.id}
                    onClose={() => setSelectedItem(null)}
                    getRelativeTime={getRelativeTime}
                />
            )}

            {selectedItem && selectedItem.type === "VendorOffer" && (
                <VendorModal
                    id={selectedItem.id}
                    onClose={() => setSelectedItem(null)}
                    getRelativeTime={getRelativeTime}
                />
            )}
        </div>
    );
}
