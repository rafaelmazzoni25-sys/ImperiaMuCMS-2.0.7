using System.ComponentModel;
using System.Windows.Forms;
using LicenseManager.Models;

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
    private ToolStripButton refreshButton;
    private DataGridView licensesDataGridView;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusStripLabel;
    private BindingSource licensesBindingSource;
    private DataGridViewTextBoxColumn keyColumn;
    private DataGridViewTextBoxColumn emailColumn;
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
        refreshButton = new ToolStripButton();
        licensesDataGridView = new DataGridView();
        licensesBindingSource = new BindingSource();
        keyColumn = new DataGridViewTextBoxColumn();
        emailColumn = new DataGridViewTextBoxColumn();
        statusColumn = new DataGridViewTextBoxColumn();
        createdColumn = new DataGridViewTextBoxColumn();
        expiresColumn = new DataGridViewTextBoxColumn();
        notesColumn = new DataGridViewTextBoxColumn();
        statusStrip = new StatusStrip();
        statusStripLabel = new ToolStripStatusLabel();
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
        layoutPanel.Size = new System.Drawing.Size(900, 520);
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
            refreshButton
        });
        toolStrip.Location = new System.Drawing.Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Padding = new Padding(5, 5, 5, 5);
        toolStrip.Size = new System.Drawing.Size(900, 35);
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
            statusColumn,
            createdColumn,
            expiresColumn,
            notesColumn
        });
        licensesDataGridView.DataSource = licensesBindingSource;
        licensesDataGridView.Dock = DockStyle.Fill;
        licensesDataGridView.MultiSelect = false;
        licensesDataGridView.ReadOnly = true;
        licensesDataGridView.RowHeadersVisible = false;
        licensesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        licensesDataGridView.Location = new System.Drawing.Point(3, 38);
        licensesDataGridView.Name = "licensesDataGridView";
        licensesDataGridView.Size = new System.Drawing.Size(894, 479);
        licensesDataGridView.TabIndex = 1;
        // 
        // licensesBindingSource
        // 
        licensesBindingSource.DataSource = typeof(License);
        // 
        // keyColumn
        // 
        keyColumn.DataPropertyName = nameof(License.Key);
        keyColumn.HeaderText = "Chave";
        keyColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // 
        // emailColumn
        // 
        emailColumn.DataPropertyName = nameof(License.Email);
        emailColumn.HeaderText = "E-mail";
        emailColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // 
        // statusColumn
        // 
        statusColumn.DataPropertyName = nameof(License.Status);
        statusColumn.HeaderText = "Status";
        statusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        // 
        // createdColumn
        // 
        createdColumn.DataPropertyName = nameof(License.CreatedAt);
        createdColumn.HeaderText = "Criada em";
        createdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        createdColumn.DefaultCellStyle.Format = "g";
        // 
        // expiresColumn
        // 
        expiresColumn.DataPropertyName = nameof(License.ExpiresAt);
        expiresColumn.HeaderText = "Expira em";
        expiresColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        expiresColumn.DefaultCellStyle.Format = "g";
        // 
        // notesColumn
        // 
        notesColumn.DataPropertyName = nameof(License.Notes);
        notesColumn.HeaderText = "Observações";
        notesColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { statusStripLabel });
        statusStrip.Location = new System.Drawing.Point(0, 520);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new System.Drawing.Size(900, 22);
        statusStrip.TabIndex = 2;
        statusStrip.Text = "statusStrip1";
        // 
        // statusStripLabel
        // 
        statusStripLabel.Text = "Ativas: 0   Inativas: 0   Banidas: 0";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(900, 542);
        Controls.Add(layoutPanel);
        Controls.Add(statusStrip);
        MinimumSize = new System.Drawing.Size(720, 480);
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
