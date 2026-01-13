using System;
using System.Collections.Generic;

namespace AuthenticationModule.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsLocked { get; set; }

    public bool EmailConfirmed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
