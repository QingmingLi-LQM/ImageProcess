
using System.Windows.Forms;

namespace SharpWindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn = new Button();
            this.singleBtn = new Button();
            this.multiBtn = new Button();
            this.imageStitchBtn = new Button();
            this.imageSpiliteBtn = new Button();
            this.subImag = new Button();
            this.BtnFileName = new Button();
            this.clarifyEvalue = new Button();
            this.SuspendLayout();

            this.btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Location = new System.Drawing.Point(0, 0);
            this.btn.Size = new System.Drawing.Size(80, 30);
            this.btn.Text = "点击";
            this.btn.Name = "btn";
            this.btn.Click += new System.EventHandler(this.btn_Click);

            this.singleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.singleBtn.UseVisualStyleBackColor = true;
            this.singleBtn.Location = new System.Drawing.Point(80, 0);
            this.singleBtn.Size = new System.Drawing.Size(80, 30);
            this.singleBtn.Text = "单通道像素";
            this.singleBtn.Name = "singleBtn";
            this.singleBtn.Click += new System.EventHandler(this.singleBtn_Click);

            this.multiBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiBtn.UseVisualStyleBackColor = true;
            this.multiBtn.Location = new System.Drawing.Point(160, 0);
            this.multiBtn.Size = new System.Drawing.Size(80, 30);
            this.multiBtn.Text = "多通道像素";
            this.multiBtn.Name = "btn";
            this.multiBtn.Click += new System.EventHandler(this.multiBtn_Click);

            this.imageStitchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageStitchBtn.UseVisualStyleBackColor = true;
            this.imageStitchBtn.Location = new System.Drawing.Point(400, 0);
            this.imageStitchBtn.Size = new System.Drawing.Size(80, 30);
            this.imageStitchBtn.Text = "图片混合";
            this.imageStitchBtn.Name = "imageStitchBtn";
            this.imageStitchBtn.Click += new System.EventHandler(this.stitchImageBtn_Click);

            this.imageSpiliteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageSpiliteBtn.UseVisualStyleBackColor = true;
            this.imageSpiliteBtn.Location = new System.Drawing.Point(480, 0);
            this.imageSpiliteBtn.Size = new System.Drawing.Size(80, 30);
            this.imageSpiliteBtn.Text = "图片拼接";
            this.imageSpiliteBtn.Name = "imageSpiliteBtn";
            this.imageSpiliteBtn.Click += new System.EventHandler(this.imageSpiliteBtn_Click);

            this.subImag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subImag.UseVisualStyleBackColor = true;
            this.subImag.Location = new System.Drawing.Point(0, 35);
            this.subImag.Size = new System.Drawing.Size(80, 30);
            this.subImag.Text = "子块图像";
            this.subImag.Name = "subImag";
            this.subImag.Click += new System.EventHandler(this.subImag_Click);

            this.BtnFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFileName.UseVisualStyleBackColor = true;
            this.BtnFileName.Location = new System.Drawing.Point(85, 35);
            this.BtnFileName.Size = new System.Drawing.Size(80, 30);
            this.BtnFileName.Text = "文件名称";
            this.BtnFileName.Name = "BtnFileName";
            this.BtnFileName.Click += new System.EventHandler(this.BtnFileName_Click);

            this.clarifyEvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clarifyEvalue.UseVisualStyleBackColor = true;
            this.clarifyEvalue.Location = new System.Drawing.Point(165, 35);
            this.clarifyEvalue.Size = new System.Drawing.Size(80, 30);
            this.clarifyEvalue.Text = "清晰度评价";
            this.clarifyEvalue.Name = "clarifyEvalue";
            this.clarifyEvalue.Click += new System.EventHandler(this.clarifyEvalution_Click);

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.singleBtn);
            this.Controls.Add(this.multiBtn);
            this.Controls.Add(this.imageStitchBtn);
            this.Controls.Add(this.imageSpiliteBtn);
            this.Controls.Add(this.subImag);
            this.Controls.Add(this.BtnFileName);
            this.Controls.Add(this.clarifyEvalue);
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private Button btn;
        private Button singleBtn;
        private Button multiBtn;
        private Button imageStitchBtn;
        private Button imageSpiliteBtn;
        private Button subImag;
        private Button BtnFileName;
        private Button clarifyEvalue;
    }
}

