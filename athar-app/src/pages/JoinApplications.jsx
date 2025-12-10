import { useEffect, useState } from "react";
import {
    approveCharity,
    getPendingCharity,
    rejectCharity,
} from "../services/charityService";

export default function JoinApplications() {
    const [loading, setLoading] = useState(true);
    const [charities, setCharities] = useState([]);

    const handleApprove = async (id) => {
        try {
            setLoading(true);
            const result = await approveCharity(id);
            setCharities((prev) =>
                prev.map((c) =>
                    c.id === id ? { ...c, status: result.status } : c
                )
            );
        } catch (error) {
            alert("حدث خطأ أثناء الموافقة على الجمعية");
        } finally {
            setLoading(false);
        }
    };

    const handleReject = async (id) => {
        try {
            setLoading(true);
            const result = await rejectCharity(id);
            setCharities((prev) =>
                prev.map((c) =>
                    c.id === id ? { ...c, status: result.status } : c
                )
            );
        } catch (error) {
            alert("حدث خطأ أثناء رفض الجمعية");
        } finally {
            setLoading(false);
        }
    };

    function truncateText(text, maxLength) {
        if (!text) return "";
        return text.length > maxLength
            ? text.substring(0, maxLength) + "......."
            : text;
    }

    useEffect(() => {
        async function loadCharities() {
            try {
                setLoading(true);
                const data = await getPendingCharity();
                console.log(data);
                
                setCharities(data);
            } catch (err) {
                console.error("Failed to fetch campaigns:", err);
            } finally {
                setLoading(false);
            }
        }
        loadCharities();
    }, []);

    
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
                                {truncateText(charity.description, 120)}
                            </p>

                            <p className="location">
                                {charity.city}, {charity.country} -{" "}
                                {new Date(
                                    charity.createdAt
                                ).toLocaleDateString()}
                            </p>

                            <div className="verification-doc">
                                <p>وثيقة التحقق:</p>
                                <img
                                    src={charity.verificationDocument}
                                    alt="Verification"
                                />
                            </div>
                        </div>
                    </div>

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
