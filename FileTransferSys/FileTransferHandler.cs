using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace FileTransferSys
{
    /// <summary>
    /// 文件转移
    /// </summary>
    public class FileTransferHandler
    {
        /// <summary>
        /// 配置地址
        /// </summary>
        private string _configPath;
        
        /// <summary>
        /// 配置中心
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// 输入地址
        /// </summary>
        private string _inputPath;

        /// <summary>
        /// 输出地址
        /// </summary>
        private string _outputPath;

        /// <summary>
        /// 需要处理的文件类型
        /// </summary>
        private List<string> _fileTypes;

        /// <summary>
        /// 资源初始化
        /// </summary>
        public FileTransferHandler()
        {
            _configPath = Directory.GetCurrentDirectory();
            _configuration = CommonService.ConfigInit(_configPath);
            _inputPath = _configuration["Appsettings:InputPath"];
            _outputPath = _configuration["Appsettings:OutputPath"];
            _fileTypes = _configuration["Appsettings:FileTypes"].Split(',').ToList();
        }

        /// <summary>
        /// 文件转移
        /// </summary>
        public void FileTransfer()
        {
            if (string.IsNullOrWhiteSpace(_inputPath) || string.IsNullOrWhiteSpace(_outputPath))
            {
                return;
            }
            FileTrance(_inputPath);
        }

        private void FileTrance(string path)
        {
            var directories = Directory.GetDirectories(path);
            if (directories == null || directories.Length == 0)
            {
                FileHandle(path);
            }
            else
            {
                foreach (var item in directories)
                {
                    FileTrance(item);
                }
                FileHandle(path);
            }
        }

        private void FileHandle(string path)
        {
            var files = Directory.GetFiles(path);
            if (files == null || files.Length == 0)
            {
                return;
            }
            foreach (var filePath in files)
            {
                var fileExtension = CommonService.GetFileExtension(filePath);
                if (fileExtension == null || string.IsNullOrWhiteSpace(fileExtension))
                {
                    continue;
                }
                if (!_fileTypes.Contains(fileExtension))
                {
                    continue;
                }

                try
                {
                    var transferService = TransferServiceFactory.GetTransferService(fileExtension);
                    transferService.FileTransfer(filePath, _outputPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
