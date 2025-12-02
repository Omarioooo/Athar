import { RxHamburgerMenu } from "react-icons/rx";
import { IoMdClose } from "react-icons/io";

export default function MenuToggle({ open, setOpen }) {
    return (
        <button className="menu-toggle" onClick={() => setOpen(!open)}>
            {open ? <IoMdClose /> : <RxHamburgerMenu />}
        </button>
    );
}
