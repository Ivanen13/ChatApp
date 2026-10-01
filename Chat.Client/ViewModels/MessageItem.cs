using Chat.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.ViewModels
{
    public class MessageItem(MessageDto dto, bool isMine)
    {
        public int Id => dto.Id;
        public string Username => dto.Username;
        public string Content => dto.Content;
        public DateTime LocalTime => dto.SentAt.ToLocalTime();
        public bool IsMine => isMine;
    }
}
