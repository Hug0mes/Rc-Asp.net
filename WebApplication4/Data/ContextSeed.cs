﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Enums;

namespace UserManagement.MVC.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(WebApplication4.Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(WebApplication4.Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(WebApplication4.Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(WebApplication4.Enums.Roles.Basic.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.SuperAdmin.ToString());
                }

            }

            var AdminUser = new IdentityUser
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Admin.ToString());
                }

            }

            var BasicUser = new IdentityUser
            {
                UserName = "basic",
                Email = "basic@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, WebApplication4.Enums.Roles.Basic.ToString());
                }

            }

        }
    }
}
