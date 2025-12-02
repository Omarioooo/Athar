import { UseAuth } from "../Auth/Auth";
import AdminDashboard from "../pages/dashboard/AdminDashboard";
import CharityDashBoard from "../pages/dashboard/CharityDashboard";
import NotAuth from "../pages/callback/NotAuth";

export function DashboardWrapper() {
    const auth = UseAuth();
    const role = auth.user?.role;

    if (!role) return <NotAuth />;

    switch (role) {
        case "admin":
            return <AdminDashboard />;
        case "charity":
            return <CharityDashBoard />;
        default:
            return <NotAuth />;
    }
}
