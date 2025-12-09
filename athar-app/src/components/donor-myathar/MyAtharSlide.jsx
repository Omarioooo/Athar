import { Link } from "react-router-dom";

export default function MyAtharSlide({ id, name, imgSrc }) {
    return (
        <div className="story-slide" key={id}>
            <Link to={`/charity/${id}`}>
                <div className="story-avatar">
                    <img src={imgSrc} alt={name} />
                </div>
            </Link>
            <p className="story-name">{name}</p>
        </div>
    );
}
