using System;
using System.Windows.Forms;
using LicenseManager.Models;
using LicenseManager.Services;

namespace LicenseManager;

public partial class LoginForm : Form
{
    private readonly AuthService _authService;

    public User? AuthenticatedUser { get; private set; }

    public LoginForm(AuthService authService)
    {
        _authService = authService;
        InitializeComponent();
    }

    private void OnLogin(object sender, EventArgs e)
    {
        var username = usernameTextBox.Text.Trim();
        var password = passwordTextBox.Text;

        var user = _authService.Authenticate(username, password);
        if (user is null)
        {
            MessageBox.Show("Credenciais inválidas. Verifique usuário e senha.",
                "Falha na autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            passwordTextBox.SelectAll();
            passwordTextBox.Focus();
            return;
        }

        AuthenticatedUser = user;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void OnCancel(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
