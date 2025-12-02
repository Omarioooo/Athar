export default function AddCampaignModal() {
    return (
        <form className="add-campaign-form">
            <h4>إنشاء حملة جديدة</h4>

            {/* Title */}
            <div className="input-group">
                <input type="text" required />
                <label className="input-title">عنوان الحملة</label>
            </div>

            {/* Description */}
            <div className="input-group textarea-group">
                <textarea required></textarea>
                <label className="input-title">وصف الحملة</label>
            </div>

            {/* Image Upload */}
            <div className="file-group">
                <label className="file-label">
                    اختر صورة الحملة
                    <input type="file" accept="image/*" />
                </label>
            </div>

            {/* Duration */}
            <div className="input-group">
                <input type="number" required />
                <label className="input-title">مدة الحملة بالأيام</label>
            </div>

            {/* Category */}
            <div className="input-group select-group">
                <select required>
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

            {/* Goal Amount */}
            <div className="input-group">
                <input type="number" required />
                <label className="input-title">المبلغ المستهدف</label>
            </div>

            {/* Checkbox group */}
            <div className="checkbox-wrapper">
                <label className="checkbox-card">
                    <input type="checkbox" />
                    <span className="checkmark"></span>
                    <span className="text">حملة عاجلة</span>
                </label>

                <label className="checkbox-card">
                    <input type="checkbox" />
                    <span className="checkmark"></span>
                    <span className="text">تبرعات عينية</span>
                </label>
            </div>

            {/* Actions */}
            <div className="form-actions">
                <button type="submit" className="btn btn-warning">
                    إنشاء الحملة
                </button>

                <button type="button" className="btn btn-secondary">
                    إغلاق
                </button>
            </div>
        </form>
    );
}
