import { MessageCircle } from "lucide-react";

export default function CharityDescription({ description }) {
    return (
        <section className="description-section">
            <div className="charity-section-header-tittle description-tittle">
                <div className="section-icon">
                    <MessageCircle size={24} />
                </div>
                <h2>عن الجمعية</h2>
            </div>
            <p className="charity-description">{description}</p>
        </section>
    );
}
