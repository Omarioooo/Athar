export default function DonationCard({
    imgSrc,
    campaign,
    amount,
    date,
    charity,
    className = "",
}) {
    return (
        <div className={`donation-card ${className}`}>
            <img src={imgSrc} alt={campaign} />

            <div className="card-overlay">
                <h3>{campaign}</h3>

                <div className="donation-details">
                    <span className="amount">
                        {amount.toLocaleString()} ج.م
                    </span>

                    <span className="date">
                        {new Date(date).toLocaleDateString("ar-EG", {
                            year: "numeric",
                            month: "long",
                            day: "numeric",
                        })}
                    </span>
                </div>

                <div className="charity-name">{charity}</div>
            </div>
        </div>
    );
}
