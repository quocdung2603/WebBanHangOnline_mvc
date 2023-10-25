using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_ImportProductDetail")]
    public class ImportProductDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ImportProductId { get; set; }    
        public int ProductId { get; set; }
        public string Title { get; set; }  
        public int Quantity { get; set; }   
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }  
        public string Color { get; set; }   
        public string Size { get; set; }
        public virtual Product Product { get; set; }
        public virtual ImportProduct ImportProduct { get; set; }
    }
}