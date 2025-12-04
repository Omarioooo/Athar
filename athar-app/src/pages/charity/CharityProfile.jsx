import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

import CharityHeader from "../../components/charity/charity-profile/CharityHeader";
import CharityActions from "../../components/charity/charity-profile/CharityActions";
import CharityDescription from "../../components/charity/charity-profile/CharityDescription";
import CharityCampaigns from "../../components/charity/charity-profile/CharityCampaigns";
import CharityMedia from "../../components/charity/charity-profile/CharityMedia";

import { getCharityView } from "../../services/charityService";

export default function CharityProfile() {
    const { id } = useParams();

    const [charity, setCharity] = useState(null);
    const [loading, setLoading] = useState(true);

    const formatNumber = (num) => (num ? num.toLocaleString("ar-SA") : "0");

    useEffect(() => {
        const loadProfile = async () => {
            if (!id) return;

            try {
                setLoading(true);
                const data = await getCharityView(id);
                console.log(data);
                
                setCharity(data);
            } catch (err) {
                console.error("فشل تحميل بيانات الجمعية:", err);
                setCharity(null);
            } finally {
                setLoading(false);
            }
        };

        loadProfile();
    }, [id]);

    if (loading) {
        return (
            <div className="flex flex-col items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-[#d4af37]"></div>
                <p className="mt-6 text-xl text-gray-700">
                    جاري تحميل بيانات الجمعية...
                </p>
            </div>
        );
    }

    if (!charity) {
        return (
            <div className="text-center py-20">
                <p className="text-2xl text-gray-600">الجمعية غير موجودة</p>
            </div>
        );
    }

    return (
        <div className="charity-profile">
            {/* Cover */}
            <div className="charity-profile-cover">
                <img src={charity.coverImage} alt="غلاف الجمعية" />
                <div className="cover-overlay"></div>
            </div>

            {/* Header */}
            <CharityHeader charity={charity} formatNumber={formatNumber} />

            {/* Content */}
            <div className="profile-container">
                <CharityDescription description={charity.description} />
                <CharityActions />

                {/* Campaigns */}
                {charity.campaigns && charity.campaigns.length > 0 && (
                    <CharityCampaigns
                        campaigns={charity.campaigns}
                        formatNumber={formatNumber}
                    />
                )}

                {/* Media Posts */}
                {charity.mediaPosts && charity.mediaPosts.length > 0 && (
                    <CharityMedia
                        mediaPosts={charity.mediaPosts}
                        formatNumber={formatNumber}
                    />
                )}
            </div>
        </div>
    );
}
