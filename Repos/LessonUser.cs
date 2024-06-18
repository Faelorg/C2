using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class LessonUser
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? LessonId { get; set; }
}
