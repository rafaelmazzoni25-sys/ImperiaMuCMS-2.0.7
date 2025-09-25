using System;
using System.Windows.Forms;
using LicenseManager.Models;

namespace LicenseManager;

public partial class LicenseDialog : Form
{
    public License License { get; private set; }

    public LicenseDialog()
    {
        InitializeComponent();
        statusComboBox.DataSource = Enum.GetValues(typeof(LicenseStatus));
        License = new License();
        BindLicense();
    }

    public LicenseDialog(License license) : this()
    {
        License = license.Clone();
        BindLicense();
    }

    private void BindLicense()
    {
        keyTextBox.Text = License.Key;
        emailTextBox.Text = License.Email;
        statusComboBox.SelectedItem = License.Status;
        createdDateTimePicker.Value = License.CreatedAt.ToLocalTime();
        expiresCheckBox.Checked = License.ExpiresAt.HasValue;
        expiresDateTimePicker.Value = (License.ExpiresAt ?? DateTime.UtcNow.AddMonths(1)).ToLocalTime();
        notesTextBox.Text = License.Notes;
    }

    private void OnExpiresCheckedChanged(object sender, EventArgs e)
    {
        expiresDateTimePicker.Enabled = expiresCheckBox.Checked;
    }

    private void OnSave(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(keyTextBox.Text))
        {
            MessageBox.Show("Informe a chave da licença.", "Dados obrigatórios",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(emailTextBox.Text))
        {
            MessageBox.Show("Informe o e-mail do cliente.", "Dados obrigatórios",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        License.Key = keyTextBox.Text.Trim();
        License.Email = emailTextBox.Text.Trim();
        License.Status = (LicenseStatus)statusComboBox.SelectedItem!;
        License.CreatedAt = DateTime.SpecifyKind(createdDateTimePicker.Value, DateTimeKind.Local).ToUniversalTime();
        License.ExpiresAt = expiresCheckBox.Checked
            ? DateTime.SpecifyKind(expiresDateTimePicker.Value, DateTimeKind.Local).ToUniversalTime()
            : null;
        License.Notes = notesTextBox.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void OnCancel(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
