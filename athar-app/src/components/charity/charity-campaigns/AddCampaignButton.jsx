import { FaPlus } from "react-icons/fa";

export default function AddCampaignButton({ open, setOpen }) {
    console.log(open);
    
    return (
        <>
            <div className="add-campaign-wrapper">
                <button
                    className={`add-campaign-btn ${open ? "active" : "not-active"}`}
                    onClick={() => setOpen(true)}
                >
                    <FaPlus />
                </button>
            </div>
        </>
    );
}