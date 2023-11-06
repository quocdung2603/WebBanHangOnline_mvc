using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Voucher")]
    public class Voucher : CommonAbstract
    {
        public Voucher()
        {
            this.UserVouchers = new HashSet<UserVoucher>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Title { get; set; }
        public int Type { get; set; }   
        public decimal Value { get; set; }  
        public decimal PercentValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }   
        public virtual IEnumerable<UserVoucher> UserVouchers { get; set; }
    }
}