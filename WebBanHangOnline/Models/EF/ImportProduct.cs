using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    public class ImportProduct : CommonAbstract
    {
        public ImportProduct()
        {
            this.ImportProductDetails = new HashSet<ImportProductDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public virtual ICollection<ImportProductDetail> ImportProductDetails { get; set; }
    }
}