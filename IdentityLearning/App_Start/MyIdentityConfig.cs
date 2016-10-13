using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using IdentityLearning.Models;
using IdentityLearning.Services;

namespace IdentitySample.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class MyUserManager : UserManager<MyUser>
    {
        public MyUserManager(IUserStore<MyUser> store) : base(store)
        {
        }

        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options,
            IOwinContext context)
        {            
            var manager = new MyUserManager(new MyUserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<MyUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false                                
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
             var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<MyUser>(dataProtectionProvider.Create("ASP.NET Identity"));                               
            }
            return manager;
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes

    public class MySignInManager : SignInManager<MyUser, string>
    {
        public MySignInManager(MyUserManager userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(MyUser user)
        {            
            return user.GenerateUserIdentityAsync((MyUserManager)UserManager);
        }

        public async override Task SignInAsync(MyUser user, bool isPersistent, bool rememberBrowser)
        {
            await base.SignInAsync(user, isPersistent, rememberBrowser).ConfigureAwait(false);
        }

        public static MySignInManager Create(IdentityFactoryOptions<MySignInManager> options, IOwinContext context)
        {
            return new MySignInManager(context.GetUserManager<MyUserManager>(), context.Authentication);
        }
    }
}