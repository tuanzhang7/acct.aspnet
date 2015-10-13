namespace acct.common.POCO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    [Serializable]
    public class Expense
    {
        public Expense()
        {
        }
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }
        [DisplayName("Description")]
        public string Remark { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ExpenseCategory ExpenseCategory { get; set; }
    }
}
