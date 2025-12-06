import { useState } from "react";
import {
    UserCheck,
    Store,
    Gift,
    X,
    Phone,
    MessageSquare,
    Calendar,
    Clock,
} from "lucide-react";
import { forms } from "../../../utils/data";
import { getRelativeTimeForNotification as getRelativeTime } from "../../../utils/HelpersUtils";

export default function VolunteeringForms() {
    const [selectedType, setSelectedType] = useState("all");
    const [selectedItem, setSelectedItem] = useState(null);

    const icons = {
        volunteer: UserCheck,
        merchant: Store,
        donation: Gift,
    };

    const type = {
        volunteer: "تطوع",
        merchant: "تاجر",
    };

    const filteredForms =
        selectedType === "all"
            ? forms
            : forms.filter((f) => f.type === selectedType);

    const counts = {
        all: forms.length,
        volunteer: forms.filter((f) => f.type === "volunteer").length,
        merchant: forms.filter((f) => f.type === "merchant").length,
        donation: forms.filter((f) => f.type === "donation").length,
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
                {["all", "volunteer", "merchant", "donation"].map((t) => (
                    <button
                        key={t}
                        className={`filter-pill ${t} ${
                            selectedType === t ? "active" : ""
                        }`}
                        onClick={() => setSelectedType(t)}
                    >
                        <span>{t === "all" ? "الكل" : labels.type[t]}</span>
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

                            <div
                                className={`volunteering-card ${
                                    !item.read ? "unread" : ""
                                }`}
                            >
                                <div className="card-header">
                                    <div className="card-name">
                                        <h3>{item.fullName}</h3>
                                        {!item.read && (
                                            <span className="unread-dot"></span>
                                        )}
                                    </div>
                                    <div className="card-badges">
                                        <span
                                            className={`status-badge ${item.status}`}
                                        >
                                            {labels.status[item.status]}
                                        </span>
                                        <span
                                            className={`type-badge ${item.type}`}
                                        >
                                            {labels.type[item.type]}
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
            {selectedItem && (
                <div
                    className="modal-overlay"
                    onClick={() => setSelectedItem(null)}
                >
                    <div
                        className="modal-content"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <button
                            className="modal-close"
                            onClick={() => setSelectedItem(null)}
                        >
                            <X size={24} />
                        </button>

                        <div className={`modal-header ${selectedItem.type}`}>
                            <div className={`modal-icon ${selectedItem.type}`}>
                                {(() => {
                                    const Icon = icons[selectedItem.type];
                                    return <Icon size={24} />;
                                })()}
                            </div>
                            <div className="modal-header-text">
                                <h2>{selectedItem.fullName}</h2>
                                <span
                                    className={`type-badge ${selectedItem.type}`}
                                >
                                    {labels.type[selectedItem.type]}
                                </span>
                            </div>
                        </div>

                        <div className="modal-body">
                            <div className="modal-section">
                                <div className="modal-section-header">
                                    <MessageSquare size={18} />
                                    <h3>الوصف</h3>
                                </div>
                                <p>{selectedItem.description}</p>
                            </div>

                            <div className="modal-section">
                                <div className="modal-section-header">
                                    <MessageSquare size={18} />
                                    <h3>الرسالة</h3>
                                </div>
                                <p>{selectedItem.message}</p>
                            </div>

                            <div className="modal-info-grid">
                                <div className="modal-info-item">
                                    <Phone size={18} />
                                    <div>
                                        <span className="info-label">
                                            رقم الهاتف
                                        </span>
                                        <span className="info-value">
                                            {selectedItem.phone}
                                        </span>
                                    </div>
                                </div>

                                <div className="modal-info-item">
                                    <Calendar size={18} />
                                    <div>
                                        <span className="info-label">
                                            التاريخ
                                        </span>
                                        <span className="info-value">
                                            {new Date(
                                                selectedItem.date
                                            ).toLocaleDateString("ar-SA")}
                                        </span>
                                    </div>
                                </div>

                                <div className="modal-info-item">
                                    <Clock size={18} />
                                    <div>
                                        <span className="info-label">
                                            الوقت
                                        </span>
                                        <span className="info-value">
                                            {getRelativeTime(selectedItem.date)}
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div className="modal-actions">
                                <button
                                    className={`action-button primary ${selectedItem.type}`}
                                >
                                    الرد على الطلب
                                </button>
                                <button className="action-button secondary">
                                    تحديد كمقروء
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
