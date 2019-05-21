using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    public class CommonTransferEventHandler : ITransferEventHandler
    {
        /// <summary>
        /// 转移服务
        /// </summary>
        private ITransferService _transferService;

        public TransferEvent TransferEvent => CommonTransfer;

        private CommonTransferEventHandler()
        {
            _transferService = CommonTransferService.GetInstance();
        }

        public static ITransferEventHandler GetInstance()
        {
            return InstanceClass.Instance;
        }

        public bool IsCanTransfer(string fileType)
        {
            switch (fileType.Trim().ToLower())
            {
                case "txt": return true;
                default: return false;
            }
        }

        private Task CommonTransfer(string fileType, string filePath, string outputPath)
        {
            if (!IsCanTransfer(fileType))
            {
                return null;
            }
            return Task.Factory.StartNew(() => _transferService.FileTransfer(filePath, outputPath));
        }

        private class InstanceClass
        {
            public static CommonTransferEventHandler Instance = new CommonTransferEventHandler();
        }
    }
}
