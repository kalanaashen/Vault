using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Controls.Documents;
using HarfBuzzSharp;
using Avalonia.Input.Platform;

namespace PasswordWallet.Views;

public partial class PasswordGenWindow : Window
{


    private readonly PasswordGenarator _passwordgen;
    public PasswordGenWindow()
    {
        InitializeComponent();
        _passwordgen = new PasswordGenarator();

    }


    private void GeneratePassword_Click(object? sender, RoutedEventArgs e)
    {
        bool isNumeric = NumberCheckBox.IsChecked == true;
        bool isUpperCase = UpperCaseCheckBox.IsChecked == true;
        bool isLowerCase = LowerCaseCheckBox.IsChecked == true;
        bool isSpecial = SymbolCheckBox.IsChecked == true;
        int length = int.TryParse(LengthTextBox.Text, out int parsedValue) ? parsedValue : 16;
        if (length < 4 || length > 128)
        {
            GeneratedPasswordTextBox.Text = "Length must be between 4 and 128.";
            return;
        }

        if (!isNumeric &&
    !isUpperCase &&
    !isLowerCase &&
    !isSpecial)
        {
            GeneratedPasswordTextBox.Text =
                "Select at least one character type.";

            return;
        }
        _passwordgen.IsNumeric = isNumeric;
        _passwordgen.IsCharacter = isSpecial;
        _passwordgen.IsLowerCase = isLowerCase;
        _passwordgen.IsUpperCase = isUpperCase;
        _passwordgen.Length = length;


        string text = _passwordgen.GeneratePassword();

        GeneratedPasswordTextBox.Text = text;


    }




    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {

        Close();




    }


    private async void CopyPassword_Click(
     object? sender,
     RoutedEventArgs e)
    {
        string? password = GeneratedPasswordTextBox.Text;

        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;

        if (clipboard is not null)
        {
            await clipboard.SetTextAsync(password);
        }
    }





}