using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using KS3;
using KS3.Model;
using KS3.KS3Exception;

namespace KS3FormSample
{
    /**
     * 一个简单的KS3的Browser的实现
     */
    partial class KS3Browser : Form
    {
        static String accessKey = "your accessKey";
        static String secretKey = "your secretKey";
        
        // KS3 Operation class 
        static KS3Client ks3Client = null;

        public KS3Browser()
        {
            InitializeComponent();
        }

        private void KS3Browser_Load(object sender, EventArgs e) {
            ks3Client = new KS3Client(accessKey, secretKey);
            
            this.loadBucketList();
        }

        private static void showMessage(String message, String caption)
        {
            Thread thread = new Thread(delegate() { MessageBox.Show(message, caption); });
            thread.Start();
        }

        /**
         * 得到Bucket的列表，并将这个列表录入到ComboBox中
         */
        public void loadBucketList()
        {
            try
            {
                this.bucketBox.Items.Clear();
                this.bucketBox.Items.Add(this.defaultBucketItem);
                this.bucketBox.SelectedIndex = 0;

                IList<Bucket> bucketList = ks3Client.listBuckets();
                foreach (Bucket bucket in bucketList)
                    this.bucketBox.Items.Add(new BucketItem(bucket.getName()));
            }
            catch (ServiceException e)
            {
                showMessage(e.ToString(), "错误");
            }
            catch
            {
                showMessage("初始化BucketList失败", "错误");
            }
        }

        /**
         * 当ComboBox中选择的Bucket发生变化时触发此函数，刷新Object的列表
         */
        private void changeBucketHandler(Object sender, EventArgs eventArgs)
        {
            this.loadObjectList();
        }

        /**
         * 刷新Object的列表
         */
        public void loadObjectList()
        {
            lock (this)
            {
                this.objectsView.Rows.Clear();

                Object obj = this.bucketBox.SelectedItem;
                if (!obj.Equals(this.defaultBucketItem))
                {
                    this.loadObjectList(this.bucketBox.SelectedItem.ToString());
                    this.deleteBucketBtn.Enabled = true;
                    this.uploadFileBtn.Enabled = true;
                }
                else
                {
                    this.deleteBucketBtn.Enabled = false;
                    this.uploadFileBtn.Enabled = false;
                }
            }
        }

        /**
         * 根据指定Bucket名称获取Object的列表并填入表格
         */
        private void loadObjectList(String bucketName)
        {
            ObjectListing objList = ks3Client.listObjects(bucketName);

            IList<ObjectSummary> summaryList = objList.getObjectSummaries();
            
            foreach (ObjectSummary summary in summaryList)
            {
                String[] row = new string[] {
                    summary.getKey(),
                    summary.getSize().ToString() + " B", 
                    summary.getLastModified().ToString() };
                this.objectsView.Rows.Add(row);
            }

        }

        /**
         * 当创建Bucket的按钮被点击时触发此函数，并实例化创建Bucket的窗体
         */
        private void createBucketHandler(Object sender, EventArgs eventArgs)
        {
            new CreateBucketForm(this, ks3Client).ShowDialog();
        }

        /**
         * 当删除Bucket的按钮被点击时触发此函数，并删除此时ComboBox中选中的那个Bucket
         */
        private void deleteBucketHandler(Object sender, EventArgs eventArgs)
        {
            try
            { 
                Object obj = this.bucketBox.SelectedItem;
                if (!obj.Equals(this.defaultBucketItem))
                {
                    String bucketName = obj.ToString().Trim();
                    ks3Client.deleteBucket(bucketName);

                    showMessage("成功删除Bucket: " + bucketName, "消息");

                    this.loadBucketList();
                }
            }
            catch (ServiceException e)
            {
                showMessage(e.ToString(), "错误");
            }
            catch
            {
                showMessage("未知错误，请稍后再试", "错误");
            }
        }

