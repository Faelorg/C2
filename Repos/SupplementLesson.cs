using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class SupplementLesson
{
    public string Id { get; set; } = null!;

    public string? Tittle { get; set; }

    public string? File { get; set; }

    public string? LessonId { get; set; }
}
