import { useState } from "react";
import { FaSearch } from "react-icons/fa";

export default function SearchBar({ title, onSearch }) {
    const [query, setQuery] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        onSearch(query.trim());
    };

    const handleChange = (e) => {
        const value = e.target.value;
        setQuery(value);

        onSearch(value.trim());
    };
    return (
        <>
            <div className="search-title">{title}</div>

            <div className="search-container">
                <form className="search-form">
                    <label className="label-for-search">
                        <button
                            className="search-btn"
                            type="submit"
                            onClick={handleSubmit}
                        >
                            <FaSearch className="search-icon" />
                        </button>
                    </label>
                    <input
                        id="search-input"
                        placeholder="بحث.."
                        value={query}
                        onChange={handleChange}
                        className="input"
                        name="text"
                        type="text"
                    />
                </form>
            </div>
        </>
    );
}
