using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FileTransferSys
{
    public static class TransferServiceFactory
    {
        public static ITransferService GetTransferService(string fileType)
        {
            switch (fileType.ToLower())
            {
                case "mp4":
                case "avi":
                case "mkv": return VideoTransferService.GetInstance();
                case "jpg":
                case "png": return ImageTransferService.GetInstance();
                default: return CommonTransferService.GetInstance();
            }
        }

        public static ObserveHandler GetObserveHandler()
        {
            var observeHandler = ObserveHandler.GetInstance();
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
                        observeHandler.OnTransfer += (invokeResult as ITransferEventHandler).TransferEvent;
                    }
                }
            }
            return observeHandler;
        }
    }
}
