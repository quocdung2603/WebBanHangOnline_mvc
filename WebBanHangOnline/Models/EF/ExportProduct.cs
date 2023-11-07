using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_ExportProduct")]
    public class ExportProduct : CommonAbstract
    {
        public ExportProduct()
        {
            this.ExportProductDetails = new HashSet<ExportProductDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Note { get; set; }    
        public virtual ICollection<ExportProductDetail> ExportProductDetails { get; set; }
    }
}