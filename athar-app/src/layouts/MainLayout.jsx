import { UseAuth } from "../Auth/Auth";
import Header from "../components/header/Header";
import { Outlet } from "react-router-dom";

export default function MainLayout() {
    const { user, loading } = UseAuth();

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    const role = user?.role || null;

    return (
        <div className="container">
            <Header role={role} />
            <div className="main-body">
                <Outlet />
            </div>
        </div>
    );
}
