using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_UserVoucher")]
    public class UserVoucher
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public int VoucherId { get; set; }  
        public string UserId { get; set; }
        public int Type { get; set; }
        public bool IsUse { get; set; }
        public virtual Voucher Voucher { get; set; }    
    }
}