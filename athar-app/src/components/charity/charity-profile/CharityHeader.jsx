import { Users, TrendingUp, Heart } from "lucide-react";

export default function CharityHeader({
    charity,
    isFollowing,
    setIsFollowing,
    formatNumber,
}) {
    return (
        <div className="charity-profile-header">
            <div className="charity-profile-header-content">
                <div className="charity-profile-logo-wrapper">
                    <img
                        src={charity.logo}
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
                                {formatNumber(charity.followersCount)} متابع
                            </span>
                        </div>
                        <div className="stat-divider"></div>
                        <div className="stat-item">
                            <TrendingUp size={20} />
                            <span>
                                {formatNumber(charity.campaignsCount)} حملة
                            </span>
                        </div>
                    </div>
                </div>

                <button
                    className={`follow-btn ${isFollowing ? "following" : ""}`}
                    onClick={() => setIsFollowing(!isFollowing)}
                >
                    <Heart size={18} fill={isFollowing ? "#ffca28" : "none"} />
                    {isFollowing ? "متابع" : "متابعة"}
                </button>
            </div>
        </div>
    );
}
