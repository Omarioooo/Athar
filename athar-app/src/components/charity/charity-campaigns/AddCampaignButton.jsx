import { useState } from "react";
import { FaPlus } from "react-icons/fa";
import AddCampaignModal from "../../modals/AddCampaignModal";

export default function AddCampaignButton() {
    const [open, setOpen] = useState(false);

    const closeModal = () => setOpen(false);

    return (
        <>
            {/* زر الإضافة */}
            <div className="add-campaign-wrapper">
                <button
                    className={`add-campaign-btn ${
                        open ? "active" : "not-active"
                    }`}
                    onClick={() => setOpen(!open)}
                >
                    <FaPlus />
                </button>
            </div>

            {/* Modal */}
            {open && (
                <div className="add-campaign-overlay" onClick={closeModal}>
                    <div
                        className="add-campaign-modal"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <AddCampaignModal closeModal={closeModal} />
                    </div>
                </div>
            )}
        </>
    );
}
