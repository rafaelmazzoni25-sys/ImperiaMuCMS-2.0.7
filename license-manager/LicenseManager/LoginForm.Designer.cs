using System.Windows.Forms;

namespace LicenseManager;

partial class LoginForm
{
    private Label titleLabel;
    private Label usernameLabel;
    private TextBox usernameTextBox;
    private Label passwordLabel;
    private TextBox passwordTextBox;
    private Button loginButton;
    private Button cancelButton;
    private Label infoLabel;

    private void InitializeComponent()
    {
        titleLabel = new Label();
        usernameLabel = new Label();
        usernameTextBox = new TextBox();
        passwordLabel = new Label();
        passwordTextBox = new TextBox();
        loginButton = new Button();
        cancelButton = new Button();
        infoLabel = new Label();
        SuspendLayout();
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        titleLabel.Location = new System.Drawing.Point(24, 20);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new System.Drawing.Size(239, 21);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Entre para acessar o gerenciador";
        // 
        // usernameLabel
        // 
        usernameLabel.AutoSize = true;
        usernameLabel.Location = new System.Drawing.Point(24, 60);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new System.Drawing.Size(57, 15);
        usernameLabel.TabIndex = 1;
        usernameLabel.Text = "Usuário";
        // 
        // usernameTextBox
        // 
        usernameTextBox.Location = new System.Drawing.Point(24, 78);
        usernameTextBox.Name = "usernameTextBox";
        usernameTextBox.PlaceholderText = "admin";
        usernameTextBox.Size = new System.Drawing.Size(304, 23);
        usernameTextBox.TabIndex = 2;
        // 
        // passwordLabel
        // 
        passwordLabel.AutoSize = true;
        passwordLabel.Location = new System.Drawing.Point(24, 112);
        passwordLabel.Name = "passwordLabel";
        passwordLabel.Size = new System.Drawing.Size(39, 15);
        passwordLabel.TabIndex = 3;
        passwordLabel.Text = "Senha";
        // 
        // passwordTextBox
        // 
        passwordTextBox.Location = new System.Drawing.Point(24, 130);
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.PlaceholderText = "admin";
        passwordTextBox.Size = new System.Drawing.Size(304, 23);
        passwordTextBox.TabIndex = 4;
        passwordTextBox.UseSystemPasswordChar = true;
        // 
        // loginButton
        // 
        loginButton.Location = new System.Drawing.Point(164, 174);
        loginButton.Name = "loginButton";
        loginButton.Size = new System.Drawing.Size(75, 27);
        loginButton.TabIndex = 5;
        loginButton.Text = "Entrar";
        loginButton.UseVisualStyleBackColor = true;
        loginButton.Click += OnLogin;
        // 
        // cancelButton
        // 
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new System.Drawing.Point(253, 174);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new System.Drawing.Size(75, 27);
        cancelButton.TabIndex = 6;
        cancelButton.Text = "Cancelar";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += OnCancel;
        // 
        // infoLabel
        // 
        infoLabel.AutoSize = true;
        infoLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        infoLabel.ForeColor = System.Drawing.SystemColors.GrayText;
        infoLabel.Location = new System.Drawing.Point(24, 206);
        infoLabel.Name = "infoLabel";
        infoLabel.Size = new System.Drawing.Size(245, 13);
        infoLabel.TabIndex = 7;
        infoLabel.Text = "Credenciais padrão: admin / admin (altere depois).";
        // 
        // LoginForm
        // 
        AcceptButton = loginButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new System.Drawing.Size(352, 238);
        Controls.Add(infoLabel);
        Controls.Add(cancelButton);
        Controls.Add(loginButton);
        Controls.Add(passwordTextBox);
        Controls.Add(passwordLabel);
        Controls.Add(usernameTextBox);
        Controls.Add(usernameLabel);
        Controls.Add(titleLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Autenticação";
        ResumeLayout(false);
        PerformLayout();
    }
}
