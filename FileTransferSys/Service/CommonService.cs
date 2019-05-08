using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace FileTransferSys
{
    /// <summary>
    /// 通用服务
    /// </summary>
    public static class CommonService
    {
        public static IConfiguration ConfigInit(string path)
        {
            var configBuilder = new ConfigurationBuilder();
            var configFile = Directory.EnumerateFiles(path).FirstOrDefault(item => item.EndsWith("appsettings.json"));
            if (string.IsNullOrWhiteSpace(configFile))
            {
                var parentPath = GetFileParentPath(path);
                var parentConfigFile = Directory.EnumerateFiles(parentPath).FirstOrDefault(item => item.EndsWith("appsettings.json"));
                if (string.IsNullOrWhiteSpace(parentConfigFile))
                {
                    throw new ArgumentNullException($"appsettings.json文件不存在！");
                }
                return configBuilder.AddJsonFile(parentConfigFile).Build();
            }
            return configBuilder.AddJsonFile(configFile).Build();
        }

        public static string GetFileFolder(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            var splitedArr = filePath.Split('\\');
            if (splitedArr == null || splitedArr.Length < 2)
            {
                return null;
            }
            return splitedArr[splitedArr.Length - 2];
        }

        public static string GetFileParentPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            var splitedArr = filePath.Split('\\');
            if (splitedArr == null || splitedArr.Length == 0)
            {
                return null;
            }
            var splitedList = splitedArr.ToList();
            splitedList.RemoveAt(splitedList.Count - 1);
            return string.Join("\\", splitedList);
        }

        public static string GetFileName(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            var splitedArr = filePath.Split('\\');
            if (splitedArr == null || splitedArr.Length == 0)
            {
                return null;
            }
            return splitedArr[splitedArr.Length - 1];
        }

        public static string GetFileExtension(string filePath)
        {
            var fileName = GetFileName(filePath);
            if (fileName == null)
            {
                return null;
            }
            var splitedArr = fileName.Split('.');
            if (splitedArr == null || splitedArr.Length < 2)
            {
                return null;
            }

            return splitedArr[splitedArr.Length - 1].ToLower();
        }
    }
}
