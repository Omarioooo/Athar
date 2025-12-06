import VolunteerModalMenu from "./VolunteerModalMenu";
import MerchantModalMenu from "./MerchantModalMenu";

export default function ModalMenu({ modalType, closeModal, id }) {
    if (!modalType) return null;

    console.log("ModalMenu id:", id, "Type:", typeof id);

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
