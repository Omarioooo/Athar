import { Link } from "react-router-dom";
import profilePic from "../../assets/images/profile.png";

export default function ProfileIcon({ role }) {
    const isLink = ["donor", "charity"].includes(role);

    const content = (
        <img src={profilePic} alt="Profile" className="profile-icon" />
    );
    return (
        <>
            {isLink ? (
                <Link to="/profile" className="profile-icon">
                    {content}
                </Link>
            ) : (
                <div className="profile-icon">{content}</div>
            )}
        </>
    );
}
