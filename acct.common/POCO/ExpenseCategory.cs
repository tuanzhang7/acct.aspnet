using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace acct.common.POCO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    public partial class ExpenseCategory
    {
        public ExpenseCategory()
        {
            this.Expense = new HashSet<Expense>();
        }

        public int Id { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Category { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
    }
}
