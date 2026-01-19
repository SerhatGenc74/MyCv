using System;
using System.Collections.Generic;

namespace MyCv.Models;

public partial class Content
{
    public int Id { get; set; }

    public string ContentId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Tags { get; set; }

    public string? VisibleContent { get; set; }

    public string? Content1 { get; set; }

    public string? SubContent { get; set; }

    public bool DeleteId { get; set; }
}
