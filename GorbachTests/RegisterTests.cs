using BuildFlowApp.ViewModels;

public class RegisterViewModelTests
{
    private class TestableRegisterViewModel : RegisterViewModel
    {
        public bool WasSuccessCalled { get; private set; }
        public List<string> ErrorMessages { get; } = new();

        public TestableRegisterViewModel()
        {
            OnError += msg => ErrorMessages.Add(msg);
            OnSuccess += () => WasSuccessCalled = true;
        }
    }


    [Theory]
    [InlineData(null, "email@example.com", "Password1", "Password1")]
    [InlineData("user", null, "Password1", "Password1")]
    [InlineData("user", "email@example.com", null, "Password1")]
    [InlineData("user", "email@example.com", "Password1", null)]
    [InlineData("", "", "", "")]
    public void Register_EmptyFields_ReturnsError(string username, string email, string password, string confirmPassword)
    {
        var vm = new TestableRegisterViewModel
        {
            Username = username,
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        };

        var result = vm.Register();

        Assert.False(result);
        Assert.Contains("Пожалуйста, заполните все поля.", vm.ErrorMessages);
    }

    [Fact]
    public void Register_UsernameTooShort_ShowsError()
    {
        var vm = new TestableRegisterViewModel
        {
            Username = "ab",
            Email = "email@example.com",
            Password = "Password1",
            ConfirmPassword = "Password1"
        };

        var result = vm.Register();

        Assert.False(result);
        Assert.Contains("Логин пользователя должен содержать минимум 3 символа.", vm.ErrorMessages);
    }

    [Theory]
    [InlineData("invalidemail")]
    [InlineData("invalid@")]
    [InlineData("invalid.com")]
    [InlineData("invalid@domain.")]
    [InlineData("invalid@.com")]
    public void Register_InvalidEmailFormat_ShowsError(string invalidEmail)
    {
        var vm = new TestableRegisterViewModel
        {
            Username = "user",
            Email = invalidEmail,
            Password = "Password1",
            ConfirmPassword = "Password1"
        };

        var result = vm.Register();

        Assert.False(result);
        Assert.Contains("Пожалуйста, введите корректный email адрес.", vm.ErrorMessages);
    }

    [Fact]
    public void Register_PasswordsDoNotMatch_ShowsError()
    {
        var vm = new TestableRegisterViewModel
        {
            Username = "user",
            Email = "email@example.com",
            Password = "Password1",
            ConfirmPassword = "Different1"
        };

        var result = vm.Register();

        Assert.False(result);
        Assert.Contains("Пароли не совпадают.", vm.ErrorMessages);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("abcdefgh")] 
    [InlineData("12345678")] 
    public void Register_WeakPassword_ShowsError(string weakPassword)
    {
        var vm = new TestableRegisterViewModel
        {
            Username = "user",
            Email = "email@example.com",
            Password = weakPassword,
            ConfirmPassword = weakPassword
        };

        var result = vm.Register();

        Assert.False(result);
        Assert.Contains("Пароль должен содержать минимум 8 символов, включая цифры и буквы.", vm.ErrorMessages);
    }

    [Fact]
    public void Register_ExistingUser_ShowsError()
    {
        var vm = new RegisterViewModelWithMockedAuthService(false);
        bool successTriggered = false;
        string errorMsg = null;

        vm.OnSuccess += () => successTriggered = true;
        vm.OnError += msg => errorMsg = msg;

        vm.Username = "testuser";
        vm.Email = "test@example.com";
        vm.Password = "Password1";
        vm.ConfirmPassword = "Password1";

        var result = vm.Register();

        Assert.False(result);
        Assert.False(successTriggered);
        Assert.Equal("Пользователь с таким именем или email уже существует.", errorMsg);
    }

    private class RegisterViewModelWithMockedAuthService : RegisterViewModel
    {
        private readonly bool _registerSuccess;

        public RegisterViewModelWithMockedAuthService(bool registerSuccess)
        {
            _registerSuccess = registerSuccess;
        }

        protected override bool RegisterUser(string username, string email, string password)
        {
            return _registerSuccess;
        }
    }
}
