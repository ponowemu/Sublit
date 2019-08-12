using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sublit2
{
    class Document
    {
        public bool Checked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int DocType { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public string Creator { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string Description { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }

    }
}
