import Nav from "../components/profile-nav/Nav";

export default function ProfileLayout({ navLinks, baseUrl, children }) {
    return (
        <div className="profile-body">
            <Nav links={navLinks} baseUrl={baseUrl} />
            <div className="profile-section">
                {children}
            </div>
        </div>
    );
}
