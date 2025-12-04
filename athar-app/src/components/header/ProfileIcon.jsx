// components/header/ProfileIcon.jsx
import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import defaultAvatar from "../../assets/images/profile.png";
import { getDonorProfile } from "../../services/donorService";
import { getCharityProfile } from "../../services/charityService";

export default function ProfileIcon({user}) {
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
                    data = await getDonorProfile({ id: user.id });
                } else if (user.role === "CharityAdmin") {
                    data = await getCharityProfile({ id: user.id });
                }

                const imgUrl = data?.imageUrl;
                if (imgUrl) {
                    // setProfileImage(imgUrl);
                    setProfileImage(
                        "https://image.shutterstock.com/image-vector/businessman-multi-tasking-skill-260nw-150498062.jpg"
                    );
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
                e.target.src = defaultAvatar;
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
