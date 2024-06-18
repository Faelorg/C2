using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Task
{
    public string Id { get; set; } = null!;

    public string? Content { get; set; }

    public string? Tittle { get; set; }

    public int? Difficalty { get; set; }

    public int? Time { get; set; }
}
