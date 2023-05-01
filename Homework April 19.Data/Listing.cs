using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_April_19.Data
{
    public class Listing
    {
        public int Id { get; set; }
        public string Name{ get;set;}
        public DateTime Date { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool CanDelete { get; set; }
    }
}
