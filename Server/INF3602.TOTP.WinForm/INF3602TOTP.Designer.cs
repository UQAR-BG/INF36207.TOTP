namespace INF3602.TOTP.WinForm
{
    partial class INF3602TOTP
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.lbLastOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.lbTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(384, 224);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(117, 27);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Connexion";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lbLastOTP
            // 
            this.lbLastOTP.AutoSize = true;
            this.lbLastOTP.Location = new System.Drawing.Point(380, 276);
            this.lbLastOTP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLastOTP.Name = "lbLastOTP";
            this.lbLastOTP.Size = new System.Drawing.Size(97, 15);
            this.lbLastOTP.TabIndex = 1;
            this.lbLastOTP.Text = "Jeton précédant: ";
            // 
            // txtOTP
            // 
            this.txtOTP.Location = new System.Drawing.Point(384, 194);
            this.txtOTP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(116, 23);
            this.txtOTP.TabIndex = 2;
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Location = new System.Drawing.Point(517, 197);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(0, 15);
            this.lbTime.TabIndex = 3;
            // 
            // INF3602TOTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lbLastOTP);
            this.Controls.Add(this.btnLogin);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "INF3602TOTP";
            this.Text = "INF3602 - Application Serveur TOTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lbLastOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private Label lbTime;
    }
}