﻿using Microsoft.AspNetCore.Identity;


namespace InternetBanking.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cedula { get; set; }


    }
}
