import VolunteerModalMenu from "./VolunteerModalMenu";
import MerchantModalMenu from "./MerchantModalMenu";

export default function ModalMenu({ modalType, closeModal }) {
    if (!modalType) return null;

    return (
        <>
            {modalType === "volunteer" && (
                <VolunteerModalMenu closeModal={closeModal} />
            )}
            {modalType === "merchant" && (
                <MerchantModalMenu closeModal={closeModal} />
            )}
            {modalType === "donation" && (
                <MerchantModalMenu closeModal={closeModal} /> // change it
            )}
        </>
    );
}
