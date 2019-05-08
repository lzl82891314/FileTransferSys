using System;

namespace FileTransferSys
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new FileTransferHandler();
            handler.FileTransfer();
        }
    }
}
