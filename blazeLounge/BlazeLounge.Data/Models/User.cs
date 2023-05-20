using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string PasswordHashed { get; set; } = null!;

    public string Uuid { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Admin { get; set; } = false;

    public long IdUser { get; set; }

    public long FkProfileidProfile { get; set; }

}
