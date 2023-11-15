using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_ComboDetail")]
    public class ComboDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Combo")]
        public int ComboId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Combo Combo { get; set; }
        public virtual Product Product { get; set; }
    }
}