using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LicenseManager.Models;
using LicenseModel = LicenseManager.Models.License;
using LicenseManager.Services;

namespace LicenseManager;

public partial class MainForm : Form
{
    private readonly LicenseRepository _repository = new();
    private readonly BindingList<LicenseModel> _licenses = new();

    public MainForm()
    {
        InitializeComponent();
        statusFilterComboBox.DataSource = Enum.GetValues(typeof(LicenseStatus));
        statusFilterComboBox.SelectedItem = LicenseStatus.Active;
        typeFilterComboBox.Items.Clear();
        typeFilterComboBox.Items.Add("Todos");
        foreach (LicenseType licenseType in Enum.GetValues(typeof(LicenseType)))
        {
            typeFilterComboBox.Items.Add(licenseType);
        }

        if (typeFilterComboBox.Items.Count > 0)
        {
            typeFilterComboBox.SelectedIndex = 0;
        }

        licensesDataGridView.AutoGenerateColumns = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ReloadData();
    }

    private void ReloadData()
    {
        _licenses.Clear();
        foreach (var license in _repository.Load().OrderByDescending(l => l.CreatedAt))
        {
            _licenses.Add(license);
        }

        licensesBindingSource.DataSource = _licenses;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        IEnumerable<LicenseModel> filtered = _licenses;

        if (statusFilterComboBox.SelectedItem is LicenseStatus status)
        {
            filtered = filtered.Where(l => l.Status == status);
        }

        if (typeFilterComboBox.SelectedItem is LicenseType licenseType)
        {
            filtered = filtered.Where(l => l.Type == licenseType);
        }

        licensesBindingSource.DataSource = new BindingList<LicenseModel>(filtered.ToList());
        licensesDataGridView.Refresh();
        UpdateStatusCounters();
    }

    private void UpdateStatusCounters()
    {
        var totalActive = _licenses.Count(l => l.Status == LicenseStatus.Active);
        var totalInactive = _licenses.Count(l => l.Status == LicenseStatus.Inactive);
        var totalBanned = _licenses.Count(l => l.Status == LicenseStatus.Banned);
        statusStripLabel.Text =
            $"Ativas: {totalActive}   Inativas: {totalInactive}   Banidas: {totalBanned}";
    }

    private LicenseModel? GetSelectedLicense()
    {
        if (licensesBindingSource.Current is LicenseModel license)
        {
            return license;
        }

        MessageBox.Show("Selecione uma licença na lista.", "Nenhuma licença selecionada",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    private void OnAddLicense(object sender, EventArgs e)
    {
        var dialog = new LicenseDialog();
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            var license = dialog.License;
            license.CreatedAt = DateTime.UtcNow;
            _repository.Upsert(license);
            ReloadData();
        }
    }

    private void OnEditLicense(object sender, EventArgs e)
    {
        var license = GetSelectedLicense();
        if (license is null)
        {
            return;
        }

        var dialog = new LicenseDialog(license);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _repository.Upsert(dialog.License);
            ReloadData();
        }
    }

    private void OnDeleteLicense(object sender, EventArgs e)
    {
        var license = GetSelectedLicense();
        if (license is null)
        {
            return;
        }

        if (MessageBox.Show($"Remover licença {license.Key}?", "Confirmar exclusão",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            _repository.Delete(license.Id);
            ReloadData();
        }
    }

    private void OnStatusChanged(object sender, EventArgs e)
    {
        ApplyFilter();
    }

    private void OnTypeFilterChanged(object sender, EventArgs e)
    {
        ApplyFilter();
    }

    private void OnActivate(object sender, EventArgs e) => UpdateStatus(LicenseStatus.Active);

    private void OnDeactivate(object sender, EventArgs e) => UpdateStatus(LicenseStatus.Inactive);

    private void OnBan(object sender, EventArgs e) => UpdateStatus(LicenseStatus.Banned);

    private void UpdateStatus(LicenseStatus status)
    {
        var license = GetSelectedLicense();
        if (license is null)
        {
            return;
        }

        if (license.Status == status)
        {
            return;
        }

        license.Status = status;
        _repository.Upsert(license);
        ReloadData();
    }

    private void OnRefresh(object sender, EventArgs e) => ReloadData();
}
