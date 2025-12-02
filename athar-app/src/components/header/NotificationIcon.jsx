import { MdOutlineNotifications } from "react-icons/md";
import { Link } from "react-router-dom";

export default function NotificationIcon() {
    return (
        <Link to="/notifications" className="notification-icon">
            <MdOutlineNotifications size={35} />
        </Link>
    );
}
