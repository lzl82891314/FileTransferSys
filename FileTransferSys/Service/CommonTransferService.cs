using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTransferSys
{
    /// <summary>
    /// 通用文件转移策略
    /// </summary>
    public class CommonTransferService : ITransferService
    {
        private CommonTransferService()
        {
        }
        public static CommonTransferService GetInstance()
        {
            return InstanceClass.Instance;
        }

        /// <summary>
        /// 用用文件转移
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

            string outputFilePath = Path.Combine(outputPath, CommonService.GetFileName(filePath));
            File.Move(filePath, outputFilePath);
        }

        private static class InstanceClass
        {
            public static CommonTransferService Instance = new CommonTransferService();
        }
    }
}
