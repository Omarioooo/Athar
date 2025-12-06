import { useState } from "react";
import { MessageCircle, Heart, Gift } from "lucide-react";
import ModalMenu from "./ModalMenu";

export default function CharityActions({id}) {
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
            </div>

            {/* Modal */}
            {modalType && (
                <ModalMenu modalType={modalType} closeModal={closeModal} id={id}/>
            )}
        </>
    );
}
