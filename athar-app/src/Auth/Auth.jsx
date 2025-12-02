import { createContext, useState, useContext, useEffect } from "react";
import Cookies from "js-cookie";

const Authcontext = createContext(null);

export const ProvideContext = ({ children }) => {
    const [user, setuser] = useState(null);

    const login = (userdata) => {
        setuser(userdata);
        Cookies.set("user", JSON.stringify(userdata), {
            expires: 7,
            path: "/",
        });
    };

    const logout = () => {
        setuser(null);
        Cookies.remove("user", { path: "/" });
    };

    useEffect(() => {
        const storeduser = Cookies.get("user");
        if (storeduser) setuser(JSON.parse(storeduser));
    }, []);

    return (
        <Authcontext.Provider value={{ login, logout, user }}>
            {children}
        </Authcontext.Provider>
    );
};

export const UseAuth = () => useContext(Authcontext);


