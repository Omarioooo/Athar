import { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function PaymentCallbackPage() {
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        const query = new URLSearchParams(location.search);

        const success = query.get("success") === "true";
        const pending = query.get("pending") === "true";

        // لو الدفع نجح ومش معلق
        if (success && !pending) {
            navigate("/success");
        } else {
            navigate("/failed");
        }

        // اختياري: لو عايز تبعت للـ Backend عشان تتأكد
        // fetch("https://burl-citylike-jalen.ngrok-free.dev/api/Payments/verify", { ... })
    }, [location, navigate]);

    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100">
            <div>
                <div className="spinner-border text-warning mb-4" style={{width: "5rem", height: "5rem"}}></div>
                <h3 className="text-center">جاري التحقق من عملية الدفع...</h3>
            </div>
        </div>
    );
}