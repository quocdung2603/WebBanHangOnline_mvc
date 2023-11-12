using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_RoomChat")]
    public class RoomChat
    {
        public RoomChat()
        {
            this.Messages = new HashSet<Message>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Type { get; set; }

        public virtual IEnumerable<Message> Messages { get; set; }
    }
}