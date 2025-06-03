using DesktopProgram.ViewModels;
using System;
using Xunit;

public class ChangeEmailViewModelTests
{
    [Fact]
    public void Constructor_ShouldSetCurrentEmail()
    {
        var vm = new ChangeEmailViewModel("test@example.com");
        Assert.Equal("test@example.com", vm.CurrentEmail);
        Assert.True(vm.IsPlaceholderVisible);
    }

    [Fact]
    public void NewEmail_SetToEmpty_ShouldShowPlaceholder()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        vm.NewEmail = "";
        Assert.True(vm.IsPlaceholderVisible);
    }

    [Fact]
    public void NewEmail_SetToNonEmpty_ShouldHidePlaceholder()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        vm.NewEmail = "new@example.com";
        Assert.False(vm.IsPlaceholderVisible);
    }

    [Fact]
    public void CancelCommand_ShouldRaiseRequestClose()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        bool closed = false;
        vm.RequestClose += () => closed = true;

        vm.CancelCommand.Execute(null);

        Assert.True(closed);
    }

    [Fact]
    public void SaveCommand_EmptyNewEmail_ShouldShowErrorAndNotClose()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        vm.NewEmail = "";
        bool closed = false;
        vm.RequestClose += () => closed = true;

        string message = null;
        vm.ShowMessage = (msg, title) => message = msg;

        vm.SaveCommand.Execute(null);

        Assert.Equal("¬ведите корректный Email.", message);
        Assert.False(closed);
    }

    [Fact]
    public void SaveCommand_InvalidNewEmail_ShouldShowErrorAndNotClose()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        vm.NewEmail = "not-an-email";
        bool closed = false;
        vm.RequestClose += () => closed = true;

        string message = null;
        vm.ShowMessage = (msg, title) => message = msg;

        vm.SaveCommand.Execute(null);

        Assert.Equal("¬ведите корректный Email.", message);
        Assert.False(closed);
    }

    [Fact]
    public void SaveCommand_ValidNewEmail_ShouldCloseWindow()
    {
        var vm = new ChangeEmailViewModel("old@example.com");
        vm.NewEmail = "valid@example.com";

        bool closed = false;
        vm.RequestClose += () => closed = true;

        string message = null;
        vm.ShowMessage = (msg, title) => message = msg;

        vm.SaveCommand.Execute(null);

        Assert.Equal("Email успешно изменЄн.", message);
        Assert.True(closed);
    }
}
