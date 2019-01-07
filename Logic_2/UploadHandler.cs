using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Drawing.Imaging;

namespace Logic
{
    public class UploadHandler
    {

        private const string UploadPath = @"Content\images\Uploads";
        private const int MaxImageHeight = 1000;
        private const int MaxImageWidth = 1000;

        public UploadHandler(HttpPostedFileBase File, FileType FileType)
        {
            Type = FileType;
            this.File = File;
        }

        //TODO upload any type of file and handle accordenly
        public static string UploadFile()
        {
            throw new NotImplementedException();
        }

        public static string GetUploadPath()
        {
            string Path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), @"Content\images\Uploads");
            return Path;
        }

        public static string GetImageUrlPath(PreDefImageLocation ImageLocation)
        {
            switch (ImageLocation)
            {
                case PreDefImageLocation.GameImage:
                    return @"Content/images/Uploads/GameImage/";
                case PreDefImageLocation.QuestionImage:
                    return @"Content/images/Uploads/QuestionImage/";
                default:
                    break;
            }
            return "";
        }
        
        public static UploadHandler UploadImage(HttpPostedFileBase ImageFile)
        {
            if (FileIsImage(ImageFile))
            {
                return Upload(new UploadHandler(ImageFile, FileType.Image));
            }
            return null;
        }

        public static UploadHandler UploadImage(HttpPostedFileBase ImageFile, Point Size, PreDefImageLocation Location)
        {
            if (FileIsImage(ImageFile))
            {
                UploadHandler uploadFile = new UploadHandler(ImageFile, FileType.Image);
                uploadFile.Size = Size;
                uploadFile.ImageFolder = GetDefaultImageLocation(Location);
                return Upload(uploadFile);
            }
            return null;
        }

        public static UploadHandler UploadCropImage(HttpPostedFileBase ImageFile, Point Size)
        {

            if (FileIsImage(ImageFile))
            {
                UploadHandler uploadFile = new UploadHandler(ImageFile, FileType.Image);
                uploadFile.Size = Size;
                uploadFile.isCrop = true;
                return Upload(uploadFile);
            }
            return null;
        }

        private static bool FileIsImage(HttpPostedFileBase ImageFile)
        {
            string Type = string.Join("", ImageFile.ContentType.Take(5).ToArray());
            if (Type == "image")
            {
                return true;
            }
            return false;
        }

        //Uploads the provided UploadHandler class
        private static UploadHandler Upload(UploadHandler UploadFile)
        {
            if (UploadFile.Type == FileType.Image)
            {
                if (UploadFile.Name == null)
                {
                    UploadFile.Name = DateTime.Now.ToString("yyyMMddHHmm");
                }
                
                UploadFile.Folder = Path.Combine(UploadFile.PathToRoot, UploadPath, UploadFile.Folder);
                UploadFile.Folder += UploadFile.ImageFolder;
                Directory.CreateDirectory(UploadFile.Folder);

                UploadFile.Url = Path.Combine(UploadFile.PathToRoot, UploadFile.Folder, UploadFile.Name);
                int count = 0;
                while (System.IO.File.Exists(UploadFile.Url))
                {
                    count++;
                    UploadFile.Url = Path.Combine(UploadFile.PathToRoot, UploadFile.Folder, UploadFile.Name + "(" + count.ToString() + ")");
                }
                if (count > 0)
                {
                    UploadFile.Name += "(" + count.ToString() + ").jpeg";
                }
                else
                {
                    UploadFile.Name += ".jpeg";
                }

                UploadFile.Url += ".jpeg";
                UploadFile.Extention = ".jpeg";


                Image Image = Image.FromStream(UploadFile.File.InputStream);
                Point NewImageSize = new Point(Image.Width, Image.Height);

                if (UploadFile.isCrop)
                {
                    int widthDiff = NewImageSize.X - UploadFile.Size.X;
                    int heigthDiff = NewImageSize.Y - UploadFile.Size.Y;
                    int ratio = 0;

                    if (widthDiff > heigthDiff || widthDiff == heigthDiff)
                    {
                        ratio = UploadFile.Size.Y / NewImageSize.Y;
                    }
                    else
                    {
                        ratio = UploadFile.Size.X / NewImageSize.X;
                    }
                    NewImageSize.X = UploadFile.Size.X * ratio;
                    NewImageSize.Y = UploadFile.Size.Y * ratio;
                }
                else if (UploadFile.Size.X > 0 && UploadFile.Size.Y > 0)
                {
                    NewImageSize = UploadFile.Size;

                }


                var newImage = new Bitmap(NewImageSize.X, NewImageSize.Y);
                using (var g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(Image, UploadFile.Crop.X, UploadFile.Crop.Y, NewImageSize.X, NewImageSize.Y);
                }

                Image uploadImage = newImage;
                uploadImage.Save(UploadFile.Url, ImageFormat.Jpeg);

            }
            return UploadFile;

        }

        public static Point GetImageSize(PreDefImageSize PreDefImageSize)
        {
            switch (PreDefImageSize)
            {
                case PreDefImageSize.GameImage:
                    return new Point(230, 110);
                case PreDefImageSize.QuestionImage:
                    return new Point(1920, 1080);
                default:
                    break;
            }
            return new Point();
        }

        public static string GetDefaultImageLocation(PreDefImageLocation PreDefImageLocation)
        {
            switch (PreDefImageLocation)
            {
                case PreDefImageLocation.GameImage:
                    return "\\GameImage\\";
                case PreDefImageLocation.QuestionImage:
                    return "\\QuestionImage\\";
                default:
                    break;
            }
            return "";
        }

        public FileType Type;
        public string Extention;
        public string Name;
        public string Url;
        public string ImageFolder = "";
        public string Folder = "";
        public string PathToRoot = HttpRuntime.AppDomainAppPath;
        public Point Size;
        public Point Crop = new Point(0, 0);
        public bool isCrop = false;
        public int FileSize;
        public HttpPostedFileBase File;
    }

    public enum FileType { Image, Pdf };
    public enum PreDefImageSize { GameImage, QuestionImage };
    public enum PreDefImageLocation { GameImage, QuestionImage };
}
