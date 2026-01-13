using System;
using System.Collections.Generic;

namespace AuthenticationModule.Models;

public partial class LoginHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime LoginAt { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public bool IsSuccess { get; set; }

    public virtual User User { get; set; } = null!;
}
