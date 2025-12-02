import { Home } from "lucide-react";

const NotFound = () => {
    return (
        <div className="not-found">
            <div className="not-found-wrapper">
                <div className="error-code">404</div>

                <h1 className="error-title">آه! الصفحة ضاعت في البرية!</h1>

                <p className="error-quote">
                    "يمكن الصفحة راحت تنضم لحملة، أو تتبرع، أو تتطوّع… المهم
                    إنها مش هنا 😅"
                </p>

                <button className="home-btn">
                    <Home size={20} />
                    العودة للبيت
                </button>
            </div>
        </div>
    );
};

export default NotFound;
