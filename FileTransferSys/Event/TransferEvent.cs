using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    public delegate Task TransferEvent(string fileType, string filePath, string outputPath);
}
