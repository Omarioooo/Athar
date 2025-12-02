import { TbCloverFilled } from "react-icons/tb";
import { NavLink } from "react-router-dom";
export default function Logo() {
    return (
        <NavLink to="/" end>
            <div className="logo">
                <TbCloverFilled />
                <span>أثر</span>
            </div>
        </NavLink>
    );
}
