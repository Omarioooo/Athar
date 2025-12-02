import { motion } from "framer-motion";
import { Clock, Bell } from "lucide-react";

export default function NotificationCard({ item, readIds, setReadIds }) {
    const markAsRead = (id) => {
        if (!readIds.includes(id)) {
            setReadIds((prev) => [...prev, id]);
        }
    };

    const formatTime = (dateString) => {
        if (!dateString) return "";
        const now = new Date();
        const date = new Date(dateString);
        const diffMs = now.getTime() - date.getTime();
        const diffMins = Math.floor(diffMs / 60000);
        const diffHours = Math.floor(diffMs / 3600000);
        const diffDays = Math.floor(diffMs / 86400000);

        if (diffMins < 1) return "الآن";
        if (diffMins < 60) return `منذ ${diffMins} دقيقة`;
        if (diffHours < 24) return `منذ ${diffHours} ساعة`;
        if (diffDays === 1) return "أمس";
        if (diffDays < 7) return `منذ ${diffDays} أيام`;
        return date.toLocaleDateString("ar-EG");
    };

    const isUnread = !readIds.includes(item.id);

    return (
        <motion.div
            key={item.id}
            layout
            initial={{ opacity: 0, y: 20, scale: 0.98 }}
            animate={{
                opacity: 1,
                y: 0,
                scale: 1,
                transition: {
                    duration: 0.25,
                    ease: "easeOut",
                },
            }}
            exit={{
                opacity: 0,
                y: -15,
                scale: 0.98,
                transition: {
                    duration: 0.2,
                    ease: "easeIn",
                },
            }}
            onClick={() => markAsRead(item.id)}
            className={`notification-card ${isUnread ? "unread" : ""}`}
        >
            <div className={`notification-bar ${isUnread ? "not-read" : ""}`} />
            <div className="notification-inner">
                <div className="notification-icon-wrapper">
                    <Bell size={32} />
                </div>
                <div className="notification-content" style={{ flex: 1 }}>
                    <h3>{item.title || "إشعار جديد"}</h3>
                    <p>{item.message}</p>
                    <div className="notification-meta">
                        <span className="time-ago">
                            <Clock size={16} />{" "}
                            {formatTime(item.created_at || item.date)}
                        </span>
                    </div>
                </div>
            </div>
        </motion.div>
    );
}
