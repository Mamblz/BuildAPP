using System;
using System.Threading.Tasks;
using DesktopProgram.Models;
using DesktopProgram.Services;
using DesktopProgram.ViewModels;
using Moq;
using Xunit;

namespace VershininTests
{
    public class ChangePasswordViewModelTests
    {
        private User CreateTestUser()
        {
            return new User
            {
                Username = "testuser",
                PasswordHash = new AuthService().GetHashedPassword("oldpass")
            };
        }

        [Fact]
        public void SaveCommand_ShouldNotProceed_WhenFieldsAreEmpty()
        {
            var user = CreateTestUser();
            var vm = new ChangePasswordViewModel(user);

            vm.OldPassword = "";
            vm.NewPassword = "";
            vm.ConfirmPassword = "";

            bool closed = false;
            vm.RequestClose += () => closed = true;

            vm.SaveCommand.Execute(null);

            Assert.False(closed);
        }

        [Fact]
        public void SaveCommand_ShouldNotProceed_WhenNewPasswordsDoNotMatch()
        {
            var user = CreateTestUser();
            var vm = new ChangePasswordViewModel(user);

            vm.OldPassword = "oldpass";
            vm.NewPassword = "newpass1";
            vm.ConfirmPassword = "newpass2";

            bool closed = false;
            vm.RequestClose += () => closed = true;

            vm.SaveCommand.Execute(null);

            Assert.False(closed);
        }

        [Fact]
        public void SaveCommand_ShouldNotProceed_WhenOldPasswordIsIncorrect()
        {
            var user = CreateTestUser();
            var vm = new ChangePasswordViewModel(user);

            vm.OldPassword = "wrongpass";
            vm.NewPassword = "newpass";
            vm.ConfirmPassword = "newpass";

            bool closed = false;
            vm.RequestClose += () => closed = true;

            vm.SaveCommand.Execute(null);

            Assert.False(closed);
        }

        [Fact]
        public void CancelCommand_ShouldRaiseCloseRequest()
        {
            var user = CreateTestUser();
            var vm = new ChangePasswordViewModel(user);

            bool closed = false;
            vm.RequestClose += () => closed = true;

            vm.CancelCommand.Execute(null);

            Assert.True(closed);
        }
    }
}
