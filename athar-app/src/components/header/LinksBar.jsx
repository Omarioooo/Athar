import { NavLink } from "react-router-dom";

export default function LinksBar({ role }) {
    return (
        <div className="links-bar">
            <NavLink to="/" end>
                الرئيسية
            </NavLink>
            <NavLink to="/campaigns">حملاتنا</NavLink>
            <NavLink to="/charities">جمعياتنا</NavLink>

            <NavLink to="/content">الميديا</NavLink>

            {["Admin", "CharityAdmin"].includes(role) && (
                <NavLink to="/dashboard">لوحة التحكم</NavLink>
            )}

            {["Admin"].includes(role) && (
                <NavLink to="/join">طلبات الأنضمام</NavLink>
            )}

            <NavLink to="/zakaa">زكاتي</NavLink>
        </div>
    );
}
