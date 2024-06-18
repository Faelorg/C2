using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Rating
{
    public int Id { get; set; }

    public string TaskId { get; set; } = null!;

    public string? Estimation { get; set; }
}
