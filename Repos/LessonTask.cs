using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class LessonTask
{
    public int IdlessonId { get; set; }

    public string? LessonId { get; set; }

    public string? TaskId { get; set; }
}
