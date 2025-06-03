using Xunit;
using Moq;
using DesktopProgram.Models;
using DesktopProgram.ViewModels;
using DesktopProgram.Services;

public class LoginViewModelTests
{
    [Theory]
    [InlineData("", "pass1234")]
    [InlineData("user", "")]
    [InlineData("us", "pass1234")]
    [InlineData("user", "abcdefg")]
    [InlineData("user", "12345678")]
    public void IsInputValid_InvalidInputs_ReturnsFalse(string login, string password)
    {
        var vm = new LoginViewModel(Mock.Of<IAuthService>());
        Assert.False(vm.IsInputValid(login, password));
    }

    [Fact]
    public void LoginCommand_WithValidCredentials_CallsLoginSuccessful()
    {
        var mockAuth = new Mock<IAuthService>();
        var user = new User { Username = "ValidUser" };
        mockAuth.Setup(x => x.Login("validuser", "Pass1234")).Returns(user);

        var vm = new LoginViewModel(mockAuth.Object)
        {
            Login = "validuser",
            Password = "Pass1234"
        };

        User actualUser = null;
        vm.LoginSuccessful += u => actualUser = u;

        vm.LoginCommand.Execute(null);

        Assert.NotNull(actualUser);
        Assert.Equal("ValidUser", actualUser.Username);
    }

    [Fact]
    public void LoginCommand_InvalidCredentials_DoesNotCallLoginSuccessful()
    {
        var mockAuth = new Mock<IAuthService>();
        mockAuth.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

        var vm = new LoginViewModel(mockAuth.Object)
        {
            Login = "invalid",
            Password = "WrongPass123"
        };

        bool called = false;
        vm.LoginSuccessful += _ => called = true;

        vm.LoginCommand.Execute(null);

        Assert.False(called);
    }

    [Fact]
    public void RegisterCommand_TriggersSwitchToRegisterEvent()
    {
        var vm = new LoginViewModel(Mock.Of<IAuthService>());

        bool wasTriggered = false;
        vm.SwitchToRegister += () => wasTriggered = true;

        vm.RegisterCommand.Execute(null);

        Assert.True(wasTriggered);
    }

    [Fact]
    public void LoginCommand_DoesNotCallAuthService_OnInvalidInput()
    {
        var mockAuth = new Mock<IAuthService>();
        var vm = new LoginViewModel(mockAuth.Object)
        {
            Login = "",
            Password = ""
        };

        vm.LoginCommand.Execute(null);

        mockAuth.Verify(x => x.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
