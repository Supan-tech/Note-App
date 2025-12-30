using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class QueryNoteDto
    {
        //searching
        public string? Search { get; set; }

        //Sorting
        public string? SortBy { get; set; }   
        public string? SortDir { get; set; }   

        //filter
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        //Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}