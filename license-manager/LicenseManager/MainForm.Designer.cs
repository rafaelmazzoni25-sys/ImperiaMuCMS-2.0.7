using System.ComponentModel;
using System.Windows.Forms;
using LicenseManager.Models;
using LicenseModel = LicenseManager.Models.License;

namespace LicenseManager;

partial class MainForm
{
    private TableLayoutPanel layoutPanel;
    private ToolStrip toolStrip;
    private ToolStripButton addButton;
    private ToolStripButton editButton;
    private ToolStripButton deleteButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton activateButton;
    private ToolStripButton deactivateButton;
    private ToolStripButton banButton;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripLabel filterLabel;
    private ToolStripComboBox statusFilterComboBox;
    private ToolStripLabel typeFilterLabel;
    private ToolStripComboBox typeFilterComboBox;
    private ToolStripButton refreshButton;
    private DataGridView licensesDataGridView;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusStripLabel;
    private ToolStripStatusLabel userStatusLabel;
    private BindingSource licensesBindingSource;
    private DataGridViewTextBoxColumn keyColumn;
    private DataGridViewTextBoxColumn emailColumn;
    private DataGridViewTextBoxColumn typeColumn;
    private DataGridViewTextBoxColumn statusColumn;
    private DataGridViewTextBoxColumn createdColumn;
    private DataGridViewTextBoxColumn expiresColumn;
    private DataGridViewTextBoxColumn notesColumn;

