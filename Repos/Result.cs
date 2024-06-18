using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Result
{
    public int Id { get; set; }

    public int Estimation { get; set; }

    public string? Recommendations { get; set; }

    public string? TopicId { get; set; }

    public string? UserId { get; set; }
}
