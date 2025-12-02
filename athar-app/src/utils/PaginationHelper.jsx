export function paginate(data, page, itemsPerPage) {
    const lastIndex = page * itemsPerPage;
    const firstIndex = lastIndex - itemsPerPage;

    return data.slice(firstIndex, lastIndex);
}

export function getTotalPages(dataLength, itemsPerPage) {
    return Math.ceil(dataLength / itemsPerPage);
}
