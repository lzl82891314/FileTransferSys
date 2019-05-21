using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    public class ImageTransferEventHandler : ITransferEventHandler
    {
        /// <summary>
        /// 转移服务
        /// </summary>
        private ITransferService _transferService;

        public TransferEvent TransferEvent => ImageTransfer;

        private ImageTransferEventHandler()
        {
            _transferService = ImageTransferService.GetInstance();
        }

        public static ITransferEventHandler GetInstance()
        {
            return InstanceClass.Instance;
        }

        public bool IsCanTransfer(string fileType)
        {
            switch (fileType.Trim().ToLower())
            {
                case "jpg":
                case "png": return true;
                default: return false;
            }
        }

        private Task ImageTransfer(string fileType, string filePath, string outputPath)
        {
            if (!IsCanTransfer(fileType))
            {
                return null;
            }

            return Task.Factory.StartNew(() => _transferService.FileTransfer(filePath, outputPath));
        }

        private class InstanceClass
        {
            public static ImageTransferEventHandler Instance = new ImageTransferEventHandler();
        }
    }
}
