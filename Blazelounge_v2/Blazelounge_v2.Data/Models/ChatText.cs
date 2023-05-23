namespace Blazelounge_v2.Data.Models
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
