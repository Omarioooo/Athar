import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import defaultAvatar from "../../assets/images/profile.png";
import { getDonorProfile } from "../../services/donorService";
import { getCharityProfile } from "../../services/charityService";

export default function ProfileIcon({ user }) {
    const [profileImage, setProfileImage] = useState(defaultAvatar);

    useEffect(() => {
        if (!user?.id) {
            setProfileImage(defaultAvatar);
            return;
        }

        const fetchImage = async () => {
            try {
                let data = null;
                if (user.role === "Donor") {
                    data = await getDonorProfile(user.id);
                } else if (user.role === "CharityAdmin") {
                    data = await getCharityProfile(user.id);
                }

                const imgUrl = data?.imageUrl || defaultAvatar;
                if (imgUrl) {
                    setProfileImage(data.imageUrl);
                }
            } catch (err) {
                setProfileImage(defaultAvatar);
            }
        };

        fetchImage();
    }, [user?.id, user?.role]);

    const isProfile = user && ["Donor", "CharityAdmin"].includes(user.role);
    const content = (
        <img
            src={profileImage}
            alt="الملف الشخصي"
            className="profile-icon"
            onError={(e) => {
                e.target.onerror = null;
                e.target.src = profileImage;
            }}
        />
    );

    if (isProfile) {
        return (
            <Link to={`/profile/${user.id}`} className="profile-icon">
                {content}
            </Link>
        );
    }

    return <div className="profile-icon">{content}</div>;
}
