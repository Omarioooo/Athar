import { useParams, Outlet, Navigate } from "react-router-dom";
import { UseAuth } from "../Auth/Auth";
import ProfileLayout from "../layouts/ProfileLayout";
import { roleConfig } from "../routes/NavRoleConfig";

export function ProfileRoutesWrapper() {
    const { id } = useParams();
    const auth = UseAuth();
    const role = auth.user?.role;

    if (!role) return <Navigate to="/" replace />;

    if (id !== auth.user.id) {
        return <Navigate to="/not-authorized" replace />;
    }

    const config = roleConfig[role];
    if (!config) return <Navigate to="/not-authorized" replace />;

    return (
        <ProfileLayout navLinks={config.nav}>
            <Outlet context={{ config }} />
        </ProfileLayout>
    );
}
