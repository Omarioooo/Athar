import {
    fetchAllNotifications,
    fetchNotificationById,
} from "../Repository/notificationRepository";

// get a single notification
export const getNotification = async (id) => {
    try {
        const notification = await fetchNotificationById(id);
        return notification;
    } catch (error) {
        throw new Error("حدث خطأ أثناء جلب الاشعار: " + error.message);
    }
};

// get all notifications for user
export const getAllNotifications = async (id) => {
    try {
        const notifications = await fetchAllNotifications(id);
        return notifications;
    } catch (error) {
        throw new Error("حدث خطأ أثناء جلب الاشعارات : " + error.message);
    }
};
