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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Serializable]
    public partial class Options
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100), Required]
        [Index(IsUnique = true)] 
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        
    }
}
