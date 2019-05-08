using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTransferSys
{
    /// <summary>
    /// 视频类型文件转移策略
    /// </summary>
    public class VideoTransferService : ITransferService
    {
        /// <summary>
        /// 最小移动数量
        /// </summary>
        private long _minSize;

        /// <summary>
        /// 配置中心
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// 最小移动大小（字节）
        /// </summary>
        private const long MINSIZE = 1024 * 1024 * 10;

        private VideoTransferService()
        {
            _configuration = CommonService.ConfigInit(Directory.GetCurrentDirectory());
            if (!long.TryParse(_configuration["Appsettings:VideoMinSize"], out _minSize))
            {
                _minSize = MINSIZE;
            }
        }

        public static VideoTransferService GetInstance()
        {
            return InstanceClass.Instance;
        }

        /// <summary>
        /// 视频文件转移
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputPath"></param>
        public void FileTransfer(string filePath, string outputPath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(outputPath) || !Directory.Exists(outputPath))
            {
                return;
            }

            //可以做视频限制，比如当视频是.mp4格式时大于XX兆才转移
            var fileExtension = CommonService.GetFileExtension(filePath);
            if (fileExtension == null)
            {
                return;
            }
            if (fileExtension.Equals("mp4"))
            {
                using (var fileStream = File.Open(filePath, FileMode.Open))
                {
                    if (fileStream.Length < _minSize)
                    {
                        return;
                    }
                }
            }

            string outputFilePath = Path.Combine(outputPath, CommonService.GetFileName(filePath));
            File.Move(filePath, outputFilePath);
        }

        private static class InstanceClass
        {
            public static VideoTransferService Instance = new VideoTransferService();
        }
    }
}
