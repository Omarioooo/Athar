import { NavLink } from "react-router-dom";

export default function LinksBar({ role }) {
    return (
        <div className="links-bar">
            <NavLink to="/" end>
                الرئيسية
            </NavLink>
            <NavLink to="/campaigns">حملاتنا</NavLink>

            {["SuperAdmin", "CharityAdmin", "Donor"].includes(role) && (
                <NavLink to="/charities">جمعياتنا</NavLink>
            )}

            {["SuperAdmin", "CharityAdmin", "Donor"].includes(role) && (
                <NavLink to="/content">الميديا</NavLink>
            )}
            
            {["SuperAdmin", "CharityAdmin"].includes(role) && (
                <NavLink to="/dashboard">لوحة التحكم</NavLink>
            )}

            {["SuperAdmin"].includes(role) && (
                <NavLink to="/join">طلبات الأنضمام</NavLink>
            )}

            <NavLink to="/zakaa">زكاتي</NavLink>
        </div>
    );
}
