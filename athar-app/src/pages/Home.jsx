import Footer from "../components/Footer";
import FirstSection from "../components/home/FirstSection";
import SecondSection from "../components/home/SecondSection";
import ThirdSection from "../components/home/ThirdSection";

export default function Home() {
    return (
        <>
            <div className="home-page">
                <FirstSection />
                <SecondSection />
                <ThirdSection />
            </div>
            <Footer />
        </>
    );
}
