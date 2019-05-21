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
    }
}
