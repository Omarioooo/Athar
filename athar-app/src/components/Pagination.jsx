export default function Pagination({ page, totalPages, onPageChange }) {
    return (
        <div className="pagination">
            <button
                disabled={page === 1}
                onClick={() => onPageChange(page - 1)}
            >
                السابق
            </button>

            <span>
                صفحة {page} من {totalPages}
            </span>

            <button
                disabled={page === totalPages}
                onClick={() => onPageChange(page + 1)}
            >
                التالي
            </button>
        </div>
    );
}
