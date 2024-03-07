using System.ComponentModel.DataAnnotations;

namespace SignalRWebApi.ChatModels
{
    public class MessageDTO
    {
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        [Required]
        public string content { get; set; }
    }
}
