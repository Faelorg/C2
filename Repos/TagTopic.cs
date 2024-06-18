using System;
using System.Collections.Generic;

namespace Server.Repos;

public partial class TagTopic
{
    public string Id { get; set; } = null!;

    public string TopicId { get; set; } = null!;

    public string TagId { get; set; } = null!;
}
