namespace FALOFinancialProofing.Utilities
{
    public static class PaginationHelper
    {
        // 71 phần từ, một trang 10 phần từ => 70/10+1 = 8 trang
        public static IQueryable<T> Paginate<T>(IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        public static IEnumerable<int> GetPaginationPageRange(int currentPage, int totalPages, int pageRange)
        {
            int pagesBeforeCurrentPage = pageRange / 2;
            int pagesAfterCurrentPage = pageRange - pagesBeforeCurrentPage - 1;
            int firstPage = Math.Max(1, currentPage - pagesBeforeCurrentPage);
            int endPage = Math.Min(totalPages, currentPage + pagesAfterCurrentPage);
            // calculate delta: nó là độ chênh lệch, đáng lẽ deltastart và end phải bằng không nhưng do dùng max với min
            int deltaStart = pagesBeforeCurrentPage - (currentPage - firstPage);
            int deltaEnd = pagesAfterCurrentPage - (endPage - currentPage);
            firstPage = Math.Max(1, firstPage - deltaEnd);
            endPage = Math.Min(totalPages, endPage + deltaStart);
            return Enumerable.Range(firstPage, endPage - firstPage + 1);
        }// comment và trường hợp khác
    }
}
