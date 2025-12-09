import { useState, useEffect } from "react";
import { AnimatePresence, motion } from "framer-motion";
import { Bell, Clock } from "lucide-react";
import { UseAuth } from "../Auth/Auth";
import Pagination from "../components/Pagination";
import { getTotalPages, paginate } from "../utils/PaginationHelper";
import {
    fetchAllNotifications,
    fetchNotificationById,
} from "../Repository/notificationRepository";

export default function Notifications() {
    const { user } = UseAuth();

    const [notifications, setNotifications] = useState([]);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);

    const [open, setOpen] = useState(false);
    const [selected, setSelected] = useState(null);

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

    useEffect(() => {
        async function loadNotifications() {
            try {
                setLoading(true);
                const data = await fetchAllNotifications(user.id);
                console.log(data);
                console.log(user.id);

                setNotifications(data);
            } catch (err) {
                console.error("Failed to fetch notifications:", err);
            } finally {
                setLoading(false);
            }
        }
        if (user?.id) loadNotifications();
    }, [user?.id]);

    const openModal = async (id) => {
        try {
            const data = await fetchNotificationById(id);
            setSelected(data);
            setOpen(true);
        } catch (err) {
            console.error("Error loading notification:", err);
        }
    };

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

    // Pagination
    const itemsPerPage = 3;
    const currentItems = paginate(notifications, page, itemsPerPage);
    const totalPages = getTotalPages(notifications.length, itemsPerPage);

    return (
        <div className="notifications-modern-container">
            <div className="notifications-wrapper">
                <div className="notifications-header">
                    <h1 className="notifications-title">إشعاراتي</h1>
                </div>

                {notifications.length === 0 && (
                    <div className="empty-notifications">
                        <Bell size={100} />
                        <h3>لا توجد إشعارات حاليًا</h3>
                        <p>سنُعلمك بكل جديد فور حدوثه</p>
                    </div>
                )}

                <AnimatePresence mode="popLayout">
                    <div className="notifications-box">
                        {currentItems.map((item) => (
                            <motion.div
                                key={item.id}
                                layout
                                initial={{ opacity: 0, y: 20, scale: 0.98 }}
                                animate={{ opacity: 1, y: 0, scale: 1 }}
                                exit={{ opacity: 0, y: -15, scale: 0.98 }}
                                className="notification-card"
                                onClick={() => openModal(item.id)}
                                style={{ cursor: "pointer" }}
                            >
                                <div className="notification-bar" />
                                <div className="notification-inner">
                                    <div className="notification-icon-wrapper">
                                        <Bell size={32} />
                                    </div>

                                    <div
                                        className="notification-content"
                                        style={{ flex: 1 }}
                                    >
                                        <h3>{`إشعار جديد من `}<span className="sender">{item.senderName}</span></h3>
                                        <p>{item.message}</p>
                                        <div className="notification-meta">
                                            <Clock size={16} />
                                            <span className="time-ago">
                                                {formatTime(item.createdAt)}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </motion.div>
                        ))}
                    </div>
                </AnimatePresence>
                {/* Modal */}
                <AnimatePresence>
                    {open && selected && (
                        <motion.div
                            className="confirm-overlay"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            exit={{ opacity: 0 }}
                            onClick={() => setOpen(false)}
                        >
                            <motion.div
                                className="notification-box"
                                initial={{ scale: 0.8, opacity: 0 }}
                                animate={{ scale: 1, opacity: 1 }}
                                exit={{ scale: 0.8, opacity: 0 }}
                                transition={{ duration: 0.2 }}
                                onClick={(e) => e.stopPropagation()}
                            >
                                <h2>تفاصيل الإشعار</h2>

                                <p>
                                    <strong>الرسالة:</strong> {selected.message}
                                </p>
                                <p>
                                    <strong>المرسل:</strong>{" "}
                                    {selected.sender?.userName}
                                </p>

                                <h4>المستلمين:</h4>
                                <ul>
                                    {selected.receivers.map((r) => (
                                        <li key={r.receiverId}>
                                            {r.userName} -{" "}
                                        </li>
                                    ))}
                                </ul>

                                <button
                                    className="btn btn-warning mt-3"
                                    onClick={() => setOpen(false)}
                                >
                                    إغلاق
                                </button>
                            </motion.div>
                        </motion.div>
                    )}
                </AnimatePresence>

                {totalPages > 1 && (
                    <Pagination
                        page={page}
                        totalPages={totalPages}
                        onPageChange={setPage}
                    />
                )}
            </div>
        </div>
    );
}
