import { IoIosLogOut } from "react-icons/io";
import { NavLink, useNavigate } from "react-router-dom";
import { UseAuth } from "../../Auth/Auth";

export default function Nav({ links, baseUrl }) {
    const auth = UseAuth();
    const navigate = useNavigate();

    const handleLogout = (e) => {
        e.preventDefault();
        auth.logout();
        navigate("/", { replace: true });
    };

    return (
        <div className="profile-nav">
            <div className="links">
                {links.map(({ path, label, icon }) => (
                    <NavLink
                        key={path}
                        to={`${baseUrl}/${path}`}
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
