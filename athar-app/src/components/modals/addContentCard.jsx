// ContentModal.jsx
import React, { useState, useEffect } from "react";

export default function ContentModal({ isOpen, onClose, onSubmit, initialData = {} }) {
  const [formData, setFormData] = useState({
    title: initialData.title || "",
    description: initialData.description || "",
    postImage: null,
  });

  const [imagePreview, setImagePreview] = useState(initialData.imageUrl || null);
  const [submitting, setSubmitting] = useState(false);

  // تحديث البيانات عند فتح المودال
  useEffect(() => {
    if (isOpen) {
      setFormData({
        title: initialData.title || "",
        description: initialData.description || "",
        postImage: null,
      });
      setImagePreview(initialData.imageUrl || null);
    }
  }, [isOpen, initialData]);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setFormData({ ...formData, postImage: file });
      setImagePreview(URL.createObjectURL(file));
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (submitting) return;

    setSubmitting(true);

    try {
      const data = new FormData();
      data.append("Title", formData.title);
      data.append("Description", formData.description);
      if (formData.postImage) data.append("PostImage", formData.postImage);

      await onSubmit(data); // الانتظار لنجاح العملية قبل الإغلاق
      onClose();
    } catch (err) {
      console.error(err);
      alert("حدث خطأ أثناء حفظ المحتوى");
    } finally {
      setSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <>
      <div
        style={{
          position: "fixed",
          inset: 0,
          backgroundColor: "rgba(0, 0, 0, 0.65)",
          backdropFilter: "blur(8px)",
          WebkitBackdropFilter: "blur(8px)",
          zIndex: 999,
        }}
        onClick={onClose}
      />

      <div
        dir="rtl"
        style={{
          position: "fixed",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          backgroundColor: "#ffffff",
          borderRadius: "20px",
          boxShadow: "0 25px 50px rgba(0, 0, 0, 0.2)",
          width: "90%",
          maxWidth: "480px",
          maxHeight: "90vh",
          overflowY: "auto",
          zIndex: 1000,
          fontFamily: "'Tajawal', 'Cairo', sans-serif",
        }}
      >
        <div style={{ padding: "32px 28px" }}>
          <h3 style={{ margin: "0 0 28px", fontSize: "26px", fontWeight: "700", color: "#1a1a1a", textAlign: "center" }}>
            {initialData.id ? "تعديل المحتوى" : "إضافة محتوى جديد"}
          </h3>

          <form onSubmit={handleSubmit}>
            <div style={{ marginBottom: "22px" }}>
              <label style={{ display: "block", marginBottom: "8px", fontWeight: "600", color: "#2d2d2d" }}>
                العنوان <span style={{ color: "#e74c3c" }}>*</span>
              </label>
              <input
                type="text"
                name="title"
                value={formData.title}
                onChange={handleChange}
                required
                placeholder="عنوان جذاب للمحتوى..."
                style={{
                  width: "100%",
                  padding: "14px 16px",
                  borderRadius: "12px",
                  border: "1px solid #ddd",
                  fontSize: "16px",
                  transition: "all 0.3s ease",
                }}
                onFocus={(e) => {
                  e.target.style.borderColor = "#4eb6e6";
                  e.target.style.boxShadow = "0 0 0 3px rgba(78, 182, 230, 0.2)";
                }}
                onBlur={(e) => {
                  e.target.style.borderColor = "#ddd";
                  e.target.style.boxShadow = "none";
                }}
              />
            </div>

            <div style={{ marginBottom: "26px" }}>
              <label style={{ display: "block", marginBottom: "8px", fontWeight: "600", color: "#2d2d2d" }}>
                الوصف <span style={{ color: "#e74c3c" }}>*</span>
              </label>
              <textarea
                name="description"
                value={formData.description}
                onChange={handleChange}
                required
                placeholder="اكتب وصفًا واضحًا ومؤثرًا..."
                rows="5"
                style={{
                  width: "100%",
                  padding: "14px 16px",
                  borderRadius: "12px",
                  border: "1px solid #ddd",
                  fontSize: "16px",
                  resize: "vertical",
                }}
                onFocus={(e) => {
                  e.target.style.borderColor = "#4eb6e6";
                  e.target.style.boxShadow = "0 0 0 3px rgba(78, 182, 230, 0.2)";
                }}
                onBlur={(e) => {
                  e.target.style.borderColor = "#ddd";
                  e.target.style.boxShadow = "none";
                }}
              />
            </div>

            <div style={{ marginBottom: "30px" }}>
              <label style={{ display: "block", marginBottom: "10px", fontWeight: "600", color: "#2d2d2d" }}>
                صورة المحتوى
              </label>

              <label
                htmlFor="image-upload"
                style={{
                  display: "block",
                  border: "3px dashed #bbb",
                  borderRadius: "16px",
                  padding: "30px 20px",
                  textAlign: "center",
                  backgroundColor: "#fafafa",
                  cursor: "pointer",
                  transition: "all 0.3s ease",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.borderColor = "#4eb6e6";
                  e.currentTarget.style.backgroundColor = "#f0faff";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.borderColor = "#bbb";
                  e.currentTarget.style.backgroundColor = "#fafafa";
                }}
              >
                <input
                  type="file"
                  id="image-upload"
                  accept="image/*"
                  onChange={handleImageChange}
                  style={{ display: "none" }}
                />

                {imagePreview ? (
                  <img
                    src={imagePreview}
                    alt="معاينة"
                    style={{
                      maxWidth: "100%",
                      maxHeight: "220px",
                      borderRadius: "12px",
                      boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
                    }}
                  />
                ) : (
                  <>
                    <div style={{ fontSize: "48px", color: "#999", marginBottom: "12px" }}>Upload</div>
                    <p style={{ margin: 0, color: "#666", fontSize: "15px" }}>
                      اسحب الصورة هنا أو اضغط للرفع
                    </p>
                  </>
                )}
              </label>
            </div>

            <div style={{ display: "flex", gap: "14px" }}>
              <button
                type="submit"
                style={{
                  flex: 1,
                  padding: "16px",
                  background: "linear-gradient(135deg, #4eb6e6, #2e8bc0)",
                  color: "white",
                  border: "none",
                  borderRadius: "14px",
                  fontSize: "17px",
                  fontWeight: "bold",
                  cursor: "pointer",
                  boxShadow: "0 6px 15px rgba(78, 182, 230, 0.3)",
                }}
              >
                {submitting ? "جاري الحفظ..." : initialData.id ? "حفظ التعديلات" : "إضافة المحتوى"}
              </button>

              <button
                type="button"
                onClick={onClose}
                style={{
                  flex: 1,
                  padding: "16px",
                  backgroundColor: "#f5f5f5",
                  color: "#444",
                  border: "none",
                  borderRadius: "14px",
                  fontSize: "17px",
                  fontWeight: "bold",
                  cursor: "pointer",
                }}
              >
                إلغاء
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
