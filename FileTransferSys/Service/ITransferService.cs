using System;
using System.Collections.Generic;
using System.Text;

namespace FileTransferSys
{
    /// <summary>
    /// 文件转移接口
    /// </summary>
    public interface ITransferService
    {
        /// <summary>
        /// 文件转移
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="outputPath">输出地址</param>
        void FileTransfer(string filePath, string outputPath);
    }
}
