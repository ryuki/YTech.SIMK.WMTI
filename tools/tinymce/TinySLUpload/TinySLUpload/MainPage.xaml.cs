using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Threading;
using System.Windows.Browser;

namespace TinySLUpload
{
    public partial class MainPage : UserControl
    {
        //file upload uri is set in the init params of the silverlight object tag
        private string FILEUPLOAD_URI = UploadConfig.HttpHandlerUri;
        List<FileInfo> fileColl = new List<FileInfo>();
        FileInfo _file;
        Stream fileStream;
        Dispatcher UIDispatcher;
        string FileSize = ""; //to show while uploading
        int ChunkSize = 4194304;
        long _dataLength;
        long _dataSent = 0;

        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        #region "Button Upload"

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            //In the future we need to allow multiple file uploads
            fd.Multiselect = false;

            if (fd.ShowDialog() == true)
            {
                foreach (FileInfo file in fd.Files)
                {
                    fileColl.Add(file);
                }
            }

            this.UploadFiles();
        }

        #endregion

        #region "Upload Files"
        private void UploadFiles()
        {
            foreach (FileInfo file in fileColl)
            {
                UIDispatcher = this.Dispatcher;
                FileSize = this.GetFileSize(file.Length);
                this.StartUpload(file);
            }
        }
        #endregion

        #region "Start Upload"
        private void StartUpload(FileInfo file)
        {
            _file = file;
            fileStream = _file.OpenRead();
            _dataLength = fileStream.Length;

            long dataToSend = _dataLength - _dataSent;
            bool isLastChunk = dataToSend <= ChunkSize;
            bool isFirstChunk = _dataSent == 0;
            string docType = "document";

            UriBuilder httpHandlerUrlBuilder = new UriBuilder(this.FILEUPLOAD_URI);
            httpHandlerUrlBuilder.Query = string.Format("{5}file={0}&offset={1}&last={2}&first={3}&docType={4}", _file.Name, _dataSent, isLastChunk, isFirstChunk, docType, string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpHandlerUrlBuilder.Uri);
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);

        }

        #endregion

        #region "Write Stream to Callback"
        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096];
            int bytesRead = 0;
            int tempTotal = 0;

            //Set the start position
            fileStream.Position = _dataSent;

            //Read the next chunk
            //&& !_file.IsDeleted && _file.State != Constants.FileStates.Error
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();

                _dataSent += bytesRead;
                tempTotal += bytesRead;

                //Show the progress change
                UIDispatcher.BeginInvoke(delegate()
                {
                    UpdateShowProgress(false);
                });

            }

            requestStream.Close();

            //Get the response from the HttpHandler
            webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);

        }
        #endregion

        #region "Read HTTP Response"
        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                string responsestring = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                UIDispatcher.BeginInvoke(delegate()
                {
                    ShowError();
                });
            }

            if (_dataSent < _dataLength)
            {
                //continue uploading the rest of the file in chunks
                StartUpload(_file);

                //Show the progress change
                UIDispatcher.BeginInvoke(delegate()
                {
                    UpdateShowProgress(false);
                });
            }
            else
            {
                fileStream.Close();
                fileStream.Dispose();

                //Show the progress change
                UIDispatcher.BeginInvoke(delegate()
                {
                    UpdateShowProgress(true);
                });
            }

        }
        #endregion

        #region "Update Show Progress"
        private void UpdateShowProgress(bool complete)
        {
            if (complete)
            {
                txtMessage.Text = "Image has been uploaded, please click on the General tab to preview your image.";

                HtmlPage.Window.Invoke("PopulateFromSL", UploadConfig.ImagePath + _file.Name);

                fileColl.Clear();
                _dataSent = 0;
                _dataLength = 0;
                _file = null;
                UIDispatcher = null;
                fileStream = null;
                FileSize = "";               
               
            }
            else
            {
                txtMessage.Text = "Total file size: " + FileSize + " Uploading: " + string.Format("{0:###.00}%", (double)_dataSent / (double)_dataLength * 100);
                progressUpload.Value = (double)_dataSent / (double)_dataLength;
            }
        }

        #endregion

        #region "Get The File Size"
        private string GetFileSize(long length)
        {
            double bytes = (double)length;

            string fileSize = "0 KB";

            if (bytes >= 1073741824)
                fileSize = String.Format("{0:##.##}", bytes / 1073741824) + " GB";
            else if (bytes >= 1048576)
                fileSize = String.Format("{0:##.##}", bytes / 1048576) + " MB";
            else if (bytes >= 1024)
                fileSize = String.Format("{0:##.##}", bytes / 1024) + " KB";
            else if (bytes > 0 && bytes < 1024)
                fileSize = "1 KB";

            return fileSize;
        }
        #endregion

        #region "Show Error"

        private void ShowError()
        {
            txtMessage.Text = "There was an error uploading your file";
        }

        #endregion

      
    }
}
