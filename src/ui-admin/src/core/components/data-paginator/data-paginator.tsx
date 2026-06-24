export default function DataPaginator({ currentPageNumber, totalCount, pageSize, onPageChanged }
    : Readonly<{
        currentPageNumber: number,
        totalCount: number,
        pageSize: number,
        onPageChanged: (currentPageNumber: number) => void
    }>) {

    const hasPreviousPage = currentPageNumber > 1;
    const hasNextPage = currentPageNumber < Math.ceil(totalCount / pageSize);


    function previousPage(): void {
        if (hasPreviousPage) onPageChanged(currentPageNumber - 1);
    }

    function nextPage(): void {
        if (hasNextPage) onPageChanged(currentPageNumber + 1);
    }

    function getPageNumbers(): number[] {
        const totalPages = Math.ceil(
            totalCount / pageSize,
        );
        const pages: number[] = [];
        const maxPagesToShow = 5;

        if (totalPages <= maxPagesToShow) {
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i);
            }
        } else {
            const startPage = Math.max(
                1,
                currentPageNumber - Math.floor(maxPagesToShow / 2),
            );
            const endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);

            if (startPage > 1) {
                pages.push(1);
                if (startPage > 2) {
                    pages.push(-1);
                }
            }

            for (let i = startPage; i <= endPage; i++) {
                pages.push(i);
            }

            if (endPage < totalPages) {
                if (endPage < totalPages - 1) {
                    pages.push(-1);
                }
                pages.push(totalPages);
            }
        }

        return pages;
    }

    return (
        <nav className="mt-4">
            <ul className="pagination justify-content-center">
                <li className={hasPreviousPage ? 'page-item' : 'page-item disabled'}>
                    <button className={hasPreviousPage ? 'page-link' : 'page-link disabled'} onClick={previousPage} >
                        <i className="bi bi-chevron-left"></i> Previous
                    </button>
                </li>


                {getPageNumbers().map(pageNum => {
                    return (
                        <li className={pageNum === currentPageNumber ? 'page-item active' : 'page-item'}>
                            {pageNum.toString() !== '...' && <button className="page-link" onClick={() => onPageChanged(pageNum)}>
                                {pageNum}
                            </button>}
                            {pageNum.toString() === '...' && <span className="page-link"  > {
                                pageNum
                            }</span>}
                        </li >
                    )
                })}

                <li className={hasNextPage ? 'page-item' : 'page-item disabled'}>
                    <button className={hasNextPage ? 'page-link' : 'page-link disabled'} onClick={nextPage} >
                        Next <i className="bi bi-chevron-right"></i>
                    </button>
                </li>
            </ul >

            <div className="text-center text-muted mt-3">
                <small>
                    Page {currentPageNumber} of {Math.ceil(totalCount / pageSize)} (Total:{totalCount} products)
                </small>
            </div>
        </nav >
    )
}
