using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class LessonTopic
{
    public string Id { get; set; } = null!;

    public string? LessonId { get; set; }

    public string? TopicId { get; set; }
}
