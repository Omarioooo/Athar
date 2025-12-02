import axios from "axios";

export async function loginRequest(email, password) {
    return axios.post(
        "https://localhost:5192/api/Account/Login",
        {
            UserNameOrEmail: email,
            Password: password,
        },
        {
            headers: { "Content-Type": "application/json" },
        }
    );
}
