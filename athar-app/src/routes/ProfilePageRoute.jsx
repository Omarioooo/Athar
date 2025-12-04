import { useParams, Navigate } from "react-router-dom";
import { UseAuth } from "../Auth/Auth";
import ProfileLayout from "../layouts/ProfileLayout";
import { roleConfig } from "../routes/NavRoleConfig";
import NotAuthorized from "../pages/callback/NotAuth";

export default function ProfilePageRoute() {
    const { id, "*": subPath } = useParams();
    const { user } = UseAuth();

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    if (id !== user.id.toString()) {
        return <NotAuthorized />;
    }

    const config = roleConfig[user.role];

    if (!config) {
        return <NotAuthorized />;
    }

    const requestedPath = subPath || "";

    if (!requestedPath) {
        return <Navigate to={`/profile/${id}/${config.defaultPage}`} replace />;
    }

    const currentRoute = config.routes.find(r => r.path === requestedPath);

    if (!currentRoute) {
        return <Navigate to={`/profile/${id}/${config.defaultPage}`} replace />;
    }

    return (
        <ProfileLayout
            navLinks={config.nav}
            baseUrl={`/profile/${id}`}
        >
            {currentRoute.element}
        </ProfileLayout>
    );
}