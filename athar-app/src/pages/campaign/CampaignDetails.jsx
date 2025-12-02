import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchCampaignById } from "../../services/campaignService";

export default function CampaignDetail() {
    const { id } = useParams();
    const [campaign, setCampaign] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadCampaign = async () => {
            try {
                const data = await fetchCampaignById(id);
                setCampaign(data);
            } catch (error) {
                console.error(error);
            } finally {
                setLoading(false);
            }
        };
        loadCampaign();
    }, [id]);

    if (loading) {
        return (
            <div className="d-flex justify-content-center align-items-center vh-100">
                <div
                    className="spinner-border text-warning"
                    style={{ width: "4rem", height: "4rem" }}
                ></div>
            </div>
        );
    }

    if (!campaign) {
        return (
            <div className="text-center py-5">
                <h2 className="mb-4">الحملة غير موجودة</h2>
                <Link to="/" className="btn btn-warning btn-lg">
                    العودة للرئيسية
                </Link>
            </div>
        );
    }

    return (
        <>
            {/* الصورة الكبيرة */}
            <div className="position-relative">
                <img
                    src={
                        campaign.imageUrl ||
                        "https://via.placeholder.com/1920x1080"
                    }
                    className="w-100"
                    style={{ height: "90vh", objectFit: "cover" }}
                    alt={campaign.title}
                />
                <div
                    className="position-absolute top-0 start-0 end-0 bottom-0"
                    style={{
                        background:
                            "linear-gradient(to top, rgba(0,0,0,0.8), rgba(0,0,0,0.2))",
                    }}
                ></div>
                <Link
                    to="/campaign"
                    className="position-absolute top-0 end-0 btn btn-light btn-lg m-4 shadow-lg"
                >
                    الرجوع للحملات
                </Link>
            </div>

            {/* الكارت الأبيض */}
            <div
                className="container-fluid px-4 px-md-5"
                style={{
                    marginTop: "-200px",
                    position: "relative",
                    zIndex: 10,
                }}
            >
                <div className="row justify-content-center">
                    <div className="col-12 col-lg-10 col-xl-8">
                        <div className="card shadow-lg border-0 rounded-4 overflow-hidden">
                            <div className="card-body p-5 p-md-7 text-center">
                                <h1 className="display-4 fw-bold mb-4 text-dark">
                                    {campaign.title}
                                </h1>
                                {campaign.charityName && (
                                    <p className="fs-4 text-muted mb-5">
                                        جمعية {campaign.charityName}
                                    </p>
                                )}
                                <p className="lead fs-3 text-muted mb-5 px-3">
                                    {campaign.description}
                                </p>

                                {/* البطاقات الثلاثة */}
                                <div className="row g-4 mb-5">
                                    <div className="col-md-4">
                                        <div
                                            className="text-white rounded-4 p-5 shadow-lg text-center h-100 d-flex flex-column justify-content-center"
                                            style={{
                                                backgroundColor:
                                                    "rgba(10, 130, 200, 0.9)",
                                            }}
                                        >
                                            <h2 className="display-4 fw-bold mb-3">
                                                {campaign.daysLeft}
                                            </h2>
                                            <p className="fs-4 mb-0">
                                                يوم متبقي
                                            </p>
                                        </div>
                                    </div>

                                    <div className="col-md-4">
                                        <div className="bg-dark text-white rounded-4 p-5 shadow-lg text-center h-100 d-flex flex-column justify-content-center">
                                            <h2 className="display-4 fw-bold mb-3">
                                                {campaign.goalAmount.toLocaleString(
                                                    "ar-EG"
                                                )}{" "}
                                                ج.م
                                            </h2>
                                            <p className="fs-4 mb-0">الهدف</p>
                                        </div>
                                    </div>

                                    <div className="col-md-4">
                                        <div className="bg-warning text-dark rounded-4 p-5 shadow-lg text-center h-100 d-flex flex-column justify-content-center">
                                            <h2 className="display-4 fw-bold mb-3">
                                                {campaign.raisedAmount.toLocaleString(
                                                    "ar-EG"
                                                )}{" "}
                                                ج.م
                                            </h2>
                                            <p className="fs-4 mb-0">المتبقي</p>
                                        </div>
                                    </div>
                                </div>

                                {/* شريط التقدم */}
                                <div className="mb-5">
                                    <div className="d-flex justify-content-between align-items-center mb-3">
                                        <h3
                                            style={{
                                                color: "rgba(10, 130, 200, 0.9)",
                                            }}
                                        >
                                            {Math.round(campaign.progress)}%
                                        </h3>
                                        <span
                                            className="fs-5"
                                            style={{
                                                color: "rgba(10, 130, 200, 0.9)",
                                            }}
                                        >
                                            التقدم
                                        </span>
                                    </div>

                                    <div
                                        className="progress rounded-pill"
                                        style={{
                                            height: "50px",
                                            backgroundColor: "#e9ecef",
                                        }}
                                    >
                                        <div
                                            className="progress-bar progress-bar-striped progress-bar-animated d-flex align-items-center justify-content-center"
                                            style={{
                                                width: `${campaign.progress}%`,
                                                backgroundColor:
                                                    "rgba(10, 130, 200, 0.9)",
                                                backgroundImage:
                                                    "linear-gradient(45deg, rgba(255,255,255,.15) 25%, transparent 25%, transparent 50%, rgba(255,255,255,.15) 50%, rgba(255,255,255,.15) 75%, transparent 75%, transparent)",
                                                backgroundSize: "1rem 1rem",
                                            }}
                                        >
                                            <span className="fs-3 fw-bold text-white shadow-sm">
                                                {Math.round(campaign.progress)}%
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <button className="btn btn-warning btn-lg w-100 py-5 rounded-4 shadow-lg fs-2 fw-bold text-dark">
                                    تبرع الآن لدعم هذه الحملة
                                    <i className="fa-solid fa-heart fa-beat ms-3"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}
