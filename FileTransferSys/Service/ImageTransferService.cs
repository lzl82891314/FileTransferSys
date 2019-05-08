using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileTransferSys
{
    /// <summary>
    /// 图片文件转移策略
    /// </summary>
    public class ImageTransferService : ITransferService
    {
        /// <summary>
        /// 最小移动数量
        /// </summary>
        private int _minCount;

        /// <summary>
        /// 配置中心
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// 默认最小移动数量
        /// </summary>
        private const int DEFAULT_MINCOUNT = 5;

        private ImageTransferService()
        {
            _configuration = CommonService.ConfigInit(Directory.GetCurrentDirectory());
            if (!int.TryParse(_configuration["Appsettings:ImageMinCount"], out _minCount))
            {
                _minCount = DEFAULT_MINCOUNT;
            }
        }

        public static ImageTransferService GetInstance()
        {
            return InstanceClass.Instance;
        }

        /// <summary>
        /// 图片文件转移
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputPath"></param>
        public void FileTransfer(string filePath, string outputPath)
        {
            //图片转移规则为：
            //若该图片路径下存在多于X张的图片，则将此路径下的所有图片文件都转移到对应的output文件夹下，并且创建一个名称为图片所在文件夹名的文件夹存放这些图片

            //说明之前的转移中已经把该图片进行转移了，因此直接返回空即可
            if (!File.Exists(filePath))
            {
                return;
            }

            if (outputPath == null || !Directory.Exists(outputPath))
            {
                return;
            }

            //获取当前文件所在的文件夹路径
            var fileParentPath = CommonService.GetFileParentPath(filePath);
            if (fileParentPath == null || string.IsNullOrWhiteSpace(fileParentPath))
            {
                return;
            }

            var files = Directory.GetFiles(fileParentPath);
            if (files == null || files.Length == 0)
            {
                return;
            }

            //获取当前图片的扩展名
            var fileExtension = CommonService.GetFileExtension(filePath);
            var filesList = files.ToList();
            //按照逻辑，如果同类型图片数量小于X张，则不移动
            if (filesList.Count(item => CommonService.GetFileExtension(item).Equals(fileExtension)) < _minCount)
            {
                return;
            }

            //否则获取全部同类型的图片进行移动
            var moveList = filesList.FindAll(item => CommonService.GetFileExtension(item).Equals(fileExtension));
            //创建输出目录
            var outputTruePath = Path.Combine(outputPath, CommonService.GetFileFolder(filePath));
            if (!Directory.Exists(outputTruePath))
            {
                Directory.CreateDirectory(outputTruePath);
            }
            if (!Directory.Exists(outputTruePath))
            {
                throw new Exception($"输出目录【{ outputTruePath }】创建失败！");
            }

            //进行移动
            foreach (var item in moveList)
            {
                string fileName = CommonService.GetFileName(item);
                var fileOutputPath = Path.Combine(outputTruePath, fileName);
                if (!File.Exists(fileOutputPath))
                {
                    File.Move(item, fileOutputPath);
                }
            }
        }

        private static class InstanceClass
        {
            public static ImageTransferService Instance = new ImageTransferService();
        }
    }
}
