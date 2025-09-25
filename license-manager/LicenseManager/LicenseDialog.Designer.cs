using System;
using System.Windows.Forms;

namespace LicenseManager;

partial class LicenseDialog
{
    private TableLayoutPanel layoutPanel;
    private Label keyLabel;
    private TextBox keyTextBox;
    private Label emailLabel;
    private TextBox emailTextBox;
    private Label statusLabel;
    private ComboBox statusComboBox;
    private Label createdLabel;
    private DateTimePicker createdDateTimePicker;
    private CheckBox expiresCheckBox;
    private DateTimePicker expiresDateTimePicker;
    private Label notesLabel;
    private TextBox notesTextBox;
    private FlowLayoutPanel buttonsPanel;
    private Button saveButton;
    private Button cancelButton;

    private void InitializeComponent()
    {
        layoutPanel = new TableLayoutPanel();
        keyLabel = new Label();
        keyTextBox = new TextBox();
        emailLabel = new Label();
        emailTextBox = new TextBox();
        statusLabel = new Label();
        statusComboBox = new ComboBox();
        createdLabel = new Label();
        createdDateTimePicker = new DateTimePicker();
        expiresCheckBox = new CheckBox();
        expiresDateTimePicker = new DateTimePicker();
        notesLabel = new Label();
        notesTextBox = new TextBox();
        buttonsPanel = new FlowLayoutPanel();
        saveButton = new Button();
        cancelButton = new Button();
        layoutPanel.SuspendLayout();
        buttonsPanel.SuspendLayout();
        SuspendLayout();
        // 
        // layoutPanel
        // 
        layoutPanel.ColumnCount = 2;
        layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutPanel.Controls.Add(keyLabel, 0, 0);
        layoutPanel.Controls.Add(keyTextBox, 1, 0);
        layoutPanel.Controls.Add(emailLabel, 0, 1);
        layoutPanel.Controls.Add(emailTextBox, 1, 1);
        layoutPanel.Controls.Add(statusLabel, 0, 2);
        layoutPanel.Controls.Add(statusComboBox, 1, 2);
        layoutPanel.Controls.Add(createdLabel, 0, 3);
        layoutPanel.Controls.Add(createdDateTimePicker, 1, 3);
        layoutPanel.Controls.Add(expiresCheckBox, 1, 4);
        layoutPanel.Controls.Add(expiresDateTimePicker, 1, 5);
        layoutPanel.Controls.Add(notesLabel, 0, 6);
        layoutPanel.Controls.Add(notesTextBox, 1, 6);
        layoutPanel.Controls.Add(buttonsPanel, 1, 7);
        layoutPanel.Dock = DockStyle.Fill;
        layoutPanel.Padding = new Padding(10);
        layoutPanel.RowCount = 8;
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        layoutPanel.Location = new System.Drawing.Point(0, 0);
        layoutPanel.Name = "layoutPanel";
        layoutPanel.Size = new System.Drawing.Size(480, 360);
        layoutPanel.TabIndex = 0;
        // 
        // keyLabel
        // 
        keyLabel.Anchor = AnchorStyles.Left;
        keyLabel.AutoSize = true;
        keyLabel.Text = "Chave";
        // 
        // keyTextBox
        // 
        keyTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        keyTextBox.TabIndex = 0;
        // 
        // emailLabel
        // 
        emailLabel.Anchor = AnchorStyles.Left;
        emailLabel.AutoSize = true;
        emailLabel.Text = "E-mail";
        // 
        // emailTextBox
        // 
        emailTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        emailTextBox.TabIndex = 1;
        // 
        // statusLabel
        // 
        statusLabel.Anchor = AnchorStyles.Left;
        statusLabel.AutoSize = true;
        statusLabel.Text = "Status";
        // 
        // statusComboBox
        // 
        statusComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusComboBox.TabIndex = 2;
        // 
        // createdLabel
        // 
        createdLabel.Anchor = AnchorStyles.Left;
        createdLabel.AutoSize = true;
        createdLabel.Text = "Criada em";
        // 
        // createdDateTimePicker
        // 
        createdDateTimePicker.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        createdDateTimePicker.Format = DateTimePickerFormat.Custom;
        createdDateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";
        createdDateTimePicker.TabIndex = 3;
        // 
        // expiresCheckBox
        // 
        expiresCheckBox.Anchor = AnchorStyles.Left;
        expiresCheckBox.AutoSize = true;
        expiresCheckBox.Text = "Definir data de expiração";
        expiresCheckBox.CheckedChanged += OnExpiresCheckedChanged;
        expiresCheckBox.TabIndex = 4;
        // 
        // expiresDateTimePicker
        // 
        expiresDateTimePicker.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        expiresDateTimePicker.Format = DateTimePickerFormat.Custom;
        expiresDateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";
        expiresDateTimePicker.TabIndex = 5;
        // 
        // notesLabel
        // 
        notesLabel.Anchor = AnchorStyles.Left;
        notesLabel.AutoSize = true;
        notesLabel.Text = "Observações";
        // 
        // notesTextBox
        // 
        notesTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        notesTextBox.Multiline = true;
        notesTextBox.ScrollBars = ScrollBars.Vertical;
        notesTextBox.TabIndex = 6;
        // 
        // buttonsPanel
        // 
        buttonsPanel.Anchor = AnchorStyles.Right;
        buttonsPanel.AutoSize = true;
        buttonsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonsPanel.Controls.Add(saveButton);
        buttonsPanel.Controls.Add(cancelButton);
        buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonsPanel.Padding = new Padding(0, 5, 0, 0);
        buttonsPanel.TabIndex = 7;
        // 
        // saveButton
        // 
        saveButton.AutoSize = true;
        saveButton.Text = "Salvar";
        saveButton.Click += OnSave;
        // 
        // cancelButton
        // 
        cancelButton.AutoSize = true;
        cancelButton.Text = "Cancelar";
        cancelButton.Click += OnCancel;
        // 
        // LicenseDialog
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(480, 360);
        Controls.Add(layoutPanel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LicenseDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Licença";
        AcceptButton = saveButton;
        CancelButton = cancelButton;
        layoutPanel.ResumeLayout(false);
        layoutPanel.PerformLayout();
        buttonsPanel.ResumeLayout(false);
        buttonsPanel.PerformLayout();
        ResumeLayout(false);
    }
}
