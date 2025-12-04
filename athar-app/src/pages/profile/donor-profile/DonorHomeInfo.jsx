import { useEffect, useState } from "react";
import { motion } from "framer-motion";
import defaultAvatar from "../../../assets/images/profile.png";
import { UseAuth } from "../../../Auth/Auth";
import { getDonorProfile } from "../../../services/donorService";

export default function DonorHomeInfo() {
    function useDonorProfile() {
        const { user } = UseAuth();
        console.log(user);

        const [profile, setProfile] = useState(null);
        const [loading, setLoading] = useState(true);
        const [error, setError] = useState(null);

        useEffect(() => {
            if (!user?.id) {
                setError("يجب تسجيل الدخول أولاً");
                setLoading(false);
                return;
            }

            getDonorProfile({ id: user.id })
                .then((data) => {
                    if (data!==null) {
                        setProfile(data);
                    } else {
                        setError("غير مسموح لك بعرض هذه الصفحة");
                    }
                })
                .catch((err) => {
                    setError(err.message || "فشل تحميل الملف الشخصي");
                })
                .finally(() => {
                    setLoading(false);
                });
        }, [user?.id]);

        return { profile, loading, error };
    }

    const { profile, loading, error } = useDonorProfile();
    const [now, setNow] = useState(new Date());

    useEffect(() => {
        const interval = setInterval(() => setNow(new Date()), 60000);
        return () => clearInterval(interval);
    }, []);

    if (loading) {
        return (
            <div className="text-center py-5">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">
                        جاري تحميل بياناتك...
                    </span>
                </div>
            </div>
        );
    }

    if (error || !profile) {
        return (
            <div className="text-center py-5">
                <div className="alert alert-danger">
                    {error || "فشل تحميل الملف الشخصي"}
                </div>
            </div>
        );
    }

    const fullName =
        `${profile.firstName} ${profile.lastName}`.trim() || "مستخدم";

    return (
        <motion.div
            className="home-info-wrapper"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.4 }}
        >
            <div className="section-container">
                <div className="section-info">
                    {/* Avatar Section */}
                    <div className="avatar-section">
                        <div className="avatar">
                            <img
                                src={profile.imageUrl || defaultAvatar}
                                alt={fullName}
                                onError={(e) => {
                                    e.target.onerror = null;
                                    e.target.src = defaultAvatar;
                                }}
                            />
                        </div>
                        <p className="username">{fullName}</p>
                        {profile.city && profile.country && (
                            <p className="user-location text-muted">
                                {profile.city}, {profile.country}
                            </p>
                        )}
                    </div>

                    {/* Stats */}
                    <div className="stats">
                        <div className="stat-card">
                            <span className="stat-label">إجمالي التبرعات</span>
                            <span className="stat-value">
                                {profile.donationsCount}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">المتابعات</span>
                            <span className="stat-value">
                                {profile.followingCount}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">عدد التبرعات</span>
                            <span className="stat-value">
                                {profile.donationsCount}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">إجمالي المبلغ</span>
                            <span className="stat-value">
                                {Math.round(
                                    profile.totalDonationsAmount
                                ).toLocaleString()}{" "}
                                جنيه
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            {/* Donation History */}
            <div className="donation-history">
                <h2 className="section-title">سجل التبرعات</h2>

                <div className="donation-list">
                    {profile.donationsHistory &&
                    profile.donationsHistory.length > 0 ? (
                        profile.donationsHistory
                            .sort(
                                (a, b) =>
                                    new Date(b.donationDate) -
                                    new Date(a.donationDate)
                            )
                            .map((donation) => (
                                <div
                                    className="donation-item"
                                    key={donation.donationId}
                                >
                                    <div className="donation-details">
                                        <h3 className="donation-title">
                                            تبرع لحملة #{donation.campaignId}
                                            {donation.status ===
                                                "Completed" && (
                                                <span className="badge bg-success ms-2">
                                                    مكتمل
                                                </span>
                                            )}
                                        </h3>
                                        <span className="donation-time">
                                            {donation.donationDate}
                                        </span>
                                    </div>
                                    <div className="donation-amount">
                                        +{donation.amount.toLocaleString()}{" "}
                                        {donation.currency}
                                    </div>
                                </div>
                            ))
                    ) : (
                        <p className="no-donations text-center py-4 text-muted">
                            لم تقم بأي تبرعات حتى الآن
                        </p>
                    )}
                </div>
            </div>
        </motion.div>
    );
}
