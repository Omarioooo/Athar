import { UseAuth } from "../Auth/Auth";
import { Navigate } from "react-router-dom";

export const RequireCharityAdminAuth = ({ children }) => {
  const auth = UseAuth();

  if (!auth.user) {
    return <Navigate to="/login" />;
  }

  if (auth.user.role !== "CharityAdmin") {
    return <Navigate to="/" />;
  }

  return children;
};