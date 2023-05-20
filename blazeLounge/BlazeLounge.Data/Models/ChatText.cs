using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Data.Models
{
    public class ChatText
    {
        public string Message { get; set; }
        public string Color { get; set; }

        public ChatText(string message, string color)
        {
            Message = message;
            Color = color;
        }

        public ChatText()
        {
        }
    }
}
