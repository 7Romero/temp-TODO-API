namespace ToDo.API.Dto.User;

public class UserGetMeDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public IList<string>? Roles { get; set; }
}
