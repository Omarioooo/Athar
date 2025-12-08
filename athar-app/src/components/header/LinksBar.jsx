import { NavLink } from "react-router-dom";

export default function LinksBar({ role }) {
    return (
        <div className="links-bar">
            <NavLink to="/" end>
                الرئيسية
            </NavLink>
            <NavLink to="/campaigns">حملاتنا</NavLink>
            <NavLink to="/charities">جمعياتنا</NavLink>

            {/* let it after auth */}
         
                <NavLink to="/content">الميديا</NavLink>
          

            {["Admin", "CharityAdmin"].includes(role) && (
                <NavLink to="/dashboard">لوحة التحكم</NavLink>
            )}
            <NavLink to="/zakaa">زكاتي</NavLink>
        </div>
    );
}
