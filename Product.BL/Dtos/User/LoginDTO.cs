using System.ComponentModel.DataAnnotations;

namespace Product.BL;
public class LoginDTO
{
    [Required]
    public string emailAddress { get; set; } = "";
    [Required]
    public string password { get; set; } = "";

}
