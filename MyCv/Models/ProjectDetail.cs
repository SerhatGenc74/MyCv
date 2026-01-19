using System;
using System.Collections.Generic;

namespace MyCv.Models;

public partial class ProjectDetail
{
    public int Id { get; set; }

    public string ProjectId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? VisibleContent { get; set; }

    public string? Content { get; set; }

    public string? SubContent { get; set; }

    public bool DeleteId { get; set; }

    public virtual Project Project { get; set; } = null!;
}
