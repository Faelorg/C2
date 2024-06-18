using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class RatingUser
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? RatingId { get; set; }
}
