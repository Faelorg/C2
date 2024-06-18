using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Tag
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
