import { UseAuth } from "../Auth/Auth";
import { Navigate } from "react-router-dom";

export const RequireDonorAuth = ({ children }) => {
    const auth = UseAuth();

    if (!auth.user) {
        return <Navigate to="/login" />;
    }

    if (auth.user.role !== "Donor") {
        return <Navigate to="/" />;
    }

    return children;
};
