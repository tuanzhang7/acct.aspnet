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
    using acct.common.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    public abstract class Order : ITrackableEntity
    {
        //public enum TypeOptions
        //{
        //    Invoice = 1,
        //    Quotation = 2
        //}

        public enum StatusOptions
        {
            Unpaid= 1,
            Partial = 2,
            Paid = 3,
            Overdue=4
        }

        public Order()
        {
            this.OrderDetail = new HashSet<OrderDetail>();
        }

        [DisplayName("Order Number")]
        [Required(ErrorMessage = "Required")]
        [Index(IsUnique = true)]
        [MaxLength(12)]
        public string OrderNumber { get; set; }

        [DisplayName("Customer")]
        [Required(ErrorMessage = "Required")]
        public int CustomerId { get; set; }

        [DisplayName("Salesman")]
        public int? SalesmanId { get; set; }

        [DisplayName("Order Date")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        public System.DateTime OrderDate { get; set; }

        public int Id { get; set; }
        public int OrderType { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [Required]
        [DisplayName("GST")]
        [Range(0, 100, ErrorMessage = "GST Rate must be between 0 and 100")]
        public decimal GSTRate { get; set; }
 
        public int? Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Total Amount")]
        [DefaultValue(0)]
        public decimal TotalAmount { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Total")]
        [DefaultValue(0)]
        public decimal TotalWithTax
        {
            get { return Math.Round(TotalAmount * (1 + GSTRate / 100), 2); }
            private set { /* needed for EF */ }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Amount Paid")]
        [DefaultValue(0)]
        public decimal AmountPaid { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Outstanding Amount")]
        [DefaultValue(0)]
        public decimal AmountOutstanding { get; private set; }
        
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Salesman Salesman { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}