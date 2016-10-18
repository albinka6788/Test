namespace BHIC.EncryptDecrypt
{
    partial class EncryptionDecryptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.lblEncryptDecrypt = new System.Windows.Forms.Label();
            this.txtEncrptDecrypt = new System.Windows.Forms.TextBox();
            this.btnEncryptDecrypt = new System.Windows.Forms.Button();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.lblPublicKey = new System.Windows.Forms.Label();
            this.chkUseDefaultPublicKey = new System.Windows.Forms.CheckBox();
            this.btnFileEncryptDecrypt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEncryptDecrypt
            // 
            this.lblEncryptDecrypt.AutoSize = true;
            this.lblEncryptDecrypt.Location = new System.Drawing.Point(13, 33);
            this.lblEncryptDecrypt.Name = "lblEncryptDecrypt";
            this.lblEncryptDecrypt.Size = new System.Drawing.Size(91, 13);
            this.lblEncryptDecrypt.TabIndex = 0;
            this.lblEncryptDecrypt.Text = "String to Encrypt :";
            // 
            // txtEncrptDecrypt
            // 
            this.txtEncrptDecrypt.Location = new System.Drawing.Point(111, 33);
            this.txtEncrptDecrypt.Multiline = true;
            this.txtEncrptDecrypt.Name = "txtEncrptDecrypt";
            this.txtEncrptDecrypt.Size = new System.Drawing.Size(339, 110);
            this.txtEncrptDecrypt.TabIndex = 1;
            // 
            // btnEncryptDecrypt
            // 
            this.btnEncryptDecrypt.Location = new System.Drawing.Point(375, 254);
            this.btnEncryptDecrypt.Name = "btnEncryptDecrypt";
            this.btnEncryptDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncryptDecrypt.TabIndex = 2;
            this.btnEncryptDecrypt.Text = "Encrypt";
            this.btnEncryptDecrypt.UseVisualStyleBackColor = true;
            this.btnEncryptDecrypt.Click += new System.EventHandler(this.btnEncryptDecrypt_Click);
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.Location = new System.Drawing.Point(111, 194);
            this.txtPublicKey.Multiline = true;
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.ReadOnly = true;
            this.txtPublicKey.Size = new System.Drawing.Size(339, 38);
            this.txtPublicKey.TabIndex = 4;
            // 
            // lblPublicKey
            // 
            this.lblPublicKey.AutoSize = true;
            this.lblPublicKey.Location = new System.Drawing.Point(13, 194);
            this.lblPublicKey.Name = "lblPublicKey";
            this.lblPublicKey.Size = new System.Drawing.Size(63, 13);
            this.lblPublicKey.TabIndex = 3;
            this.lblPublicKey.Text = "Public Key :";
            // 
            // chkUseDefaultPublicKey
            // 
            this.chkUseDefaultPublicKey.AutoSize = true;
            this.chkUseDefaultPublicKey.Checked = true;
            this.chkUseDefaultPublicKey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseDefaultPublicKey.Location = new System.Drawing.Point(111, 160);
            this.chkUseDefaultPublicKey.Name = "chkUseDefaultPublicKey";
            this.chkUseDefaultPublicKey.Size = new System.Drawing.Size(125, 17);
            this.chkUseDefaultPublicKey.TabIndex = 5;
            this.chkUseDefaultPublicKey.Text = "Can Use Default Key";
            this.chkUseDefaultPublicKey.UseVisualStyleBackColor = true;
            this.chkUseDefaultPublicKey.CheckedChanged += new System.EventHandler(this.chkUseDefaultPublicKey_CheckedChanged);
            // 
            // btnFileEncryptDecrypt
            // 
            this.btnFileEncryptDecrypt.Location = new System.Drawing.Point(285, 254);
            this.btnFileEncryptDecrypt.Name = "btnFileEncryptDecrypt";
            this.btnFileEncryptDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnFileEncryptDecrypt.TabIndex = 6;
            this.btnFileEncryptDecrypt.Text = "File Encrypt";
            this.btnFileEncryptDecrypt.UseVisualStyleBackColor = true;
            this.btnFileEncryptDecrypt.Click += new System.EventHandler(this.btnFileEncryptDecrypt_Click);
            // 
            // EncryptionDecryptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 295);
            this.Controls.Add(this.btnFileEncryptDecrypt);
            this.Controls.Add(this.chkUseDefaultPublicKey);
            this.Controls.Add(this.txtPublicKey);
            this.Controls.Add(this.lblPublicKey);
            this.Controls.Add(this.btnEncryptDecrypt);
            this.Controls.Add(this.txtEncrptDecrypt);
            this.Controls.Add(this.lblEncryptDecrypt);
            this.MaximizeBox = false;
            this.Name = "EncryptionDecryptionForm";
            this.Text = "Encryption/Decryption Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEncryptDecrypt;
        private System.Windows.Forms.TextBox txtEncrptDecrypt;
        private System.Windows.Forms.Button btnEncryptDecrypt;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Label lblPublicKey;
        private System.Windows.Forms.CheckBox chkUseDefaultPublicKey;
        private System.Windows.Forms.Button btnFileEncryptDecrypt;
    }
}

