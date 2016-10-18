using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BHIC.Common;

namespace BHIC.EncryptDecrypt
{
    public partial class EncryptionDecryptionForm : Form
    {
        public EncryptionDecryptionForm()
        {
            InitializeComponent();
        }

        private void btnEncryptDecrypt_Click(object sender, EventArgs e)
        {

            if (btnEncryptDecrypt.Text.Equals("Encrypt", StringComparison.CurrentCulture))
            {
                if (chkUseDefaultPublicKey.Checked)
                {
                    txtEncrptDecrypt.Text = Encryption.EncryptText(txtEncrptDecrypt.Text);
                }
                else
                {
                    txtEncrptDecrypt.Text = Encryption.EncryptText(txtEncrptDecrypt.Text, txtPublicKey.Text);
                }
                lblEncryptDecrypt.Text = "String to Decrypt :";
                btnEncryptDecrypt.Text = "Decrypt";
            }
            else
            {
                if (chkUseDefaultPublicKey.Checked)
                {
                    txtEncrptDecrypt.Text = Encryption.DecryptText(txtEncrptDecrypt.Text);
                }
                else
                {
                    txtEncrptDecrypt.Text = Encryption.DecryptText(txtEncrptDecrypt.Text, txtPublicKey.Text);
                }
                lblEncryptDecrypt.Text = "String to Encrypt :";
                btnEncryptDecrypt.Text = "Encrypt";
            }
        }

        private void chkUseDefaultPublicKey_CheckedChanged(object sender, EventArgs e)
        {
            txtPublicKey.ReadOnly = chkUseDefaultPublicKey.Checked;
        }

        private void btnFileEncryptDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog inputFile = new OpenFileDialog();
            string inputFileName = string.Empty;
            string outputFileName = string.Empty;
            if (inputFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                inputFileName = inputFile.FileName;

                SaveFileDialog outputFile = new SaveFileDialog();
                if (outputFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    outputFileName = outputFile.FileName;
                    if (btnFileEncryptDecrypt.Text.Contains("Encrypt"))
                    {
                        Encryption.EncryptFile(inputFileName, outputFileName);
                        btnFileEncryptDecrypt.Text = "File Decrypt";
                    }
                    else
                    {
                        Encryption.DecryptFile(inputFileName, outputFileName);
                        btnFileEncryptDecrypt.Text = "File Encrypt";
                    }
                }
            }
        }
    }
}
