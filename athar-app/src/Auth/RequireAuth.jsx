import { UseAuth } from "../Auth/Auth";
import { Navigate, Outlet, useLocation } from "react-router-dom";

export const RequireAuth = ({ children }) => {
    const { user, loading } = UseAuth();
    const location = useLocation();

    if (loading) {
        return null;
    }

    if (!user) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    return children || <Outlet />;
};
