import { deleteDonorById, fetchDonorAtharById, fetchDonorInfoById, donorRegister as repoRegister, updateDonor } from "../Repository/donorRepository";
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

export async function getDonorProfile(id) {
    try {
        const profile = await fetchDonorByIdFromApi(id);
        return profile;
    } catch (err) {
        console.error("Failed to get donor profile:", err);
        throw err;
    }
}

export async function getDonorAthar(id) {
    try {
        const profile = await fetchDonorAtharById(id);
        return profile;
    } catch (err) {
        console.error("Failed to get donor athar:", err);
        throw err;
    }
}

export async function getDonorInfo(id) {
    try {
        const profile = await fetchDonorInfoById(id);
        return profile;
    } catch (err) {
        console.error("Failed to get donor athar:", err);
        throw err;
    }
}

export async function UpdateDonorData(id, data) {
    try {        
        const donor = await updateDonor(id, data);
        return donor;
    } catch (err) {
        console.error("Failed to update donor:", err);
        throw err;
    }
}

export async function deleteDonor(id) {
    try {        
        const donor = await deleteDonorById(id);
        return donor;
    } catch (err) {
        console.error("Failed to delete donor:", err);
        throw err;
    }
}

