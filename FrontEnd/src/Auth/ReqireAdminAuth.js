import { UseAuth } from "../Auth/Auth";
import { Navigate } from "react-router-dom";

export const RequireAdminAuth = ({ children }) => {
  const auth = UseAuth();


  if (!auth.user) {
    return <Navigate to="/login" />;
  }

  if (auth.user.role !== "Admin") {
    return <Navigate to="/" />;
  }

  return children;
};