using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSys
{
    public class ObserveHandler
    {
        public event TransferEvent OnTransfer;

        private ObserveHandler()
        {

        }

        public static ObserveHandler GetInstance()
        {
            return InstanceClass.Instance;
        }

        public Task Transfer(string fileType)
        {
            var task = Task.Factory.StartNew(() =>
            {
                OnTransfer?.Invoke(fileType, "", "");
            });
            return task;
        }

        private static class InstanceClass
        {
            public static ObserveHandler Instance = new ObserveHandler();
        }
    }
}
