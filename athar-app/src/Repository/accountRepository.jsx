import api from "../Auth/AxiosInstance";

export async function loginRequest(email, password) {
    return api.post("/Account/Login", {
        UserNameOrEmail: email,
        Password: password,
    });
}