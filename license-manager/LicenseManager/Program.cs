using System;
using System.Windows.Forms;
using LicenseManager.Models;
using LicenseManager.Services;

namespace LicenseManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var repository = new LicenseRepository();
        var auditService = new AuditService();
        var authService = new AuthService();

        using var loginForm = new LoginForm(authService);
        if (loginForm.ShowDialog() != DialogResult.OK || loginForm.AuthenticatedUser is null)
        {
            return;
        }

        User authenticatedUser = loginForm.AuthenticatedUser;
        auditService.Record("Login (desktop)", authenticatedUser.Username, details: "Sessão iniciada no gerenciador desktop.");

        Application.Run(new MainForm(repository, auditService, authenticatedUser));

        auditService.Record("Logout (desktop)", authenticatedUser.Username, details: "Sessão encerrada no gerenciador desktop.");
    }
}
