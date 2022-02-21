namespace SimplestWebView2App
{
    partial class MainForm
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
            this.sPanel = new System.Windows.Forms.Panel();
            this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize) (this.webView2)).BeginInit();
            this.SuspendLayout();
            // 
            // sPanel
            // 
            this.sPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sPanel.Location = new System.Drawing.Point(0, 0);
            this.sPanel.Name = "sPanel";
            this.sPanel.Size = new System.Drawing.Size(191, 450);
            this.sPanel.TabIndex = 0;
            // 
            // webView2
            // 
            this.webView2.CreationProperties = null;
            this.webView2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2.Location = new System.Drawing.Point(191, 0);
            this.webView2.Margin = new System.Windows.Forms.Padding(10);
            this.webView2.Name = "webView2";
            this.webView2.Padding = new System.Windows.Forms.Padding(10);
            this.webView2.Size = new System.Drawing.Size(609, 450);
            this.webView2.TabIndex = 1;
            this.webView2.ZoomFactor = 1D;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webView2);
            this.Controls.Add(this.sPanel);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize) (this.webView2)).EndInit();
            this.ResumeLayout(false);
        }

        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;

        private System.Windows.Forms.Panel sPanel;

        #endregion
    }
}