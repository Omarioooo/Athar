import { useState, useMemo } from "react";
import { AnimatePresence } from "framer-motion";
import { Bell } from "lucide-react";
import { notifications } from "../utils/data";
import Pagination from "../components/Pagination";
import NotificationCard from "../components/NotificationCard";
import { getTotalPages, paginate } from "../utils/PaginationHelper";
import { IoCheckmarkDone } from "react-icons/io5";

export default function Notifications() {
    // notification state
    const [readIds, setReadIds] = useState([]);
    const markAllAsRead = () => {
        setReadIds(notifications.map((n) => n.id));
    };

    const [filter, setFilter] = useState("all");

    const filteredNotifications = useMemo(() => {
        if (filter === "read")
            return notifications.filter((n) => readIds.includes(n.id));
        if (filter === "unread")
            return notifications.filter((n) => !readIds.includes(n.id));
        return notifications;
    }, [filter, readIds]);

    // Pagination
    const [page, setPage] = useState(1);
    const itemsPerPage = 3;
    const currentItems = paginate(filteredNotifications, page, itemsPerPage);
    const totalPages = getTotalPages(
        filteredNotifications.length,
        itemsPerPage
    );
    console.log("page no# : " + page);
    console.log("no# items per page is " + itemsPerPage);
    console.log("items are " + currentItems);

    return (
        <div className="notifications-modern-container">
            <div className="notifications-wrapper">
                <div className="notifications-header">
                    <h1 className="notifications-title">إشعاراتي</h1>

                    {/* Filter Buttons */}
                    <div className="notifications-filters">
                        <button
                            className={`filter-btn ${
                                filter === "all" ? "active" : ""
                            }`}
                            onClick={() => setFilter("all")}
                        >
                            الكل
                        </button>

                        <button
                            className={`filter-btn ${
                                filter === "read" ? "active" : ""
                            }`}
                            onClick={() => setFilter("read")}
                        >
                            مقروء
                        </button>

                        <button
                            className={`filter-btn ${
                                filter === "unread" ? "active" : ""
                            }`}
                            onClick={() => setFilter("unread")}
                        >
                            غير مقروء
                        </button>
                    </div>
                </div>

                {/* Read-All Button */}
                <button
                    className={`read-all-btn ${
                        readIds.length !== notifications.length ? "active" : ""
                    }`}
                    onClick={markAllAsRead}
                >
                    <IoCheckmarkDone size={24} />
                </button>

                <AnimatePresence mode="popLayout">
                    <div className="notifications-box">
                        {!notifications.length ? (
                            <div className="not-notifications">
                                <h3>لا يوجد إشعارات</h3>
                            </div>
                        ) : !currentItems.length ? (
                            <div className="no-notifications">
                                <h3> لا يوجد نتائج للفلترة</h3>
                            </div>
                        ) : (
                            currentItems.map((item) => (
                                <NotificationCard
                                    key={item.id}
                                    item={item}
                                    readIds={readIds}
                                    setReadIds={setReadIds}
                                />
                            ))
                        )}
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
                    <div className="empty-state">
                        <Bell size={100} />
                        <h3>لا توجد إشعارات حاليًا</h3>
                        <p>سنُعلمك بكل جديد فور حدوثه</p>
                    </div>
                )}
            </div>
        </div>
    );
}
