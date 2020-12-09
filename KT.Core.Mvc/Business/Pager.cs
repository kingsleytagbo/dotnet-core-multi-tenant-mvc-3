using System;
namespace KT.Core.Mvc.Business
{
    public class Pager
    {
        public Pager(int totalPages, int? page, int pageSize = 10, string search = null)
        {
            // calculate total, start and end pages
            //var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int totalItems = totalPages * PageSize;
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            SearchString = search;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public string SearchString { get; private set; }
    }
}