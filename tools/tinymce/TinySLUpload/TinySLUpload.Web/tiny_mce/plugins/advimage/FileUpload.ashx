<%@ WebHandler Language="C#" Class="FileUpload" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Diagnostics;

public class FileUpload : IHttpHandler {

    private HttpContext _httpContext;
    private string _tempExtension = "_temp";
    private string _fileName;
    private string _docType;
    private bool _lastChunk;
    private bool _firstChunk;
    private long _startByte;
    
    StreamWriter _debugFileStreamWriter;
    TextWriterTraceListener _debugListener;
 
    public void ProcessRequest(HttpContext context)
    {
        _httpContext = context;

        if (context.Request.InputStream.Length == 0)
            throw new ArgumentException("No file input");

        try
        {

            GetQueryStringParameters();

            string uploadFolder = GetUploadFolder();
            string tempFileName = _fileName + _tempExtension;

            if (_firstChunk)
            {
                //Delete temp file
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + tempFileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + tempFileName);

                //Delete target file
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + _fileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + _fileName);

            }

            using (FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + tempFileName, FileMode.Append))
            {
                SaveFile(context.Request.InputStream, fs);
                fs.Close();
            }

            if (_lastChunk)
            {
                File.Move(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + tempFileName, HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + _fileName);
            }

        }
        catch (Exception e)
        {
            throw;
        }

    }

    private void GetQueryStringParameters()
    {
        _fileName = _httpContext.Request.QueryString["file"];
        _docType = _httpContext.Request.QueryString["docType"];
        _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
        _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
        _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;
    }

    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }
    protected string GetUploadFolder()
    {
        string folder = System.Configuration.ConfigurationSettings.AppSettings["EditorImagePath"].ToString();
        folder = folder.Replace("~/", "");        
        return folder;
    }

    
    public bool IsReusable {
        get {
            return false;
        }
    }

}