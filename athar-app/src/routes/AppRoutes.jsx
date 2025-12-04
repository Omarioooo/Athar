import { AnimatePresence } from "framer-motion";
import { Routes, Route, useLocation } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import { RequireAuth } from "../Auth/RequireAuth";
import { DashboardWrapper } from "./DashboardWrapper";

// Pages
import Home from "../pages/Home";
import Zakaa from "../pages/Zakaa";
import Login from "../pages/login/Login";
import Register from "../pages/register/Register";
import DonorRegister from "../pages/register/DonorRegister";
import CharityRegister from "../pages/register/CharityRegister";
import Notifications from "../pages/Notifications";
import Content from "../pages/Content";
import Campaigns from "../pages/campaign/Campaigns";
import CampaignDetails from "../pages/campaign/CampaignDetails";
import Charities from "../pages/charity/Charities";
import CharityProfile from "../pages/charity/CharityProfile";
import NotFound from "../pages/callback/NotFound";
import ProfilePageRoute from "./ProfilePageRoute";
import ProfilePageRouteWrapper from "./ProfilePageRouteWrapper";

export default function AppRoutes() {
    const location = useLocation();
    return (
        <AnimatePresence mode="wait">
            <Routes location={location} key={location.pathname}>
                {/* Public Routes */}
                <Route element={<MainLayout />}>
                    <Route index element={<Home />} />
                    <Route path="zakaa" element={<Zakaa />} />
                    <Route path="content" element={<Content />} />
                    <Route path="campaigns" element={<Campaigns />} />
                    <Route path="charities" element={<Charities />} />
                    <Route path="campaign/:id" element={<CampaignDetails />} />
                    <Route path="charity/:id" element={<CharityProfile />} />

                    {/* Protected Routes */}
                    <Route element={<RequireAuth />}>
                        <Route
                            path="notifications"
                            element={<Notifications />}
                        />
                        <Route
                            path="profile/:id/*"
                            element={<ProfilePageRoute />}
                        />
                    </Route>
                </Route>

                {/* DashBoard */}
                <Route
                    path="dashboard/*"
                    element={
                        <RequireAuth>
                            <DashboardWrapper />
                        </RequireAuth>
                    }
                />

                {/* Auth Routes (without layout) */}
                <Route path="/login" element={<Login />} />

                <Route path="/signup">
                    <Route index element={<Register />} />

                    <Route path="donorregister" element={<DonorRegister />} />

                    <Route
                        path="charityregister"
                        element={<CharityRegister />}
                    />
                </Route>

                {/* 404 */}
                <Route path="*" element={<NotFound />} />
            </Routes>
        </AnimatePresence>
    );
}
