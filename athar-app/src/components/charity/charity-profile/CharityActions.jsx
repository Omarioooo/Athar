import { useState } from "react";
import { MessageCircle, Heart, Gift } from "lucide-react";
import ModalMenu from "./ModalMenu";

export default function CharityActions() {
    const [modalType, setModalType] = useState(null);

    const closeModal = () => setModalType(null);

    return (
        <>
            <div className="action-buttons-section">
                <button
                    className="action-btn merchant-btn"
                    onClick={() => setModalType("merchant")}
                >
                    <MessageCircle size={24} />
                    <span>تواصل كتاجر</span>
                </button>

                <button
                    className="action-btn volunteer-btn"
                    onClick={() => setModalType("volunteer")}
                >
                    <Heart size={24} />
                    <span>تطوع</span>
                </button>

                <button
                    className="action-btn donation-btn"
                    onClick={() => setModalType("donation")}
                >
                    <Gift size={24} />
                    <span>تبرع نوعي</span>
                </button>
            </div>

            {/* Modal */}
            {modalType && (
                <ModalMenu modalType={modalType} closeModal={closeModal} />
            )}
        </>
    );
}
