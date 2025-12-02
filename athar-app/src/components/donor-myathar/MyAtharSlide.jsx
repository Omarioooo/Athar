export default function MyAtharSlide({ id, name, imgSrc }) {
    return (
        <div className="story-slide" key={id}>
            <div className="story-avatar">
                <img src={imgSrc} alt={name} />
            </div>
            <p className="story-name">{name}</p>
        </div>
    );
}
