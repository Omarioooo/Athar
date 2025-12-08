import { useState } from "react";
import { CreateCampaign } from "../../services/campaignService";

export default function AddCampaignModal({ open, setOpen, id }) {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [duration, setDuration] = useState("");
    const [goalAmount, setGoalAmount] = useState("");
    const [category, setCategory] = useState("");
    const [imageFile, setImageFile] = useState(null);
    const [loading, setLoading] = useState(false);

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

            await CreateCampaign(id, formData);
        } catch (err) {
            console.error(err);
            alert("فشل إنشاء الحملة");
        } finally {
            setOpen(false);
            setLoading(false);
        }
    };

    return (
        <form className="add-campaign-form" onSubmit={handleSubmit}>
            <h4>إنشاء حملة جديدة</h4>
            <div className="input-group">
                <input
                    type="text"
                    required
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                />
                <label className="input-title">عنوان الحملة</label>
            </div>
            <div className="input-group textarea-group">
                <textarea
                    required
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                ></textarea>
                <label className="input-title">وصف الحملة</label>
            </div>
            <div className="file-group">
                <label className="file-label">
                    اختر صورة الحملة
                    <input
                        type="file"
                        accept="image/*"
                        onChange={(e) => setImageFile(e.target.files[0])}
                    />
                </label>
            </div>
            <div className="input-group">
                <input
                    type="number"
                    required
                    value={duration}
                    onChange={(e) => setDuration(e.target.value)}
                />
                <label className="input-title">مدة الحملة بالأيام</label>
            </div>
            <div className="input-group select-group">
                <select
                    required
                    value={category}
                    onChange={(e) => setCategory(e.target.value)}
                >
                    <option value="">اختر الفئة</option>
                    <option value="0">تعليم</option>
                    <option value="1">صحة</option>
                    <option value="2">أيتام</option>
                    <option value="3">غذاء</option>
                    <option value="4">مأوى</option>
                    <option value="99">أخرى</option>
                </select>
                <label className="input-title">الفئة</label>
            </div>
            <div className="input-group">
                <input
                    type="number"
                    required
                    value={goalAmount}
                    onChange={(e) => setGoalAmount(e.target.value)}
                />
                <label className="input-title">المبلغ المستهدف</label>
            </div>
            <div className="form-actions">
                <button type="submit" className="btn btn-warning">
                    {loading ? "جاري الإنشاء..." : "إنشاء الحملة"}
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
    );
}
