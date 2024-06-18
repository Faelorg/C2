using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Diploma
{
    public int Id { get; set; }

    public string TopicId { get; set; } = null!;

    public DateTime Date { get; set; }

    public int? Estimation { get; set; }
}
