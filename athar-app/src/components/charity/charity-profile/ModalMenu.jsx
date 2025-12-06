import VolunteerModalMenu from "./VolunteerModalMenu";
import MerchantModalMenu from "./MerchantModalMenu";

export default function ModalMenu({ modalType, closeModal, id }) {
    if (!modalType) return null;

    return (
        <>
            {modalType === "volunteer" && (
                <VolunteerModalMenu id={id} closeModal={closeModal} />
            )}
            {modalType === "merchant" && (
                <MerchantModalMenu id={id} closeModal={closeModal} />
            )}
        </>
    );
}
