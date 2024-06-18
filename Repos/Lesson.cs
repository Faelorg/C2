using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class Lesson
{
    public string Id { get; set; } = null!;

    public string? Tittle { get; set; }

    public string? Content { get; set; }

    public string? Autor { get; set; }

    public string? TopicId { get; set; }
}
