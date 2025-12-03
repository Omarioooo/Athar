import { UseAuth } from "../Auth/Auth";
import Header from "../components/header/Header";
import { Outlet } from "react-router-dom";

export default function MainLayout() {
    const { loading } = UseAuth();

    if (loading) {
        return (
            <>
                <div className="d-flex justify-content-center align-items-center min-vh-100">
                    <div
                        className="spinner-border text-primary"
                        style={{ width: "3rem", height: "3rem" }}
                    >
                        <span className="visually-hidden">جاري التحميل...</span>
                    </div>
                </div>
            </>
        );
    }

    return (
        <div className="container">
            <Header />
            <div className="main-body">
                <Outlet />
            </div>
        </div>
    );
}
