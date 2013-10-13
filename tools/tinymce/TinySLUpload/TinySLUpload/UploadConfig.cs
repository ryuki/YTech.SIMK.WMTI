using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TinySLUpload
{
    public class UploadConfig
    {
        private static string _handlerUri;
        public static string HttpHandlerUri
        {
            get { return _handlerUri; }
            set { _handlerUri = value; }
        }

        private static string _imagePath;
        public static string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }
    }
}
