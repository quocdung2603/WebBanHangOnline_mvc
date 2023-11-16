using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_TimePromotionDetail")]
    public class TimePromotionDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TimePromotion")]
        public int TimePromotionId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual TimePromotion TimePromotion { get; set; }
        public virtual Product Product { get; set; }
    }
}