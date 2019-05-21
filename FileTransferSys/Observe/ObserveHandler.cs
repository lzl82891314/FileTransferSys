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
            var interfaceType = typeof(ITransferEventHandler);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                if (assemblyTypes == null || assemblyTypes.Length == 0)
                {
                    continue;
                }
                foreach (var assemblyType in assemblyTypes)
                {
                    if (interfaceType.IsAssignableFrom(assemblyType))
                    {
                        var method = assemblyType.GetMethod("GetInstance");
                        if (method == null)
                        {
                            continue;
                        }
                        var invokeResult = method.Invoke(null, null);
                        if (invokeResult == null || !(invokeResult is ITransferEventHandler))
                        {
                            continue;
                        }
                        OnTransfer += (invokeResult as ITransferEventHandler).TransferEvent;
                    }
                }
            }
        }

        public static ObserveHandler GetInstance()
        {
            return InstanceClass.Instance;
        }

        public void Transfer(string fileType, string filePath, string outputPath)
        {
            //普通调用
            OnTransfer?.Invoke(fileType, filePath, outputPath);

            #region 并行foreach
            //var invokeList = OnTransfer.GetInvocationList();
            //Parallel.ForEach(invokeList, invoker =>
            //{
            //    invoker.DynamicInvoke(fileType, filePath, outputPath);
            //});
            #endregion
        }

        private static class InstanceClass
        {
            public static ObserveHandler Instance = new ObserveHandler();
        }
    }
}
