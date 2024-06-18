using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string? FullName { get; set; }

    public string? Token { get; set; }

    public string? TopicId { get; set; }
}
