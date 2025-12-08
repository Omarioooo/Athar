import { useEffect, useState } from "react";
import CampaignCard from "../../../components/charity/charity-campaigns/CampaignCard";
import Pagination from "../../../components/Pagination";
import { getTotalPages, paginate } from "../../../utils/PaginationHelper";
import { UseAuth } from "../../../Auth/Auth";
import { getCharityCampaigns } from "../../../services/charityService";
import { FaPlus } from "react-icons/fa";
import { CreateCampaign } from "../../../services/campaignService";
import { AnimatePresence, motion } from "framer-motion";

export default function MyCampaigns() {
    const { user } = UseAuth();

    const [campaigns, setCampaigns] = useState([]);
    const [loading, setLoading] = useState(true);
    const [open, setOpen] = useState(false);

    // Form states
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [duration, setDuration] = useState("");
    const [goalAmount, setGoalAmount] = useState("");
    const [category, setCategory] = useState("");
    const [imageFile, setImageFile] = useState(null);

    const [page, setPage] = useState(1);
    const [filter, setFilter] = useState("all");

    const mapStatusToFilter = (statusId) => {
        return statusId === 2 ? "completed" : "notCompleted";
    };

    async function loadCampaigns() {
        try {
            const data = await getCharityCampaigns(user.id);
            setCampaigns(data);
        } catch (err) {
            console.error("Failed to fetch campaigns:", err);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        if (user?.id) loadCampaigns();
    }, [user?.id]);

    const resetForm = () => {
        setTitle("");
        setDescription("");
        setDuration("");
        setGoalAmount("");
        setCategory("");
        setImageFile(null);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            setLoading(true);

            const formData = new FormData();
            formData.append("Title", title);
            formData.append("Description", description);
            formData.append("Duration", duration);
            formData.append("GoalAmount", goalAmount);
            formData.append("Category", category);
            formData.append("ImageFile", imageFile);

            await CreateCampaign(user.id, formData);

            await loadCampaigns();
            resetForm();
            setOpen(false);
        } catch (err) {
            console.error(err);
            alert("فشل إنشاء الحملة");
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="d-flex justify-content-center py-5">
                <div
                    className="spinner-border text-warning"
                    style={{ width: "4rem", height: "4rem" }}
                ></div>
            </div>
        );
    }

    const itemsPerPage = 6;
    const currentItems = paginate(campaigns, page, itemsPerPage);
    const filteredCampaigns =
        filter === "all"
            ? currentItems
            : currentItems.filter(
                  (c) => mapStatusToFilter(c.status) === filter
              );

    const totalPages = getTotalPages(campaigns.length, itemsPerPage);

    return (
        <>
            <div className="campaigns-wrapper">
                {/* Filter buttons */}
                <div className="campaigns-filter">
                    {["all", "completed", "notCompleted"].map((f) => (
                        <button
                            key={f}
                            className={`filter-btn ${
                                filter === f ? "active" : ""
                            }`}
                            onClick={() => setFilter(f)}
                        >
                            {f === "all" && "كل الحملات"}
                            {f === "completed" && "مكتملة"}
                            {f === "notCompleted" && "غير مكتملة"}
                        </button>
                    ))}
                </div>

                {filteredCampaigns.length > 0 ? (
                    <>
                        <div className="cards">
                            {filteredCampaigns.map((campaign) => (
                                <CampaignCard
                                    key={campaign.id}
                                    id={campaign.id}
                                    title={campaign.title}
                                    description={campaign.description}
                                    raisedAmount={campaign.raisedAmount}
                                    goalAmount={campaign.goalAmount}
                                    progress={campaign.progress}
                                    startDate={campaign.startDate}
                                    endDate={campaign.endDate}
                                    charityName={campaign.charity_name}
                                    imageUrl={campaign.imageUrl}
                                    status={campaign.statusText}
                                    statusArabic={campaign.statusArabic}
                                />
                            ))}
                        </div>

                        <Pagination
                            page={page}
                            totalPages={totalPages}
                            onPageChange={setPage}
                        />
                    </>
                ) : (
                    <div className="no-campaigns">
                        <h3>لا توجد حملات</h3>
                    </div>
                )}

                <div className="add-campaign-wrapper">
                    <button
                        className={`add-campaign-btn ${
                            open ? "active" : "not-active"
                        }`}
                        onClick={() => setOpen(true)}
                    >
                        <FaPlus />
                    </button>
                </div>

                {/* Modal */}
                <AnimatePresence>
                    {open && (
                        <motion.div
                            className="add-campaign-overlay"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            exit={{ opacity: 0 }}
                        >
                            <motion.div
                                className="add-campaign-modal"
                                initial={{ scale: 0.8, opacity: 0 }}
                                animate={{ scale: 1, opacity: 1 }}
                                exit={{ scale: 0.8, opacity: 0 }}
                                transition={{ duration: 0.2 }}
                            >
                                <form
                                    className="add-campaign-form"
                                    onSubmit={handleSubmit}
                                >
                                    <h4>إنشاء حملة جديدة</h4>

                                    <div className="input-group">
                                        <input
                                            type="text"
                                            required
                                            value={title}
                                            onChange={(e) =>
                                                setTitle(e.target.value)
                                            }
                                        />
                                        <label className="input-title">
                                            عنوان الحملة
                                        </label>
                                    </div>

                                    <div className="input-group textarea-group">
                                        <textarea
                                            required
                                            value={description}
                                            onChange={(e) =>
                                                setDescription(e.target.value)
                                            }
                                        ></textarea>
                                        <label className="input-title">
                                            وصف الحملة
                                        </label>
                                    </div>

                                    <div className="file-group">
                                        <label className="file-label">
                                            اختر صورة الحملة
                                            <input
                                                type="file"
                                                accept="image/*"
                                                onChange={(e) =>
                                                    setImageFile(
                                                        e.target.files[0]
                                                    )
                                                }
                                            />
                                        </label>
                                    </div>

                                    <div className="input-group">
                                        <input
                                            type="number"
                                            required
                                            value={duration}
                                            onChange={(e) =>
                                                setDuration(e.target.value)
                                            }
                                        />
                                        <label className="input-title">
                                            مدة الحملة بالأيام
                                        </label>
                                    </div>

                                    <div className="input-group select-group">
                                        <select
                                            required
                                            value={category}
                                            onChange={(e) =>
                                                setCategory(e.target.value)
                                            }
                                        >
                                            <option value="">اختر الفئة</option>
                                            <option value="0">تعليم</option>
                                            <option value="1">صحة</option>
                                            <option value="2">أيتام</option>
                                            <option value="3">غذاء</option>
                                            <option value="4">مأوى</option>
                                            <option value="99">أخرى</option>
                                        </select>
                                        <label className="input-title">
                                            الفئة
                                        </label>
                                    </div>

                                    <div className="input-group">
                                        <input
                                            type="number"
                                            required
                                            value={goalAmount}
                                            onChange={(e) =>
                                                setGoalAmount(e.target.value)
                                            }
                                        />
                                        <label className="input-title">
                                            المبلغ المستهدف
                                        </label>
                                    </div>

                                    <div className="form-actions">
                                        <button
                                            type="submit"
                                            className="btn btn-warning"
                                        >
                                            {loading
                                                ? "جاري الإنشاء..."
                                                : "إنشاء الحملة"}
                                        </button>

                                        <button
                                            type="button"
                                            className="btn btn-secondary"
                                            onClick={() => setOpen(false)}
                                        >
                                            إغلاق
                                        </button>
                                    </div>
                                </form>
                            </motion.div>
                        </motion.div>
                    )}
                </AnimatePresence>
            </div>
        </>
    );
}
