using System;
using System.Collections.Generic;
using System.Text;

namespace FileTransferSys
{
    public interface ITransferEventHandler
    {
        TransferEvent TransferEvent { get; }

        bool IsCanTransfer(string fileType);
    }
}
