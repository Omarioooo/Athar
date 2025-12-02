import { Outlet } from "react-router-dom";
import Nav from "../components/profile-nav/Nav";

export default function ProfileLayout({ navLinks }) {
    return (
        <div className="profile-body">
            <Nav links={navLinks} />
            <Outlet className="profile-section" />
        </div>
    );
}
