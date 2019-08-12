using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sublit2
{
    class Order
    {
        public bool Checked { get; set; }
        public int Id { get; set; }
        public string Reference { get; set; }
        public string CustomerData { get; set; }
        public string Email { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
