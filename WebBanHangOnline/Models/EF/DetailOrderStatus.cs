using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_DetailOrderStatus")]
    public class DetailOrderStatus
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public int OrderId { get; set; }    
        public string IdUConfirm { get; set; }
        public DateTime CofirmDate { get; set; }
        public string IdUExport { get; set; }
        public DateTime ExportDate { get; set; }
        public string IdUDelivery { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string IdUCancel { get; set; }   
        public DateTime CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string IdUReturn { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReturnReason { get; set; }    
        public virtual Order Order { get; set; }
    }
}