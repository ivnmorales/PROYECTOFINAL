﻿using Microsoft.AspNetCore.Identity;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Pomodoro.Shared.Dtos;

namespace Pomodoro.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<User> _signInManager;



        public UserHelper(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager
            , SignInManager<User> signInManager)

        {

            _context = context;

            _userManager = userManager;

            _roleManager = roleManager;

            _signInManager = signInManager;

        }



        public async Task<IdentityResult> AddUserAsync(User user, string password)

        {

            return await _userManager.CreateAsync(user, password);

        }



        public async Task AddUserToRoleAsync(User user, string roleName)

        {

            await _userManager.AddToRoleAsync(user, roleName);

        }



        public async Task CheckRoleAsync(string roleName)

        {

            bool roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)

            {

                await _roleManager.CreateAsync(new IdentityRole

                {

                    Name = roleName

                });

            }

        }



        public async Task<User> GetUserAsync(string email)

        {

            return await _context.Users

                .FirstOrDefaultAsync(x => x.Email == email);

        }



        public async Task<bool> IsUserInRoleAsync(User user, string roleName)

        {

            return await _userManager.IsInRoleAsync(user, roleName);

        }

        // Implementación de LoginAsync
        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        }

        // Implementación de LogoutAsync
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }

} 
       
    

