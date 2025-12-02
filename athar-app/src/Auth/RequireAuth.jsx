import { UseAuth } from "../Auth/Auth";
import { Navigate } from "react-router-dom";

export const RequireAuth = ({ children }) => {
    const { user, loading } = UseAuth();

    if (loading) {
        return <div>Loading...</div>;
    }

    if (!user) {
        return <Navigate to="/login" replace={true} />;
    }

    return children;
};
