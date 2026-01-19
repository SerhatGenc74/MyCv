using System;
using System.Collections.Generic;

namespace MyCv.Models;

public partial class Project
{
    public int Id { get; set; }

    public string ProjectId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? CoverImgUrl { get; set; }

    public string? Tags { get; set; }

    public bool DeleteId { get; set; }
}
