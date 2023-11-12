using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Message")]
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("RoomChat")]
        public int RoomId { get; set; } 
        public DateTime TimesChat { get; set; }  
        public string  Content { get; set; }
        public virtual RoomChat RoomChat { get; set; }
    }
}