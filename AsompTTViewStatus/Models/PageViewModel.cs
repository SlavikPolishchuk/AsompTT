namespace AsompTTViewStatus.Models
{
    public class PageViewModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public bool HasPageNext { get { return Page < TotalPages; } }
        public bool HasPrevPage { get { return Page > 1; } }
        
        public PageViewModel(int page, int count, int pageSize=20)
        {
            Page = page;
            TotalPages =  (int)Math.Ceiling(count/(double)pageSize);
        }


    }
}
