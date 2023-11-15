using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Combo")]
    public class Combo : CommonAbstract
    {
        public Combo()
        {
            this.ComboDetails = new HashSet<ComboDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal PercentDec { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<ComboDetail> ComboDetails { get; set; }
    }
}