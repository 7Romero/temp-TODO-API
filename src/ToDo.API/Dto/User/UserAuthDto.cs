using System.ComponentModel.DataAnnotations;

namespace ToDo.API.Dto.User;

public class UserAuthDto
{
    [Required(ErrorMessage = "The email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Username { get; set; }

    [Required(ErrorMessage = "The password is required")]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }
}
