using System;
using System.Collections.Generic;

namespace MyCv.Models;

public partial class AdminUser
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public byte[] Password { get; set; } = null!;
}
