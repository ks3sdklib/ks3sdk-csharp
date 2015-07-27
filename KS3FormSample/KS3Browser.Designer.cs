using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace KS3FormSample
{
    partial class KS3Browser
    {
        private ComboBox bucketBox = new ComboBox();
        private BucketItem defaultBucketItem = new BucketItem("+ 选择空间");
        private Font defaultFont = new Font(new FontFamily("微软雅黑"), 10);
        private DataGridView objectsView = new DataGridView();
        DataGridViewButtonColumn downloadColumn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
        Button createBucketBtn = new Button();
        Button deleteBucketBtn = new Button();
        Button uploadFileBtn = new Button();

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ComboBox bucketList
            bucketBox.DropDownStyle = ComboBoxStyle.DropDownList;
            bucketBox.Items.Add(this.defaultBucketItem);
            bucketBox.SelectedItem = this.defaultBucketItem;
            bucketBox.SetBounds(0, 0, 200, 0);
            bucketBox.Font = this.defaultFont;
            bucketBox.SelectedIndexChanged += new EventHandler(this.changeBucketHandler);
            this.Controls.Add(bucketBox);

            // DataGridView objectsView
            objectsView = new DataGridView();
            objectsView.Font = this.defaultFont;
            objectsView.ColumnHeadersHeight = 30;
            objectsView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            objectsView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            objectsView.RowHeadersVisible = false;
            objectsView.AllowUserToAddRows = false;
            objectsView.AllowUserToResizeRows = false;
            objectsView.AllowUserToResizeColumns = false;
            objectsView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; 
            objectsView.SetBounds(0, 50, 700, 450);
            objectsView.ColumnCount = 3;
            objectsView.Columns[0].Name = "文件";
            objectsView.Columns[1].Name = "大小";
            objectsView.Columns[2].Name = "修改时间";
            objectsView.ReadOnly = true;
            this.Controls.Add(objectsView);

            objectsView.Columns.Add(downloadColumn);
            objectsView.Columns.Add(deleteColumn);
            downloadColumn.Name = "";
            downloadColumn.UseColumnTextForButtonValue = true;
            downloadColumn.Text = "下载";
            deleteColumn.Name = "";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.Text = "删除";
            downloadColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            deleteColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            downloadColumn.Width = 70;
            deleteColumn.Width = 70;

            objectsView.CellContentClick += new DataGridViewCellEventHandler(this.cellEventHandler);

            // 创建bucket
            createBucketBtn.Text = "创建Bucket";
            createBucketBtn.SetBounds(400, 0, 100, 30);
            createBucketBtn.Font = this.defaultFont;
            createBucketBtn.Click += new EventHandler(this.createBucketHandler);
            this.Controls.Add(createBucketBtn);

            // 删除bucket
            deleteBucketBtn.Text = "删除Bucket";
            deleteBucketBtn.SetBounds(500, 0, 100, 30);
            deleteBucketBtn.Font = this.defaultFont;
            deleteBucketBtn.Enabled = false;
            deleteBucketBtn.Click += new EventHandler(this.deleteBucketHandler);
            this.Controls.Add(deleteBucketBtn);

            // 上传文件
            uploadFileBtn.Text = "上传文件";
            uploadFileBtn.SetBounds(600, 0, 100, 30);
            uploadFileBtn.Font = this.defaultFont;
            uploadFileBtn.Enabled = false;
            uploadFileBtn.Click += new EventHandler(this.uploadFileHandler);
            this.Controls.Add(uploadFileBtn);

            // KS3Browser
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(700, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            this.Name = "KS3 Simple Browser";
            this.Text = "KS3 Simple Browser";
            this.Load += new System.EventHandler(this.KS3Browser_Load);
            this.ResumeLayout(false);
        }

    } // end of class KS3Browser

    /**
     * 将Bucket的name封装到这个类中
     */
    class BucketItem
    {
        private String bucketName = null;

        public BucketItem(String bucketName)
        {
            this.bucketName = bucketName;
        }

        public override int GetHashCode()
        {
            return this.bucketName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == this.GetType())
            { 
                BucketItem other = (BucketItem) obj;
                return this.bucketName.Equals(other.bucketName);
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return this.bucketName;
        }
    } // end of class BucketItem


    partial class CreateBucketForm
    {
        private TextBox nameBox = new TextBox();
        private Label tipLabel = new Label();
        private Font defaultFont = new Font(new FontFamily("微软雅黑"), 10);
        private Label nameLabel = new Label();
        private Button confirmBtn = new Button();
        private Button cancleBtn = new Button();
        
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 输入Bucket名称
            nameLabel.Font = this.defaultFont;
            nameLabel.Text = "Bucket名称:";
            nameLabel.SetBounds(50, 40, 90, 20);
            this.Controls.Add(nameLabel);
            nameBox.Font = this.defaultFont;
            nameBox.SetBounds(140, 38, 200, 30);
            this.Controls.Add(this.nameBox);

            // Tip
            String tag = "遵循DNS命名规范\n\n" + 
            "》仅包含小写英文字母(a-z), 数字, 点(.), 横线(-)\n" +
            "》须由字母或数字开头,数字或小写字母结尾\n" +
            "》长度为6-255位\n" +
            "》不允许 IP 格式 (eg. 10.10.10.1)\n" +
            "》不允许kss开头";
            tipLabel.Text = tag;
            tipLabel.SetBounds(50, 70, 300, 150);
            tipLabel.Font = this.defaultFont;
            this.Controls.Add(tipLabel);

            // 确定按钮
            confirmBtn.Text = "确定";
            confirmBtn.SetBounds(50, 230, 100, 30);
            confirmBtn.Font = this.defaultFont;
            confirmBtn.Click += new EventHandler(this.confirmHandler);
            this.Controls.Add(confirmBtn);

            // 取消按钮
            cancleBtn.Text = "取消";
            cancleBtn.SetBounds(160, 230, 100, 30);
            cancleBtn.Font = this.defaultFont;
            cancleBtn.Click += new EventHandler(this.cancleHandler);
            this.Controls.Add(cancleBtn);

            // CreateBucketForm
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 300);
            this.Name = "Create Bucket";
            this.Text = "Create Bucket";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
    } // end of class CreateBucketForm


    partial class UploadFileForm
    {
        private Label sourceFileLabel = new Label();
        private Label targetBucketLabel = new Label();
        private Label progressLabel = new Label();
        private Label statusLabel = new Label();
        private Label resultLabel = new Label();
        private Button completeBtn = new Button();
        private Button cancleBtn = new Button();
        private ProgressBar progressBar = new ProgressBar();
        private int scale = 1;
        private Font defaultFont = new Font(new FontFamily("微软雅黑"), 10);

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 源文件
            sourceFileLabel.Text = "源文件 :    " + this.file.Name;
            sourceFileLabel.AutoSize = true;
            sourceFileLabel.Font = this.defaultFont;
            sourceFileLabel.SetBounds(30, 30, 300, 30);
            this.Controls.Add(sourceFileLabel);

            // Bucket
            targetBucketLabel.Text = "Bucket :   " + this.bucketName;
            targetBucketLabel.AutoSize = true;
            targetBucketLabel.Font = this.defaultFont;
            targetBucketLabel.SetBounds(30, 60, 300, 30);
            this.Controls.Add(targetBucketLabel);

            // 进度
            progressLabel.Text = "进    度 :";
            progressLabel.AutoSize = true;
            progressLabel.Font = this.defaultFont;
            progressLabel.SetBounds(30, 120, 50, 30);
            this.Controls.Add(progressLabel);


            // 进度条
            progressBar.SetBounds(100, 118, 350, 25);
            long fileLength = file.Length;
            if (fileLength > 1000000000)
                this.scale = 1000000;
            progressBar.Maximum = (int)(fileLength / scale);
            progressBar.Value = 0;
            this.Controls.Add(progressBar);


            // 状态
            statusLabel.Text = "状    态 :";
            statusLabel.AutoSize = true;
            statusLabel.Font = this.defaultFont;
            statusLabel.SetBounds(30, 150, 50, 30);
            this.Controls.Add(statusLabel);

            // 结果
            resultLabel.Text = "准备中";
            resultLabel.AutoSize = true;
            resultLabel.Font = this.defaultFont;
            resultLabel.SetBounds(95, 150, 200, 30);
            this.Controls.Add(resultLabel);

            // 完成按钮
            completeBtn.Text = "完成";
            completeBtn.SetBounds(250, 210, 100, 30);
            completeBtn.Font = this.defaultFont;
            completeBtn.Enabled = false;
            completeBtn.Click += new EventHandler(this.completeHandler);
            this.Controls.Add(completeBtn);

            // 取消按钮
            cancleBtn.Text = "取消";
            cancleBtn.SetBounds(350, 210, 100, 30);
            cancleBtn.Font = this.defaultFont;
            cancleBtn.Click += new EventHandler(this.cancleHandler);
            this.Controls.Add(cancleBtn);


            // UploadFileForm
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 300);
            this.Name = "Upload File";
            this.Text = "Upload File";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
    } // end of class UploadFileForm


    partial class DownloadFileForm
    {
        private Label sourceBucketLabel = new Label();
        private Label targetFileLabel = new Label();
        private Label progressLabel = new Label();
        private Label statusLabel = new Label();
        private Label resultLabel = new Label();
        private Button completeBtn = new Button();
        private Button cancleBtn = new Button();
        private ProgressBar progressBar = new ProgressBar();
        private int scale = 1;
        private Font defaultFont = new Font(new FontFamily("微软雅黑"), 10);

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 源文件
            sourceBucketLabel.Text = "源文件 :    " + "/" + this.bucketName + "/" + this.key;
            sourceBucketLabel.AutoSize = true;
            sourceBucketLabel.Font = this.defaultFont;
            sourceBucketLabel.SetBounds(30, 30, 300, 30);
            this.Controls.Add(sourceBucketLabel);

            // 目标文件
            targetFileLabel.Text = "目标文件 :   " + this.file.Name;
            targetFileLabel.AutoSize = true;
            targetFileLabel.Font = this.defaultFont;
            targetFileLabel.SetBounds(30, 60, 300, 30);
            this.Controls.Add(targetFileLabel);

            // 进度
            progressLabel.Text = "进    度 :";
            progressLabel.AutoSize = true;
            progressLabel.Font = this.defaultFont;
            progressLabel.SetBounds(30, 120, 50, 30);
            this.Controls.Add(progressLabel);


            // 进度条
            progressBar.SetBounds(100, 118, 350, 25);
            long fileLength = this.size;
            if (fileLength > 1000000000)
                this.scale = 1000000;
            progressBar.Maximum = (int)(fileLength / scale);
            progressBar.Value = 0;
            this.Controls.Add(progressBar);


            // 状态
            statusLabel.Text = "状    态 :";
            statusLabel.AutoSize = true;
            statusLabel.Font = this.defaultFont;
            statusLabel.SetBounds(30, 150, 50, 30);
            this.Controls.Add(statusLabel);

            // 结果
            resultLabel.Text = "准备中";
            resultLabel.AutoSize = true;
            resultLabel.Font = this.defaultFont;
            resultLabel.SetBounds(95, 150, 200, 30);
            this.Controls.Add(resultLabel);

            // 完成按钮
            completeBtn.Text = "完成";
            completeBtn.SetBounds(250, 210, 100, 30);
            completeBtn.Font = this.defaultFont;
            completeBtn.Enabled = false;
            completeBtn.Click += new EventHandler(this.completeHandler);
            this.Controls.Add(completeBtn);

            // 取消按钮
            cancleBtn.Text = "取消";
            cancleBtn.SetBounds(350, 210, 100, 30);
            cancleBtn.Font = this.defaultFont;
            cancleBtn.Click += new EventHandler(this.cancleHandler);
            this.Controls.Add(cancleBtn);

            // DownloadFileForm
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 300);
            this.Name = "Download File";
            this.Text = "Download File";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
    } // end of class DownloadFileForm
}
