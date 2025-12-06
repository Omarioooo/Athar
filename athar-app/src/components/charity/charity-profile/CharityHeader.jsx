import { Users, TrendingUp, Heart } from "lucide-react";
import { useEffect, useState } from "react";
import {
    followCharity,
    isCharityFollowed,
    unFollowCharity,
} from "../../../services/followService";

export default function CharityHeader({
    id,
    charity,
    formatNumber,
}) {
    const [isFollowed, setIsFollowed] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadFollowState();
    }, [id]);

    async function loadFollowState() {
        setLoading(true);

        const followed = await isCharityFollowed(id);

        setIsFollowed(followed);
        setLoading(false);
    }

    async function toggleFollow() {
        try {
            if (isFollowed) {
                await unFollowCharity(id);
                setIsFollowed(false);
            } else {
                await followCharity(id);
                setIsFollowed(true);
            }
        } catch (err) {
            alert(err.message || "Error happened!");
        }
    }

    console.log("Is followed , ", isFollowed);
    

    if (loading) {
        return (
            <div className="flex flex-col items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-[#d4af37]"></div>
                <p className="mt-6 text-xl text-gray-700">
                    جاري تحميل بيانات الجمعية...
                </p>
            </div>
        );
    }
    return (
        <div className="charity-profile-header">
            <div className="charity-profile-header-content">
                <div className="charity-profile-logo-wrapper">
                    <img
                        src={charity.imgUrl}
                        alt={charity.name}
                        className="charity-profile-logo"
                    />
                </div>

                <div className="charity-profile-info">
                    <h1 className="charity-profile-name">{charity.name}</h1>

                    <div className="charity-profile-stats">
                        <div className="stat-item">
                            <Users size={20} />
                            <span>
                                {formatNumber(charity.numOfFollowers)} متابع
                            </span>
                        </div>
                        <div className="stat-divider"></div>
                        <div className="stat-item">
                            <TrendingUp size={20} />
                            <span>
                                {formatNumber(charity.numOfCampaigns)} حملة
                            </span>
                        </div>
                    </div>
                </div>

                <button
                    className={`follow-btn ${isFollowed ? "following" : ""}`}
                    onClick={() => toggleFollow()}
                >
                    <Heart size={18} fill={isFollowed ? "#ffca28" : "none"} />
                    {isFollowed ? "متابع" : "متابعة"}
                </button>
            </div>
        </div>
    );
}
