import { ChevronLeft, Gift, Heart } from "lucide-react";
import { useState } from "react";

export default function CharityMedia({ mediaPosts, formatNumber }) {
    const [showAll, setShowAll] = useState(false);

    const visiblePosts = showAll ? mediaPosts : mediaPosts.slice(0, 3);
    return (
        <div className="charity-profile-section">
            <div className="charity-section-header">
                <div className="charity-section-header-tittle">
                    <div className="section-icon">
                        <Gift size={24} />
                    </div>
                    <h2>آخر المنشورات</h2>
                </div>

                {mediaPosts.length > 3 && (
                    <div className="section-header-more">
                        <button
                            className="view-more-btn"
                            onClick={() => setShowAll(!showAll)}
                        >
                            {showAll ? "إظهار أقل" : "مشاهدة المزيد"}
                            <ChevronLeft size={20} />
                        </button>
                    </div>
                )}
            </div>

            <div className="media-grid">
                {visiblePosts.map((post) => (
                    <div key={post.id} className="media-card">
                        <div className="media-image">
                            <img src={post.image} alt={post.caption} />
                            <div className="media-overlay">
                                <div className="media-likes">
                                    <Heart size={20} fill="white" />
                                    <span>{formatNumber(post.likes)}</span>
                                </div>
                            </div>
                        </div>
                        <p className="media-caption">{post.caption}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}
