using System.ComponentModel.DataAnnotations;

namespace JWT.Model
{
    public class JsonDto
    {
        [Required]
        public string token { get; set; }
    }
}
