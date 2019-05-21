using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FileTransferSys
{
    class Program
    {
        /// <summary>
        /// 当前路径地址
        /// </summary>
        private static string _configPath = Directory.GetCurrentDirectory();

        /// <summary>
        /// 配置中心
        /// </summary>
        private static IConfiguration _configuration = CommonService.ConfigInit(_configPath);

        static void Main(string[] args)
        {
            var inputPath = _configuration["Appsettings:InputPath"];
            var outputPath = _configuration["Appsettings:OutputPath"];

            ITransferHandler handler = new FileTransferDefaultHandler();
            handler.FileTransfer(inputPath, outputPath).Wait();

            //var observe = ObserveHandler.GetInstance();
            Console.ReadKey();
        }
    }
}
