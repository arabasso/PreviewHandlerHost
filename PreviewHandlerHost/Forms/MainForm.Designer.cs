namespace PreviewHandlerHost.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.OptionsMenuStrip = new System.Windows.Forms.MenuStrip();
            this.ArquivoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ArquivoCarregarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PreviewHandlerHost = new PreviewHandlerHost.Controls.PreviewHandlerHost();
            this.OptionsMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsMenuStrip
            // 
            this.OptionsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArquivoMenuItem});
            this.OptionsMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.OptionsMenuStrip.Name = "OptionsMenuStrip";
            this.OptionsMenuStrip.Size = new System.Drawing.Size(972, 24);
            this.OptionsMenuStrip.TabIndex = 1;
            this.OptionsMenuStrip.Text = "menuStrip1";
            // 
            // ArquivoMenuItem
            // 
            this.ArquivoMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArquivoCarregarMenuItem});
            this.ArquivoMenuItem.Name = "ArquivoMenuItem";
            this.ArquivoMenuItem.Size = new System.Drawing.Size(61, 20);
            this.ArquivoMenuItem.Text = "&Arquivo";
            // 
            // ArquivoCarregarMenuItem
            // 
            this.ArquivoCarregarMenuItem.Name = "ArquivoCarregarMenuItem";
            this.ArquivoCarregarMenuItem.Size = new System.Drawing.Size(128, 22);
            this.ArquivoCarregarMenuItem.Text = "&Carregar...";
            this.ArquivoCarregarMenuItem.Click += new System.EventHandler(this.ArquivoCarregarMenuItemClick);
            // 
            // PreviewHandlerHost
            // 
            this.PreviewHandlerHost.BackColor = System.Drawing.Color.White;
            this.PreviewHandlerHost.CannotCreateClassText = "Não foi possível criar a classe";
            this.PreviewHandlerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewHandlerHost.Location = new System.Drawing.Point(0, 24);
            this.PreviewHandlerHost.Name = "PreviewHandlerHost";
            this.PreviewHandlerHost.NoPreviewHandlerText = "Nenhuma visualização disponível";
            this.PreviewHandlerHost.PreviewLoadingText = "Carregando visualização...";
            this.PreviewHandlerHost.Size = new System.Drawing.Size(972, 547);
            this.PreviewHandlerHost.TabIndex = 2;
            this.PreviewHandlerHost.Text = "Selecione um arquivo para visualizar";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 571);
            this.Controls.Add(this.PreviewHandlerHost);
            this.Controls.Add(this.OptionsMenuStrip);
            this.MainMenuStrip = this.OptionsMenuStrip;
            this.Name = "MainForm";
            this.Text = "Windows Forms Preview Handler";
            this.OptionsMenuStrip.ResumeLayout(false);
            this.OptionsMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip OptionsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ArquivoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ArquivoCarregarMenuItem;
        private Controls.PreviewHandlerHost PreviewHandlerHost;
    }
}

