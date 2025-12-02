import { person } from "../../../utils/data";
import { motion } from "framer-motion";
import profilePic from "../../../assets/images/profile.png";

export default function DonorHomeInfo() {
    const donation_history = person.donation_history;
    return (
        <motion.div
            className="home-info-wrapper"
            animate={{ opacity: 1 }}
            initial={{ opacity: 0 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
        >
            <div className="section-container">
                <div className="section-info">
                    <div className="avatar-section">
                        <div className="avatar">
                            <img src={profilePic} alt={person.user.name} />
                        </div>
                        <p className="username">{person.user.name}</p>
                    </div>

                    <div className="stats">
                        <div className="stat-card">
                            <span className="stat-label">الاشتراكات</span>
                            <span className="stat-value">
                                {person.user.stats.subscriptions}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">المتابعات</span>
                            <span className="stat-value">
                                {person.user.stats.follows}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">
                                إجمالي عدد التبرعات
                            </span>
                            <span className="stat-value">
                                {person.user.stats.donations_count}
                            </span>
                        </div>
                        <div className="stat-card">
                            <span className="stat-label">
                                إجمالي مبلغ التبرعات
                            </span>
                            <span className="stat-value">
                                {person.user.stats.total_donations_amount.toLocaleString()}{" "}
                                جنيه
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div className="donation-history">
                <h2 className="section-title">سجل التبرعات</h2>

                <div className="donation-list">
                    {donation_history.length > 0 ? (
                        donation_history.map((donation) => (
                            <div className="donation-item" key={donation.id}>
                                <div className="donation-details">
                                    <h3 className="donation-title">
                                        {donation.title}
                                    </h3>
                                    <span className="donation-time">
                                        {donation.time_ago}
                                    </span>
                                </div>

                                <div className="donation-amount">
                                    {donation.amount} {donation.currency}
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="no-donations">لا يوجد تبرعات حتى الآن</p>
                    )}
                </div>
            </div>
            {/* </div> */}
        </motion.div>
    );
}
