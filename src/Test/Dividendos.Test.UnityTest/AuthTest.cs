using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dividendos.UnityTest
{
    public class AuthTest
    {
        private static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<TUser>(), "asfsafd")).ReturnsAsync(IdentityResult.Success.Succeeded);

            return mgr;
        }


        [Fact]
        public async void LogIn()
        {
            //ApplicationUser appUser = new ApplicationUser()
            //{
            //    UserName = "asdfsadf",
            //    Email = "admin1@teste.com.br",
            //    EmailConfirmed = true,
            //    Name = "Admin 1",
            //    PasswordHash = "asdfasf"
            //};

            //List<ApplicationUser> lstUser = new List<ApplicationUser>();

            //Mock<UserManager<ApplicationUser>> userManager = MockUserManager(lstUser);

            //IdentityResult resultado =  userManager.Object.CreateAsync(appUser, "asdf");

            //Assert.True( userManager.Object.CheckPasswordAsync(appUser, "asdf"), "Falha ao logar");
        }
    }
}
