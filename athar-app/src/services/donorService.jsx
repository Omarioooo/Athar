import { donorRegister as repoRegister } from "../Repository/donorRepository";
import { validateEmail } from "../utils//validation/validateEmail";
import {
    validatePassword,
    validateConfirmPassword,
} from "../utils//validation/validatePassword";
import {
    validateFirstName,
    validateLastName,
} from "../utils/validation/validateDonorData";
import { parseRegisterError } from "../utils/errors/parseRegisterError";
import { fetchDonorByIdFromApi } from "../Repository/donorRepository";

export function validateDonorForm(user) {
    return {
        emailerror: validateEmail(user.email),
        firstnamerror: validateFirstName(user.firstname),
        lastnameerror: validateLastName(user.lastname),
        passworderror: validatePassword(user.password),
        confirmpassworderror: validateConfirmPassword(
            user.password,
            user.confirmpassword
        ),
    };
}

export async function registerDonor(user) {
    const errors = validateDonorForm(user);

    if (Object.values(errors).some((e) => e !== null)) {
        return { success: false, errors };
    }

    const formData = new FormData();
    formData.append("Email", user.email);
    formData.append("FirstName", user.firstname);
    formData.append("LastName", user.lastname);
    formData.append("Password", user.password);
    formData.append("Country", user.country);
    formData.append("City", user.city);
    formData.append("Role", "Donor");
    if (user.img) formData.append("ProfileImage", user.img);

    try {
        await repoRegister(formData);
        return { success: true };
    } catch (err) {
        return {
            success: false,
            errors: { serverError: parseRegisterError(err) },
        };
    }
}

export async function getDonorProfile({ id }) {
    const profile = await fetchDonorByIdFromApi(id);
    return profile;
}
