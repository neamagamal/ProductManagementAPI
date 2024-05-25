using System.ComponentModel.DataAnnotations;

namespace Product.BL;
public class RegisterDto
{
    [Required]
    public string fistName { get; set; } = "";
    [Required]
    public string lastName { get; set; } = "";
    [Required]
    public string password { get; set; } = "";
    [Required]
    public string emailAddress { get; set; } = "";
}
