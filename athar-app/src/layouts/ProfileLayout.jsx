import { Outlet } from "react-router-dom";
import Nav from "../components/profile-nav/Nav";

export default function ProfileLayout({ navLinks, baseUrl }) {
    return (
        <div className="profile-body">
            <Nav links={navLinks} baseUrl={baseUrl} />
            <div className="profile-section">
                <Outlet />
            </div>
        </div>
    );
}
