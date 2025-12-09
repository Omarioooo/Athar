import React, { useState } from "react";
import { CiHeart } from "react-icons/ci";
import { LiaCampgroundSolid } from "react-icons/lia";
import { LuExternalLink } from "react-icons/lu";
import { SlUserFollow } from "react-icons/sl";
import { Link } from "react-router-dom";

export default function CharityCard({
    id,
    name,
    description,
    campaignsCount,
    img,
    isFollowed: initialFollowState,
}) {
    const [isFollowed, setIsFollowed] = useState(initialFollowState);

    const toggleFollow = () => {
        setIsFollowed((prev) => !prev);
    };

    function truncateText(text, maxLength) {
        if (!text) return "";
        return text.length > maxLength
            ? text.substring(0, maxLength) + "......."
            : text;
    }

    const followStyle = {
        backgroundColor: isFollowed ? "#ffc107" : "white",
        color: isFollowed ? "black" : "#ffc107",
        border: "2px solid #ffc107",
        padding: "8px 16px",
        borderRadius: "8px",
        fontWeight: "bold",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
        gap: "6px",
        transition: "0.3s",
    };

    return (
        <>
            <div className="charity-card">
                <div className="card-head">
                    <div className="info">
                        <img src={img} alt="charity" />
                        <div className="text">
                            <p>{name}</p>
                            <span>
                                <LiaCampgroundSolid />
                                {campaignsCount} حملات
                            </span>
                        </div>
                    </div>
                    <Link to={`/charity/${id}`}>
                        <LuExternalLink className="charity-link" />
                    </Link>
                </div>

                <div className="description">
                    <p>{truncateText(description, 110)}</p>
                </div>

                <div className="charity-card-btns">
                    <div>
                        <button className="donate">
                            تبرع الآن
                            <CiHeart className="icon" />
                        </button>

                        <button style={followStyle} onClick={toggleFollow}>
                            {isFollowed ? "إلغاء المتابعة" : "متابعة"}
                            <SlUserFollow className="icon" />
                        </button>
                    </div>
                </div>
            </div>
        </>
    );
}
