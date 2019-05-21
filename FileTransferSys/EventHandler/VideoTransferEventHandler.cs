using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    public class VideoTransferEventHandler : ITransferEventHandler
    {
        /// <summary>
        /// 转移服务
        /// </summary>
        private ITransferService _transferService;

        public TransferEvent TransferEvent => VideoTransfer;

        private VideoTransferEventHandler()
        {
            _transferService = VideoTransferService.GetInstance();
        }

        public static ITransferEventHandler GetInstance()
        {
            return InstanceClass.Instance;
        }

        public bool IsCanTransfer(string fileType)
        {
            switch (fileType.Trim().ToLower())
            {
                case "mp4":
                case "rmvb":
                case "3gp":
                case "mkv":
                case "avi": return true;
                default: return false;
            }
        }

        private Task VideoTransfer(string fileType, string filePath, string outputPath)
        {
            if (!IsCanTransfer(fileType))
            {
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(() => _transferService.FileTransfer(filePath, outputPath));
        }

        private class InstanceClass
        {
            public static VideoTransferEventHandler Instance = new VideoTransferEventHandler();
        }
    }
}
