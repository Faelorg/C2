using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class UserTask
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? TaskId { get; set; }

    public string? Estimation { get; set; }

    public int? Status { get; set; }

    public string? Description { get; set; }
}
