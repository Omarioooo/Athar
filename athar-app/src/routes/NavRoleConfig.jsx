import { IoSettingsOutline } from "react-icons/io5";
import { RiHome9Line } from "react-icons/ri";
import { TbReplaceFilled } from "react-icons/tb";
import { GiSelfLove } from "react-icons/gi";
import { AiOutlineNotification } from "react-icons/ai";

import CharityHomeInfo from "../pages/profile/charity-profile/CharityHomeInfo";
import MyCampaigns from "../pages/profile/charity-profile/MyCampaigns";
import CharitySettings from "../pages/profile/charity-profile/CharitySettings";
import DonorHomeInfo from "../pages/profile/donor-profile/DonorHomeInfo";
import MyAthar from "../pages/profile/donor-profile/MyAthar";
import DonorSettings from "../pages/profile/donor-profile/DonorSettings";
import Home from "../pages/Home";
import VolunteeringForms from "../pages/profile/charity-profile/VolunteeringForms";

export const roleConfig = {
    CharityAdmin: {
        role: "charityAdmin",
        nav: [
            { label: "حسابي", path: "charityhomeinfo", icon: <RiHome9Line /> },
            { label: "حملاتي", path: "mycampaigns", icon: <TbReplaceFilled /> },
            {
                label: "تطوعات",
                path: "volunteering",
                icon: <AiOutlineNotification />,
            },
            {
                label: "الإعدادات",
                path: "charitysettings",
                icon: <IoSettingsOutline />,
            },
        ],
        routes: [
            { path: "charityhomeinfo", element: <CharityHomeInfo /> },
            { path: "mycampaigns", element: <MyCampaigns /> },
            {
                path: "volunteering",
                element: <VolunteeringForms />,
            },
            { path: "charitysettings", element: <CharitySettings /> },
        ],
        defaultPage: "charityhomeinfo",
    },

    Donor: {
        role: "Donor",
        nav: [
            { label: "حسابي", path: "donorhomeinfo", icon: <RiHome9Line /> },
            { label: "أثري", path: "myathar", icon: <GiSelfLove /> },
            {
                label: "الإعدادات",
                path: "donorsettings",
                icon: <IoSettingsOutline />,
            },
        ],
        routes: [
            { path: "donorhomeinfo", element: <DonorHomeInfo /> },
            { path: "myathar", element: <MyAthar /> },
            { path: "donorsettings", element: <DonorSettings /> },
        ],
        defaultPage: "donorhomeinfo",
    },

    SuperAdmin: {
        role: "superAdmin",
        nav: [
            { label: "الرئيسية", path: "adminhomeinfo", icon: <RiHome9Line /> },
        ],
        routes: [{ path: "home", element: <Home /> }],
        defaultPage: "home",
    },
};
