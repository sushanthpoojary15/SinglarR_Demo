using System.ComponentModel.DataAnnotations;

namespace SignalRWebApi.ChatModels
{
    public class UserDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
