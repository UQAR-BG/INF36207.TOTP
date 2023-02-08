namespace INF36207.TOTP.Client
{
    partial class ClientForm
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.lblMessageProchainToken = new System.Windows.Forms.Label();
            this.timerCountdown = new System.Windows.Forms.Timer(this.components);
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.txtJeton = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCountdown.Location = new System.Drawing.Point(122, 206);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(169, 59);
            this.lblCountdown.TabIndex = 0;
            this.lblCountdown.Text = "0 sec.";
            // 
            // lblMessageProchainToken
            // 
            this.lblMessageProchainToken.AutoSize = true;
            this.lblMessageProchainToken.Location = new System.Drawing.Point(109, 124);
            this.lblMessageProchainToken.Name = "lblMessageProchainToken";
            this.lblMessageProchainToken.Size = new System.Drawing.Size(207, 15);
            this.lblMessageProchainToken.TabIndex = 1;
            this.lblMessageProchainToken.Text = "Temps restant avant le prochain jeton:";
            // 
            // timerCountdown
            // 
            this.timerCountdown.Interval = 100;
            this.timerCountdown.Tick += new System.EventHandler(this.timerCountdown_Tick);
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Location = new System.Drawing.Point(297, 339);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(59, 52);
            this.btnCopyToClipboard.TabIndex = 3;
            this.btnCopyToClipboard.Text = "Copier";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // txtJeton
            // 
            this.txtJeton.Font = new System.Drawing.Font("Verdana", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtJeton.Location = new System.Drawing.Point(70, 339);
            this.txtJeton.Name = "txtJeton";
            this.txtJeton.ReadOnly = true;
            this.txtJeton.Size = new System.Drawing.Size(231, 52);
            this.txtJeton.TabIndex = 4;
            this.txtJeton.Text = "00000000";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 519);
            this.Controls.Add(this.txtJeton);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.lblMessageProchainToken);
            this.Controls.Add(this.lblCountdown);
            this.Name = "ClientForm";
            this.Text = "INF36207 - Application Client TOTP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblCountdown;
        private Label lblMessageProchainToken;
        private System.Windows.Forms.Timer timerCountdown;
        private Button btnCopyToClipboard;
        private TextBox txtJeton;
    }
}