        /**
         * 当上传文件的按钮被点击时，触发此函数，用户选择一个文件后实例化上传文件的窗体
         */
        private void uploadFileHandler(Object sender, EventArgs eventArgs)
        {
            Object obj = this.bucketBox.SelectedItem;
            if (!obj.Equals(this.defaultBucketItem))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo file = new FileInfo(dialog.FileName);

                    new UploadFileForm(this, ks3Client, obj.ToString(), file).Show();
                }
            }
        }

        /**
         * 当DataGridView中的元素被点击时触发此函数，主要用于查看用户是否点击了删除Object和下载
         * Object这两个按钮，如果点击了，则进行相应的操作
         */
        private void cellEventHandler(Object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.ColumnIndex == 4)
            {
                int row = eventArgs.RowIndex;
                String key = this.objectsView.Rows[row].Cells[0].Value.ToString().Trim();
                Object obj = this.bucketBox.SelectedItem;

                if(!obj.Equals(this.defaultBucketItem))
                {
                    String bucketName = obj.ToString().Trim();

                    this.deleteObject(bucketName, key);
                    
                    this.loadObjectList();
                }
            }
            else if (eventArgs.ColumnIndex == 3)
            {
                int row = eventArgs.RowIndex;
                String key = this.objectsView.Rows[row].Cells[0].Value.ToString().Trim();
                Object obj = this.bucketBox.SelectedItem;

                if (!obj.Equals(this.defaultBucketItem))
                {
                    String bucketName = obj.ToString().Trim();

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.RestoreDirectory = true;
                    dialog.FileName = key;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (!File.Exists(dialog.FileName))
                            File.Create(dialog.FileName).Close();
                        
                        FileInfo file = new FileInfo(dialog.FileName);

                        new DownloadFileForm(this, ks3Client, bucketName, key, file).Show();
                    }
                }
            }
        }

        /**
         * 根据Bucket的名称和Object的名称删除一个指定的Object
         */
        private void deleteObject(String bucketName, String key)
        {
            try
            {
                ks3Client.deleteObject(bucketName, key);

                showMessage("成功删除文件: " + key, "消息");  
            }
            catch (ServiceException e)
            {
                showMessage(e.ToString(), "错误");
            }
            catch
            {
                showMessage("未知错误，请稍后再试", "错误");
            }
        }

    } // end of class KS3Browser

    /**
     * 在创建Bucket的过程中和用户进行交互的窗体
     */
    partial class CreateBucketForm : Form
    {
        private KS3Browser ks3Browser;
        private KS3Client ks3Client;

        public CreateBucketForm(KS3Browser ks3Browser, KS3Client ks3Client)
        {
            this.ks3Browser = ks3Browser;
            this.ks3Client = ks3Client;
            InitializeComponent();
        }

        /**
         * 当用户点击取消时，触发此函数
         */
        private void cancleHandler(Object sender, EventArgs eventArgs)
        {
            this.Dispose();
        }

        /**
         * 当用户点击确定时触发此函数，并依据服务器返回的结果提示用户Bucket是否
         * 创建成功
         */
        private void confirmHandler(Object sender, EventArgs eventArgs)
        {
            Bucket bucket = null;
            try
            {
                bucket = this.ks3Client.createBucket(this.nameBox.Text.Trim());
                showMessage("创建成功，Bucket名称为: " + bucket.getName(), "消息");

                this.ks3Browser.loadBucketList();
                this.Dispose();
            }
            catch (ServiceException e)
            {
                showMessage(e.ToString(), "错误");
            }
            catch
            {
                showMessage("未知错误，请稍后再试", "错误");
            }
        }

        /**
         * 用于显示错误信息
         */
        private static void showMessage(String message, String caption)
        {
            Thread thread = new Thread(delegate() { MessageBox.Show(message, caption); });
            thread.Start();
        }
    } // end of class CreateBucketForm


    /**
     * 在上传文件的过程中和用户进行交互的窗体
     */
    partial class UploadFileForm : Form, ProgressListener
    {
        private KS3Browser ks3Browser;
        private KS3Client ks3Client;
        private String bucketName;
        private FileInfo file;
        private bool cancled = false; // 标记当前这个任务是否已被取消

        private long completedSize; // 已完成的字节数
        private long size; // 文件的总字节数
        
        public UploadFileForm(KS3Browser ks3Browser, KS3Client ks3Client, String bucketName, FileInfo file)
        {
            this.ks3Browser = ks3Browser;
            this.ks3Client = ks3Client;
            this.bucketName = bucketName;
            this.file = file;

            this.completedSize = 0;
            this.size = file.Length;
            
            InitializeComponent();

            start();
        }

        /**
         * 开始上传，新建一个线程用于IO操作
         */
        private void start()
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest(this.bucketName, this.file.Name, this.file);
            putObjectRequest.setProgressListener(this);

            Thread thread = new Thread(delegate() {
                try
                {
                    ks3Client.putObject(putObjectRequest);
                }
                catch (ServiceException e)
                {
                    showMessage(e.ToString(), "错误");
                }
                catch (ProgressInterruptedException e)
                { 
                    // Do nothing here ...
                }
                catch
                {
                    showMessage("未知错误，请稍后再试", "错误");
                }
                
            });
            thread.Start();
        }
        
        /**
         * 当用户点击完成按钮时，触发此函数，并关闭窗体
         */
        private void completeHandler(Object sender, EventArgs eventArgs)
        {
            this.Dispose();
        }

        /**
         * 当用户点击取消按钮时，触发此函数，并标记当前任务已取消
         */
        private void cancleHandler(Object sender, EventArgs eventArgs)
        {
            this.cancled = true;
        }

        /**
         * 实现的ProgressListener的接口，用于询问当前任务是否继续
         */
        public bool askContinue()
        {
            return !this.cancled;
        }

        private delegate void ProgressChangedHandler(ProgressEvent progressEvent);

        /**
         * 实现的ProgressListener的接口，处理IO线程反馈回来的消息
         */
        public void progressChanged(ProgressEvent progressEvent)
        {
            
            /**
             * 用线程安全的方式重绘界面
             */
            if (this.resultLabel.InvokeRequired)
            {
                this.Invoke(new ProgressChangedHandler(progressChanged), new object[] { progressEvent });
                return;
            }

            int eventCode = progressEvent.getEventCode();
            if (eventCode == ProgressEvent.STARTED)
                this.resultLabel.Text = "正在进行中";
            else if (eventCode == ProgressEvent.COMPLETED)
            {
                this.resultLabel.Text = "已完成";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
                this.ks3Browser.loadObjectList();
            }
            else if (eventCode == ProgressEvent.FAILED)
            {
                this.resultLabel.Text = "已失败";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
            }
            else if (eventCode == ProgressEvent.CANCELED)
            {
                this.resultLabel.Text = "已取消";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
            }
            else if (eventCode == ProgressEvent.TRANSFERRED)
            {
                this.completedSize += progressEvent.getBytesTransferred();
                
                long x = this.completedSize;
                if (scale > 1)
                    x = (long)((double)x / scale + 0.5);

                this.progressBar.Value = (int) x;
            }
        }

        /**
         * 用于显示错误信息
         */
        private static void showMessage(String message, String caption)
        {
            Thread thread = new Thread(delegate() { MessageBox.Show(message, caption); });
            thread.Start();
        }
    } // end of class UploadFileForm


    /**
     * 进行下载文件的操作时和用户进行交互的窗体
     */
    partial class DownloadFileForm : Form, ProgressListener
    {
        private KS3Browser ks3Browser;
        private KS3Client ks3Client;
        private String bucketName;
        private String key;
        private FileInfo file;
        private bool cancled = false;

        private long completedSize;
        private long size;

        public DownloadFileForm(KS3Browser ks3Browser, KS3Client ks3Client, String bucketName, String key, FileInfo file)
        {
            this.ks3Browser = ks3Browser;
            this.ks3Client = ks3Client;
            this.bucketName = bucketName;
            this.key = key;
            this.file = file;

            try
            {
                // 先通过获取Object的Metadata中的Content-Length字段的值，来确定当前要下载的文件的大小
                ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, key);
                
                this.completedSize = 0;
                this.size = objMeta.getContentLength();
            }
            catch (ServiceException e)
            {
                showMessage(e.ToString(), "错误");
                this.Dispose();
                return ;
            }
            catch
            {
                showMessage("未知错误，请稍后再试", "错误");
                return ;
            }

            InitializeComponent();

            this.completedSize = 0;
            start();
        }

        /**
         * 开始下载，并新建一个线程用于IO操作
         */
        private void start()
        {

            GetObjectRequest getObjectRequest = new GetObjectRequest(this.bucketName, this.key, this.file);
            getObjectRequest.setProgressListener(this);

            Thread thread = new Thread(delegate() {
                KS3Object ks3Obj = null;
                try
                {
                     ks3Obj = ks3Client.getObject(getObjectRequest);
                }
                catch (ServiceException e)
                {
                    showMessage(e.ToString(), "错误");
                }
                catch (ProgressInterruptedException e)
                {
                    // Do nothing here ...
                }
                catch
                {
                    showMessage("未知错误，请稍后再试", "错误");
                }
                finally
                {
                    if (ks3Obj != null && ks3Obj.getObjectContent() != null)
                        ks3Obj.getObjectContent().Close();
                }
                
            });
            thread.Start();
        }
        
        /**
         * 当用户点击完成按钮时触发此函数，并关闭窗体
         */
        private void completeHandler(Object sender, EventArgs eventArgs)
        {
            this.Dispose();
        }


        /**
         * 当用户点击取消按钮时，触发此函数，并标记当前任务已取消
         */
        private void cancleHandler(Object sender, EventArgs eventArgs)
        {
            this.cancled = true;
        }

        /**
         * 实现的ProgressListener的接口，用于询问当前任务是否继续
         */
        public bool askContinue()
        {
            return !this.cancled;
        }

        private delegate void ProgressChangedHandler(ProgressEvent progressEvent);

        /**
         * 实现的ProgressListener的接口，处理IO线程反馈回来的消息
         */
        public void progressChanged(ProgressEvent progressEvent)
        {
            /**
             * 用线程安全的方式重绘界面
             */
            if (this.resultLabel.InvokeRequired)
            {
                this.Invoke(new ProgressChangedHandler(progressChanged), new object[] { progressEvent });
                return;
            }

            int eventCode = progressEvent.getEventCode();
            if (eventCode == ProgressEvent.STARTED)
                this.resultLabel.Text = "正在进行中";
            else if (eventCode == ProgressEvent.COMPLETED)
            {
                this.resultLabel.Text = "已完成";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
            }
            else if (eventCode == ProgressEvent.FAILED)
            {
                this.resultLabel.Text = "已失败";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
            }
            else if (eventCode == ProgressEvent.CANCELED)
            {
                this.resultLabel.Text = "已取消";
                this.cancleBtn.Enabled = false;
                this.completeBtn.Enabled = true;
            }
            else if (eventCode == ProgressEvent.TRANSFERRED)
            {
                this.completedSize += progressEvent.getBytesTransferred();

                long x = this.completedSize;
                if (scale > 1)
                    x = (long)((double)x / scale + 0.5);

                this.progressBar.Value = (int)x;
            }
        }

        /**
         * 用于显示错误信息
         */
        private static void showMessage(String message, String caption)
        {
            Thread thread = new Thread(delegate() { MessageBox.Show(message, caption); });
            thread.Start();
        }
    }
}
