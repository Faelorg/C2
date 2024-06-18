using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Topic
{
    public string Id { get; set; } = null!;

    public string? Tittle { get; set; }

    public string? Description { get; set; }
}
