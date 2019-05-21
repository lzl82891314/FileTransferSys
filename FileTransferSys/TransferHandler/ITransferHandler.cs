using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    interface ITransferHandler
    {
        Task FileTransfer(string inputPath, string outputPath);
    }
}
