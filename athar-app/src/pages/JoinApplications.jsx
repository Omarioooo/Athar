import { useEffect, useState } from "react";
import { charitiesList } from "../utils/data";
import { getPendingCharity } from "../services/charityService";

export default function JoinApplications() {
    const [loading, setLoading] = useState(true);
    const [charities, setCharities] = useState([]);

    const handleApprove = (id) => {
        alert(`تم الموافقة على الجمعية رقم ${id}`);
        setCharities((prev) =>
            prev.map((c) => (c.id === id ? { ...c, status: "Approved" } : c))
        );
    };

    const handleReject = (id) => {
        alert(`تم رفض الجمعية رقم ${id}`);
        setCharities((prev) =>
            prev.map((c) => (c.id === id ? { ...c, status: "Rejected" } : c))
        );
    };

    function truncateText(text, maxLength) {
        if (!text) return "";
        return text.length > maxLength
            ? text.substring(0, maxLength) + "......."
            : text;
    }

    useEffect(() => {
        setLoading(true);
        async function loadCharities() {
            try {
                const data = await getPendingCharity();
                setCharities(data);
            } catch (err) {
                console.error("Failed to fetch campaigns:", err);
            } finally {
                setLoading(false);
            }
        }

        loadCharities();
    }, charities);

    return (
        <div className="cards-container">
            {charities.map((charity) => (
                <div key={charity.id} className="charity-card">
                    <div>
                        <div className="charity-image">
                            <img src={charity.imageUrl} alt={charity.name} />
                        </div>
                        <div className="charity-content">
                            <h2>{charity.name}</h2>
                            <p className="email">{charity.email}</p>
                            <p className="description">
                                {truncateText(truncateText, 120)}
                            </p>
                            <p className="location">
                                {charity.city}, {charity.country} -{" "}
                                {new Date(
                                    charity.createdAt
                                ).toLocaleDateString()}
                            </p>
                            {/* Verification Document */}
                            <div className="verification-doc">
                                <p>وثيقة التحقق:</p>
                                <img
                                    src={charity.verificationDocument}
                                    alt="Verification"
                                />
                            </div>
                        </div>
                    </div>
                    {/* Buttons */}
                    <div className="buttons">
                        <button
                            className="approve"
                            onClick={() => handleApprove(charity.id)}
                        >
                            Approve
                        </button>
                        <button
                            className="reject"
                            onClick={() => handleReject(charity.id)}
                        >
                            Reject
                        </button>
                    </div>
                </div>
            ))}
        </div>
    );
}
