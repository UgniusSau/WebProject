﻿namespace Blazelounge_v2.Data.Models
{
    public class ChangePasswordModel
    {
        public string? Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
