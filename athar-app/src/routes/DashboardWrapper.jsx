import { UseAuth } from "../Auth/Auth";
import MainLayout from "../layouts/MainLayout";
import AdminDashboard from "../pages/dashboard/AdminDashboard";
import CharityDashBoard from "../pages/dashboard/CharityDashboard";
import NotAuth from "../pages/callback/NotAuth";

export function DashboardWrapper() {
    const { user } = UseAuth();
    const role = user?.role;

    if (role == null || (role !== "Admin" && role !== "CharityAdmin")) {
        return <NotAuth />;
    }

    switch (role) {
        case "Admin":
            return (
                <>
                    <MainLayout /> <AdminDashboard />
                </>
            );

        case "CharityAdmin":
            return (
                <>
                    {" "}
                    <MainLayout /> <CharityDashBoard />
                </>
            );

        default:
            return <NotAuth />;
    }
}
