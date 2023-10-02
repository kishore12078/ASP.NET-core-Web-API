namespace CityInfoAPI.Models
{
    public class PagenationMetadata
    {
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public PagenationMetadata(int totalItemCount,int pageSize,int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize= pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}
