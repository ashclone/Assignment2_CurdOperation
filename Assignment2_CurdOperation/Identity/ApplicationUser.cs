﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2_CurdOperation.Identity

{

    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }

}
