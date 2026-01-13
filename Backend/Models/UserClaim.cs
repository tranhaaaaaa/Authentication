using System;
using System.Collections.Generic;

namespace AuthenticationModule.Models;

public partial class UserClaim
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
