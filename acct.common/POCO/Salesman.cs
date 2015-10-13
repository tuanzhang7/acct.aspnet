namespace acct.common.POCO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    [Serializable]
    public class Salesman
    {
        public Salesman()
        {
        }
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }
        
        //public ICollection<Order> Order { get; set; }
    }
}
