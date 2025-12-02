import { CgProfile } from "react-icons/cg";
import { CiCalculator1 } from "react-icons/ci";
import { FaHome } from "react-icons/fa";
import { HiOutlineBuildingOffice2 } from "react-icons/hi2";
import { ImStatsDots } from "react-icons/im";
import {
    MdOutlineCampaign,
    MdOutlineNotifications,
    MdOutlinePermMedia,
} from "react-icons/md";
import { NavLink } from "react-router-dom";

export default function LinksMenu({ open, setOpen, role }) {
    return (
        <div className={`links-menu ${open ? "open" : ""}`}>
            <NavLink to="/" onClick={() => setOpen(false)} end>
                <div className="link">
                    <FaHome />
                    الرئيسية
                </div>
            </NavLink>
            <NavLink to="/profile" onClick={() => setOpen(false)} end>
                <div className="link">
                    <CgProfile />
                    الصفحة الشخصية
                </div>
            </NavLink>

            {/* rm */}
            <NavLink to="/media" onClick={() => setOpen(false)}>
                <div className="link">
                    <MdOutlinePermMedia />
                    الميديا
                </div>
            </NavLink>

            {["superAdmin", "donor", "charityAdmin"].includes(role) || role ? (
                <NavLink to="/media" onClick={() => setOpen(false)}>
                    <div className="link">
                        <MdOutlinePermMedia />
                        الميديا
                    </div>
                </NavLink>
            ) : null}
            <NavLink to="/campaigns" onClick={() => setOpen(false)}>
                <div className="link">
                    <MdOutlineCampaign />
                    حملاتنا
                </div>
            </NavLink>
            <NavLink to="/charities" onClick={() => setOpen(false)}>
                <div className="link">
                    <HiOutlineBuildingOffice2 />
                    جمعياتنا
                </div>
            </NavLink>
            {/* rm */}
            <NavLink to="/dashboard" onClick={() => setOpen(false)}>
                <div className="link">
                    <ImStatsDots />
                    لوحة التحكم
                </div>
            </NavLink>
            {(role === "superAdmin" || role === "charityAdmin") && (
                <NavLink to="/dashboard" onClick={() => setOpen(false)}>
                    <div className="link">
                        <ImStatsDots />
                        لوحة التحكم
                    </div>
                </NavLink>
            )}
            <NavLink to="/notifications" onClick={() => setOpen(false)}>
                <div className="link">
                    <MdOutlineNotifications />
                    الإشعارات
                </div>
            </NavLink>
            <NavLink to="/zakaa" onClick={() => setOpen(false)}>
                <div className="link">
                    <CiCalculator1 />
                    زكاتي
                </div>
            </NavLink>
        </div>
    );
}
