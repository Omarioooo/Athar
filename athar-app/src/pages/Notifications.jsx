import { useState, useMemo, useEffect } from "react";
import { AnimatePresence } from "framer-motion";
import { Bell } from "lucide-react";
import { UseAuth } from "../Auth/Auth";
import Pagination from "../components/Pagination";
import NotificationCard from "../components/NotificationCard";
import { getTotalPages, paginate } from "../utils/PaginationHelper";
import { fetchAllNotifications } from "../Repository/notificationRepository";

export default function Notifications() {
    const { user } = UseAuth();

    const [notifications, setNotifications] = useState([]);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadNotifications() {
            try {
                setLoading(true);
                const data = await fetchAllNotifications(user.id);
                console.log(data);

                setNotifications(data);
            } catch (err) {
                console.error("Failed to fetch campaigns:", err);
            } finally {
                setLoading(false);
            }
        }

        if (user?.id) loadNotifications();
    }, [user?.id]);

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
                <AnimatePresence mode="popLayout">
                    <div className="notifications-box">
                        {currentItems.map((item) => (
                            <NotificationCard key={item.id} item={item} />
                        ))}
                    </div>
                </AnimatePresence>
                {totalPages > 1 && (
                    <Pagination
                        page={page}
                        totalPages={totalPages}
                        onPageChange={setPage}
                    />
                )}
                {notifications.length === 0 && (
                    <div className="empty-notifications">
                        <Bell size={100} />
                        <h3>لا توجد إشعارات حاليًا</h3>
                        <p>سنُعلمك بكل جديد فور حدوثه</p>
                    </div>
                )}
            </div>
        </div>
    );
}
