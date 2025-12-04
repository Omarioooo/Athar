// components/header/Header.jsx
import { useState } from "react";
import Logo from "./Logo";
import LinksBar from "./LinksBar";
import NotificationIcon from "./NotificationIcon";
import ProfileIcon from "./ProfileIcon";
import MenuToggle from "./MenuToggle";
import LinksMenu from "./LinksMenu";
import AuthButtons from "./AuthButtons";
import { UseAuth } from "../../Auth/Auth";

export default function Header() {
    const { user, loading } = UseAuth();
    const [menuOpen, setMenuOpen] = useState(false);


    if (loading) {
        return (
            <header className="header">
                <div className="header-wrap">
                    <Logo />
                    <div
                        className="skeleton skeleton-text"
                        style={{ width: 200, height: 40 }}
                    ></div>
                </div>
            </header>
        );
    }

    const role = user?.role || null;

    return (
        <header className="header">
            <div className="header-wrap">
                <Logo />
                {/* LinkBar for large Screen */}
                <LinksBar role={role} />

                {user ? (
                    <div className="header-icons">
                        <NotificationIcon />
                        <ProfileIcon user={user} />
                    </div>
                ) : (
                    <AuthButtons />
                )}

                {/* Menu btn for small Screen */}
                <MenuToggle open={menuOpen} setOpen={setMenuOpen} />
            </div>

            {/* Menu for small Screen */}
            <LinksMenu
                open={menuOpen}
                setOpen={setMenuOpen}
                user={user}
            />
        </header>
    );
}
