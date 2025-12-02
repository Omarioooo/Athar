// routes/ProfileDynamicRoutes.jsx
import { useOutletContext, Routes, Route, Navigate } from "react-router-dom";

export function ProfileDynamicRoutes() {
    const { config } = useOutletContext();

    if (!config || !config.routes) {
        return <Navigate to="/not-authorized" replace />;
    }

    return (
        <Routes>
            {/* Default page */}
            <Route
                index
                element={<Navigate to={config.defaultPage} replace />}
            />

            {config.routes.map(({ path, element }) => (
                <Route key={path} path={path} element={element} />
            ))}
        </Routes>
    );
}
