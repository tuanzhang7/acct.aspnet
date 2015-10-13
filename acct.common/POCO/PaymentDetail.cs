using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace acct.common.POCO
{
    [Serializable]
    public partial class PaymentDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int InvoiceId { get; set; }

        public string Description { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual Invoice Invoice { get; set; }
        
    }
}
