using Homework_April_19.Data;

namespace Homework_April_19.Web.Models
{
    public class ListingViewModel
    {
        public List<Listing> listings { get; set; }
        public bool CanDelete { get; set; }
    }
}
