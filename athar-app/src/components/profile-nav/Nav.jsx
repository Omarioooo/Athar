import { IoIosLogOut } from "react-icons/io";
import { Navigate, NavLink, replace } from "react-router-dom";
import { UseAuth } from "../../Auth/Auth";
import { Replace } from "lucide-react";

export default function Nav({ links }) {
    const auth = UseAuth();
    const handleLogout = (e) => {
        e.preventDefault();
        auth.logout();
        Navigate("/", Replace);
    };
    return (
        <div className="profile-nav">
            <div className="links">
                {links.map(({ path, label, icon }) => (
                    <NavLink
                        key={path}
                        to={path}
                        className={({ isActive }) =>
                            `link ${isActive ? "active" : ""}`
                        }
                        data-title={label}
                    >
                        {icon}
                        <span>{label}</span>
                    </NavLink>
                ))}
            </div>
            <button
                data-title="تسجيل خروج"
                className="logout"
                onClick={handleLogout}
            >
                <IoIosLogOut />
                <span>تسجيل خروج</span>
            </button>
        </div>
    );
}