    private void InitializeComponent()
    {
        layoutPanel = new TableLayoutPanel();
        toolStrip = new ToolStrip();
        addButton = new ToolStripButton();
        editButton = new ToolStripButton();
        deleteButton = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        activateButton = new ToolStripButton();
        deactivateButton = new ToolStripButton();
        banButton = new ToolStripButton();
        toolStripSeparator2 = new ToolStripSeparator();
        filterLabel = new ToolStripLabel();
        statusFilterComboBox = new ToolStripComboBox();
        typeFilterLabel = new ToolStripLabel();
        typeFilterComboBox = new ToolStripComboBox();
        refreshButton = new ToolStripButton();
        licensesDataGridView = new DataGridView();
        licensesBindingSource = new BindingSource();
        keyColumn = new DataGridViewTextBoxColumn();
        emailColumn = new DataGridViewTextBoxColumn();
        typeColumn = new DataGridViewTextBoxColumn();
        statusColumn = new DataGridViewTextBoxColumn();
        createdColumn = new DataGridViewTextBoxColumn();
        expiresColumn = new DataGridViewTextBoxColumn();
        notesColumn = new DataGridViewTextBoxColumn();
        statusStrip = new StatusStrip();
        statusStripLabel = new ToolStripStatusLabel();
        userStatusLabel = new ToolStripStatusLabel();
        layoutPanel.SuspendLayout();
        toolStrip.SuspendLayout();
        ((ISupportInitialize)licensesDataGridView).BeginInit();
        ((ISupportInitialize)licensesBindingSource).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // layoutPanel
        //
        layoutPanel.ColumnCount = 1;
        layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutPanel.Controls.Add(toolStrip, 0, 0);
        layoutPanel.Controls.Add(licensesDataGridView, 0, 1);
        layoutPanel.Dock = DockStyle.Fill;
        layoutPanel.RowCount = 2;
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layoutPanel.Location = new System.Drawing.Point(0, 0);
        layoutPanel.Name = "layoutPanel";
        layoutPanel.Size = new System.Drawing.Size(960, 560);
        layoutPanel.TabIndex = 0;
        //
        // toolStrip
        //
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.Items.AddRange(new ToolStripItem[]
        {
            addButton,
            editButton,
            deleteButton,
            toolStripSeparator1,
            activateButton,
            deactivateButton,
            banButton,
            toolStripSeparator2,
            filterLabel,
            statusFilterComboBox,
            typeFilterLabel,
            typeFilterComboBox,
            refreshButton
        });
        toolStrip.Location = new System.Drawing.Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Padding = new Padding(5, 5, 5, 5);
        toolStrip.Size = new System.Drawing.Size(960, 35);
        toolStrip.TabIndex = 0;
        toolStrip.Text = "toolStrip1";
        //
        // addButton
        //
        addButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        addButton.Text = "Adicionar";
        addButton.Click += OnAddLicense;
        //
        // editButton
        //
        editButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        editButton.Text = "Editar";
        editButton.Click += OnEditLicense;
        //
        // deleteButton
        //
        deleteButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        deleteButton.Text = "Excluir";
        deleteButton.Click += OnDeleteLicense;
        //
        // activateButton
        //
        activateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        activateButton.Text = "Ativar";
        activateButton.Click += OnActivate;
        //
        // deactivateButton
        //
        deactivateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        deactivateButton.Text = "Inativar";
        deactivateButton.Click += OnDeactivate;
        //
        // banButton
        //
        banButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        banButton.Text = "Banir";
        banButton.Click += OnBan;
        //
        // filterLabel
        //
        filterLabel.Margin = new Padding(10, 1, 0, 2);
        filterLabel.Text = "Status:";
        //
        // statusFilterComboBox
        //
        statusFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusFilterComboBox.Name = "statusFilterComboBox";
        statusFilterComboBox.Size = new System.Drawing.Size(121, 25);
        statusFilterComboBox.SelectedIndexChanged += OnStatusChanged;
        //
        // typeFilterLabel
        //
        typeFilterLabel.Margin = new Padding(10, 1, 0, 2);
        typeFilterLabel.Text = "Tipo:";
        //
        // typeFilterComboBox
        //
        typeFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        typeFilterComboBox.Name = "typeFilterComboBox";
        typeFilterComboBox.Size = new System.Drawing.Size(160, 25);
        typeFilterComboBox.SelectedIndexChanged += OnTypeFilterChanged;
        //
        // refreshButton
        //
        refreshButton.Alignment = ToolStripItemAlignment.Right;
        refreshButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        refreshButton.Text = "Recarregar";
        refreshButton.Click += OnRefresh;
        //
        // licensesDataGridView
        //
        licensesDataGridView.AllowUserToAddRows = false;
        licensesDataGridView.AllowUserToDeleteRows = false;
        licensesDataGridView.AutoGenerateColumns = false;
        licensesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
        licensesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        licensesDataGridView.Columns.AddRange(new DataGridViewColumn[]
        {
            keyColumn,
            emailColumn,
            typeColumn,
            statusColumn,
            createdColumn,
            expiresColumn,
            notesColumn
        });
        licensesDataGridView.DataSource = licensesBindingSource;
        licensesDataGridView.Dock = DockStyle.Fill;
        licensesDataGridView.Location = new System.Drawing.Point(3, 38);
        licensesDataGridView.MultiSelect = false;
        licensesDataGridView.Name = "licensesDataGridView";
        licensesDataGridView.ReadOnly = true;
        licensesDataGridView.RowHeadersVisible = false;
        licensesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        licensesDataGridView.Size = new System.Drawing.Size(954, 519);
        licensesDataGridView.TabIndex = 1;
        //
        // licensesBindingSource
        //
        licensesBindingSource.DataSource = typeof(LicenseModel);
        //
        // keyColumn
        //
        keyColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        keyColumn.DataPropertyName = nameof(LicenseModel.Key);
        keyColumn.HeaderText = "Chave";
        //
        // emailColumn
        //
        emailColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        emailColumn.DataPropertyName = nameof(LicenseModel.Email);
        emailColumn.HeaderText = "E-mail";
        //
        // typeColumn
        //
        typeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        typeColumn.DataPropertyName = nameof(LicenseModel.Type);
        typeColumn.HeaderText = "Tipo";
        //
        // statusColumn
        //
        statusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        statusColumn.DataPropertyName = nameof(LicenseModel.Status);
        statusColumn.HeaderText = "Status";
        //
        // createdColumn
        //
        createdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        createdColumn.DataPropertyName = nameof(LicenseModel.CreatedAt);
        createdColumn.DefaultCellStyle.Format = "g";
        createdColumn.HeaderText = "Criada em";
        //
        // expiresColumn
        //
        expiresColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        expiresColumn.DataPropertyName = nameof(LicenseModel.ExpiresAt);
        expiresColumn.DefaultCellStyle.Format = "g";
        expiresColumn.HeaderText = "Expira em";
        //
        // notesColumn
        //
        notesColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        notesColumn.DataPropertyName = nameof(LicenseModel.Notes);
        notesColumn.HeaderText = "Observações";
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { statusStripLabel, userStatusLabel });
        statusStrip.Location = new System.Drawing.Point(0, 560);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new System.Drawing.Size(960, 22);
        statusStrip.TabIndex = 2;
        statusStrip.Text = "statusStrip1";
        //
        // statusStripLabel
        //
        statusStripLabel.Spring = true;
        statusStripLabel.Text = "Ativas: 0   Inativas: 0   Banidas: 0";
        statusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // userStatusLabel
        //
        userStatusLabel.Text = "Usuário:";
        //
        // MainForm
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(960, 582);
        Controls.Add(layoutPanel);
        Controls.Add(statusStrip);
        MinimumSize = new System.Drawing.Size(780, 520);
        Name = "MainForm";
        Text = "Gerenciador de Licenças";
        layoutPanel.ResumeLayout(false);
        layoutPanel.PerformLayout();
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        ((ISupportInitialize)licensesDataGridView).EndInit();
        ((ISupportInitialize)licensesBindingSource).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
