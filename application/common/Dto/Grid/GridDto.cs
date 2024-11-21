using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Dto.Grid
{
    public class PaginationInfo
    {
        // Current page number (1-based indexing)
        public int PageNumber { get; set; }

        // Number of items per page
        public int PageSize { get; set; }

        // Total number of items in the dataset
        public int Count { get; set; }

        // Total number of pages, calculated based on TotalItems and PageSize
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)Count / PageSize);
            }
        }

        // Boolean to check if there's a previous page
        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        // Boolean to check if there's a next page
        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }

}