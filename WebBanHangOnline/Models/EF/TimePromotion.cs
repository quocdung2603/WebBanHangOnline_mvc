using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_TimePromotion")]
    public class TimePromotion: CommonAbstract
    {
        public TimePromotion()
        {
            this.TimePromotionDetails = new HashSet<TimePromotionDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PercentValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsBan { get; set; }
        public virtual ICollection<TimePromotionDetail> TimePromotionDetails { get; set; }
    }
}