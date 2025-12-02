import { useState } from "react";
import Logo from "./Logo";
import LinksBar from "./LinksBar";
import NotificationIcon from "./NotificationIcon";
import ProfileIcon from "./ProfileIcon";
import MenuToggle from "./MenuToggle";
import LinksMenu from "./LinksMenu";
import AuthButtons from "./AuthButtons";
import { UseAuth } from "../../Auth/Auth";

export default function Header({ role }) {
    const auth = UseAuth();
    const [menuOpen, setMenuOpen] = useState(false);
    return (
        <header className="header">
            <div className="header-wrap">
                <Logo />

                {/* Desktop Navigation */}
                <LinksBar role={role} />

                {!auth.user ? (
                    <AuthButtons />
                ) : (
                    <div className="header-icons">
                        <NotificationIcon />
                        <ProfileIcon role={role} />
                    </div>
                )}

                {/* Mobile Menu Button */}
                <MenuToggle open={menuOpen} setOpen={setMenuOpen} role={role} />
            </div>

            {/* Mobile Menu */}
            <LinksMenu open={menuOpen} setOpen={setMenuOpen} role={role} />
        </header>
    );
}
