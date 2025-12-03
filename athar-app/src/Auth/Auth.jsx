// context/AuthContext.jsx
import { createContext, useState, useContext, useEffect } from "react";
import Cookies from "js-cookie";
import {jwtDecode} from "jwt-decode";

const AuthContext = createContext(null);

export const ProvideContext = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    const login = (token, userDataFromApi = null) => {
        Cookies.set("auth_token", token, {
            expires: 7,
            secure: true,
            sameSite: "strict",
            path: "/"
        });

        const userInfo = userDataFromApi || jwtDecode(token);

        Cookies.set("user", JSON.stringify(userInfo), {
            expires: 7,
            secure: true,
            sameSite: "strict",
            path: "/"
        });

        setUser(userInfo);
    };

    const logout = () => {
        Cookies.remove("auth_token", { path: "/" });
        Cookies.remove("user", { path: "/" });
        setUser(null);
    };

    useEffect(() => {
        const token = Cookies.get("auth_token");
        const storedUser = Cookies.get("user");

        if (token && storedUser) {
            try {
                const userData = JSON.parse(storedUser);
                const decoded = jwtDecode(token);

                if (decoded.exp * 1000 < Date.now()) {
                    logout();
                } else {
                    setUser(userData);
                }
            } catch (err) {
                logout();
            }
        }
        setLoading(false);
    }, []);

    return (
        <AuthContext.Provider value={{ user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export const UseAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within AuthProvider");
    }
    return context;
};