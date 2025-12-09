import { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { CreatePaymentCallBack } from "../services/paymentService";

export default function PaymentCallbackPage() {
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        const query = new URLSearchParams(location.search);
        const formData = {
            paymentId: query.get("paymentId"),
            status: query.get("status"),
        };

        CreatePaymentCallBack(formData)
            .then(() => navigate("/success"))
            .catch(() => navigate("/failed"));
    }, [location, navigate]);

    return (
        <div className="d-flex justify-content-center py-5">
            <div
                className="spinner-border text-warning"
                style={{ width: "4rem", height: "4rem" }}
            ></div>
        </div>
    );
}