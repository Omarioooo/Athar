import { useParams, Link, NavLink } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchCampaignById } from "../../services/campaignService";
import Pagination from "../../components/Pagination";
import {
    getMediaOfCampaign,
    createContent,
    deletecontent,
    updatecontent,
} from "../../Repository/contentRepository";
import ContentCard from "../../components/content/contentcard";
import { UseAuth } from "../../Auth/Auth";
import ContentModal from "../../components/modals/addContentCard";

export default function CampaignDetail() {
    const { id } = useParams();
    const [campaign, setCampaign] = useState(null);
    const [loading, setLoading] = useState(true);
    const [cntloading, setcntLoading] = useState(true);
    const [content, setContents] = useState([]);
    const [contenterr, setContentErr] = useState("");
    const [totalPage, setTotalPage] = useState(1);
    const [currentPage, setCurrentPage] = useState(1);

    const [editingContent, setEditingContent] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const { user } = UseAuth();

    // -------------------- Edit Handler --------------------
    const handleEditContent = (cnt) => {
        setEditingContent(cnt); // بيانات العنصر اللى هيتعدل
        setIsModalOpen(true);
    };

    // -------------------- Add / Update Submit --------------------
    const handleSubmitContent = async (data) => {
        try {
            const formData = new FormData();
            for (const [key, value] of data.entries()) {
                formData.append(key, value);
            }

            formData.append("CampaignId", id);

            if (editingContent) {
                // ----- UPDATE -----
                await updatecontent(editingContent.id, formData);
            } else {
                // ----- CREATE -----
                await createContent(formData);
            }

            setIsModalOpen(false);
            setEditingContent(null);

            // Reload media
            loadMedia(id, currentPage);
        } catch (error) {
            console.error("خطأ:", error.response?.data || error.message);
        }
    };

    // -------------------- Load Media --------------------
    const loadMedia = async (id, page) => {
        setcntLoading(true);
        setContentErr("");
        try {
            const data = await getMediaOfCampaign(id, page, 2);
            setContents(data.contents || []);
            setTotalPage(data.totalPages || 1);
        } catch (error) {
            setContentErr("لا توجد ميديا لهذه الحملة");
            setContents([]);
        } finally {
            setcntLoading(false);
        }
    };

    // -------------------- Load Campaign --------------------
    useEffect(() => {
        const loadCampaign = async () => {
            setLoading(true);
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

    // -------------------- Load Media on Page Change --------------------
    useEffect(() => {
        loadMedia(id, currentPage);
    }, [id, currentPage]);

    // -------------------- Loading States --------------------
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
                <NavLink to="/campaigns" className="btn btn-warning btn-lg">
                    العودة للرئيسية
                </NavLink>
            </div>
        );
    }

    return (
        <>
            {/* الصورة الكبيرة */}
            <div className="position-relative img-compaigndetails">
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
                    to="/campaigns"
                    className="position-absolute top-0 end-0 btn btn-light btn-lg m-4 shadow-lg"
                >
                    الرجوع للحملات
                </Link>
            </div>

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

                                {/* الميديا */}
                                <h2 className="mt-5">
                                    الميديا الخاصة لهذه الحملة
                                </h2>

                                {cntloading ? (
                                    <div className="d-flex justify-content-center py-5">
                                        <div
                                            className="spinner-border text-warning"
                                            style={{
                                                width: "4rem",
                                                height: "4rem",
                                            }}
                                        ></div>
                                    </div>
                                ) : contenterr ? (
                                    <div className="text-center py-5 text-danger">
                                        <h4>{contenterr}</h4>
                                    </div>
                                ) : content.length === 0 ? (
                                    <div className="text-center py-5">
                                        <h4>لا توجد ميديا لعرضها</h4>
                                    </div>
                                ) : (
                                    <>
                                        <div className="row">
                                            {content.map((cnt) => {
                                                const date = new Date(
                                                    cnt.createdAt
                                                );
                                                const arabicDate =
                                                    date.toLocaleDateString(
                                                        "ar-EG",
                                                        {
                                                            weekday: "long",
                                                            year: "numeric",
                                                            month: "long",
                                                            day: "numeric",
                                                        }
                                                    );

                                                return (
                                                    <div
                                                        className="col-xl-6 col-lg-6 col-md-12 col-sm-12 mb"
                                                        key={cnt.id}
                                                    >
                                                        <ContentCard
                                                            cnt={cnt}
                                                            arabicDate={
                                                                arabicDate
                                                            }
                                                            showCampaignInfo={
                                                                false
                                                            }
                                                            user={user}
                                                            onEdit={
                                                                handleEditContent
                                                            } // ⭐ مهم
                                                        />
                                                    </div>
                                                );
                                            })}
                                        </div>

                                        <Pagination
                                            page={currentPage}
                                            totalPages={totalPage}
                                            onPageChange={setCurrentPage}
                                        />
                                    </>
                                )}

                                {/* زر الإضافة */}
                                {user && user.role === "CharityAdmin" && (
                                    <button
                                        onClick={() => {
                                            setEditingContent(null); // إضافة جديدة
                                            setIsModalOpen(true);
                                        }}
                                        style={{
                                            padding: "10px 20px",
                                            backgroundColor: "#4eb6e6",
                                            color: "#fff",
                                            border: "none",
                                            borderRadius: "5px",
                                        }}
                                    >
                                        +إضافة ميديا
                                    </button>
                                )}

                                {/* المودال */}
                                {isModalOpen && (
                                    <ContentModal
                                        isOpen={isModalOpen}
                                        onClose={() => {
                                            setIsModalOpen(false);
                                            setEditingContent(null);
                                        }}
                                        onSubmit={handleSubmitContent}
                                        initialData={editingContent || {}}
                                    />
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}
