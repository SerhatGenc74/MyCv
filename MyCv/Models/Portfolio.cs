using System;
using System.Collections.Generic;

namespace MyCv.Models;

public partial class Portfolio
{
    public int Id { get; set; }

    public string? Tag { get; set; }

    public string? Title { get; set; }

    public string? CoverImgUrl { get; set; }

    public bool DeleteId { get; set; }
}
