import React from "react";
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
}) {
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
                    <p>{description}</p>
                </div>
                <div className="charity-card-btns">
                    <div>
                        <button className="donate">
                            تبرع الأن
                            <CiHeart className="icon" />
                        </button>
                        <button className="follow">
                            متابعه
                            <SlUserFollow className="icon" />
                        </button>
                    </div>
                    <button className="campaigns-btn">اعرض الحملات</button>
                </div>
            </div>
        </>
    );
}
