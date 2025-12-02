import { X } from "lucide-react";

export default function ModalMenu({ modalType, closeModal }) {
    const titles = {
        merchant: "التواصل كتاجر",
        volunteer: "التطوع",
        donation: "تبرع نوعي",
    };

    return (
        <div className="modal-overlay" onClick={closeModal}>
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <button className="modal-close" onClick={closeModal}>
                    <X size={22} />
                </button>

                <h2 className="modal-title">{titles[modalType]}</h2>

                <form className="modal-form">
                    <label>الاسم الكامل</label>
                    <input type="text" placeholder="أدخل اسمك" />

                    <label>رقم الجوال</label>
                    <input type="text" placeholder="05xxxxxxxx" />

                    <label>الرسالة</label>
                    <textarea placeholder="أكتب رسالتك..."></textarea>

                    <button className="submit-btn">إرسال</button>
                </form>
            </div>
        </div>
    );
}
