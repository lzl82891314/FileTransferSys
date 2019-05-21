using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileTransferSys
{
    /// <summary>
    /// 文件转移
    /// </summary>
    public class FileTransferDefaultHandler : ITransferHandler
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
        /// 需要处理的文件类型
        /// </summary>
        private List<string> _fileTypes;

        /// <summary>
        /// 资源初始化
        /// </summary>
        public FileTransferDefaultHandler()
        {
            _configPath = Directory.GetCurrentDirectory();
            _configuration = CommonService.ConfigInit(_configPath);
            _fileTypes = _configuration["Appsettings:FileTypes"].Split(',').ToList();
        }


        private void FileTrance(string inputPath, string outputPath)
        {
            var directories = Directory.GetDirectories(inputPath);
            if (directories == null || directories.Length == 0)
            {
                FileHandle(inputPath, outputPath);
            }
            else
            {
                foreach (var item in directories)
                {
                    FileTrance(inputPath, outputPath);
                }
                FileHandle(inputPath, outputPath);
            }
        }

        private void FileHandle(string inputPath, string outputPath)
        {
            var files = Directory.GetFiles(inputPath);
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
                    transferService.FileTransfer(filePath, outputPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Task FileTransfer(string inputPath, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(inputPath) || string.IsNullOrWhiteSpace(outputPath))
            {
                return Task.CompletedTask;
            }

            var task = Task.Factory.StartNew(() => FileTrance(inputPath, outputPath));
            return task;
        }
    }
}
