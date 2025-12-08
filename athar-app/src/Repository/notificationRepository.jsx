import api from "../Auth/AxiosInstance";

// get a notification by id
export const fetchNotificationById = async (id) => {
    const res = await api.get(`/Notifications/${id}`);
    return res.data;
};


// get all notifications for the account
export const fetchAllNotifications = async (id) => {
    const res = await api.get(`/Notifications/all/${id}`);
    return res.data;
};