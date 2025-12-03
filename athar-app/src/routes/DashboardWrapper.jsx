import { UseAuth } from "../Auth/Auth";
import AdminDashboard from "../pages/dashboard/AdminDashboard";
import CharityDashBoard from "../pages/dashboard/CharityDashboard";
import NotAuth from "../pages/callback/NotAuth";

export function DashboardWrapper() {
    const { user } = UseAuth();
    const role = user?.role;


    console.log("role is ", role);
    
    if (!role || (role !== "superAdmin" && role !== "charityAdmin")) {
        return <NotAuth />;
    }

    switch (role) {
        case "superAdmin":
            return <AdminDashboard />;
        case "charityAdmin":
            return <CharityDashBoard />;
        default:
            return <NotAuth />;
    }
}
