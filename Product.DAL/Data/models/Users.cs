using Microsoft.AspNetCore.Identity;

namespace Product.DAL;
public class Users : IdentityUser
{
    public string firstName { get; set; } = "";
    public string lastName { get; set; } = "";
    public string emailAddress { get; set; } = "";


}
