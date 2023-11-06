using Microsoft.AspNetCore.Identity;
using ToDo.Domain.Entity.Interfaces;

namespace ToDo.Domain.Entity.Auth;

public class Role : IdentityRole<Guid>, IBaseEntity
{

}
