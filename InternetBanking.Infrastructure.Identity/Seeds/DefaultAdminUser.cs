﻿using InternetBanking.Core.Application.Enums;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            try
            {
                ApplicationUser defaultUser = new();
                defaultUser.UserName = "Adminuser";
                defaultUser.Email = "Adminuser@email.com";
                defaultUser.FirstName = "Admin John";
                defaultUser.LastName = "Admin Doe";
                defaultUser.PhoneNumber = "8095553256";
                defaultUser.EmailConfirmed = true;
                defaultUser.PhoneNumberConfirmed = true;

                if (userManager.Users.All(u => u.Id != defaultUser.Id))
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                        await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    }
                }
            }

            catch (Exception ex)
            {


            }
        }
    }
}
