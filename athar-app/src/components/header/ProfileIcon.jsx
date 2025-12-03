// components/header/ProfileIcon.jsx
import { Link } from "react-router-dom";
import { UseAuth } from "../../Auth/Auth";
import { useState, useEffect } from "react";
import defaultAvatar from "../../assets/images/profile.png";
import { getDonorProfile } from "../../services/donorService";
import { getCharityProfile } from "../../services/charityService";

export default function ProfileIcon() {
    const { user } = UseAuth();
    const [profileImage, setProfileImage] = useState(defaultAvatar);

    useEffect(() => {
        if (!user) {
            setProfileImage(defaultAvatar);
            return;
        }

        const fetchProfileImage = async () => {
            try {
                let data;
                if (user.role === "Donor") {
                    data = await getDonorProfile({ id: user.id });
                } else if (user.role === "charityAdmin") {
                    data = await getCharityProfile(user.id);
                }

                if (data?.ImageUrl) {
                    setProfileImage(data.ImageUrl);
                }
            } catch (err) {
                console.log("فشل جلب صورة البروفايل:", err);
                setProfileImage(defaultAvatar);
            }
        };

        fetchProfileImage();
    }, [user]);

    const isProfileOwner =
        user && ["Donor", "charityAdmin"].includes(user.role);

    const content = (
        <img
            src={profileImage}
            alt="Profile"
            className="profile-icon"
            onError={(e) => {
                e.target.src = defaultAvatar;
            }}
        />
    );

    if (isProfileOwner) {
        return (
            <Link to={`/profile/${user.id}`} className="profile-icon">
                {content}
            </Link>
        );
    }

    return <div className="profile-icon">{content}</div>;
}