//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace acct.common.POCO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Serializable]
    [Table("Invoice")]
    public partial class Invoice:Order
    {
        public Invoice()
        {
            this.PaymentDetails = new HashSet<PaymentDetail>();
        }
        //[DisplayName("Invoice Number")]
        //[Required(ErrorMessage = "Required")]
        //[MaxLength(12)]
        //[Index(IsUnique = true)]
        //public string InvoiceNumber { get; set; }
        
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}