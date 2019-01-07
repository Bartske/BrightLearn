using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Abstractions;
using System.Web;
using System.Web.Hosting;


namespace Logic
{
    public class UploadHandler
    {

        private const string UPLOAD_PATH = @"Content\images\Uploads";
        private const int MAX_IMAGE_HEIGHT = 1000;
        private const int MAX_IMAGE_WIDTH = 1000;

        public UploadHandler(HttpPostedFileBase file, FileType fileType)
        {
            Type = fileType;
            File = file;
        }

        //TODO upload any type of file and handle accordenly
        public static string UploadFile()
        {
            throw new NotImplementedException();
        }

        public static string GetUploadPath()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), @"Content\images\Uploads");
            return path;
        }

        public static string GetImageUrlPath(PreDefImageLocation loc)
        {
            switch (loc)
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


        //Create Upload class with type image without specifying anything returns the url of uploaded image
        //TODO resizes images to maximum allowed size
        public static UploadHandler UploadImage(HttpPostedFileBase imageFile)
        {
            if (FileIsImage(imageFile))
            {
                return Upload(new UploadHandler(imageFile, FileType.Image));
            }
            return null;
        }

        public static UploadHandler UploadImage(HttpPostedFileBase imageFile, Point size, PreDefImageLocation location)
        {
            if (FileIsImage(imageFile))
            {
                UploadHandler uploadFile = new UploadHandler(imageFile, FileType.Image);
                uploadFile.Size = size;
                uploadFile.ImageFolder = GetDefaultImageLocation(location);
                return Upload(uploadFile);
            }
            return null;
        }

        public static UploadHandler UploadCropImage(HttpPostedFileBase imageFile, Point size)
        {

            if (FileIsImage(imageFile))
            {
                UploadHandler uploadFile = new UploadHandler(imageFile, FileType.Image);
                uploadFile.Size = size;
                uploadFile.isCrop = true;
                return Upload(uploadFile);
            }
            return null;
        }

        private static bool FileIsImage(HttpPostedFileBase imageFile)
        {
            string type = string.Join("", imageFile.ContentType.Take(5).ToArray());
            if (type == "image")
            {
                return true;
            }
            return false;
        }

        //Uploads the provided UploadHandler class
        private static UploadHandler Upload(UploadHandler uploadFile)
        {
            if (uploadFile.Type == FileType.Image)
            {
                if (uploadFile.Name == null)
                {
                    uploadFile.Name = DateTime.Now.ToString("yyyMMddHHmm");
                }
                
                uploadFile.Folder = Path.Combine(uploadFile.PathToRoot, UPLOAD_PATH, uploadFile.Folder);
                uploadFile.Folder += uploadFile.ImageFolder;
                Directory.CreateDirectory(uploadFile.Folder);

                uploadFile.Url = Path.Combine(uploadFile.PathToRoot, uploadFile.Folder, uploadFile.Name);
                int count = 0;
                while (System.IO.File.Exists(uploadFile.Url))
                {
                    count++;
                    uploadFile.Url = Path.Combine(uploadFile.PathToRoot, uploadFile.Folder, uploadFile.Name + "(" + count.ToString() + ")");
                }
                if (count > 0)
                {
                    uploadFile.Name += "(" + count.ToString() + ").jpeg";
                }
                else
                {
                    uploadFile.Name += ".jpeg";
                }

                uploadFile.Url += ".jpeg";
                uploadFile.Extention = ".jpeg";


                Image image = Image.FromStream(uploadFile.File.InputStream, true, true);
                Point newImageSize = new Point(image.Width, image.Height);

                if (uploadFile.isCrop)
                {
                    int widthDiff = newImageSize.X - uploadFile.Size.X;
                    int heigthDiff = newImageSize.Y - uploadFile.Size.Y;
                    int ratio = 0;

                    if (widthDiff > heigthDiff || widthDiff == heigthDiff)
                    {
                        ratio = uploadFile.Size.Y / newImageSize.Y;
                    }
                    else
                    {
                        ratio = uploadFile.Size.X / newImageSize.X;
                    }
                    newImageSize.X = uploadFile.Size.X * ratio;
                    newImageSize.Y = uploadFile.Size.Y * ratio;
                }
                else if (uploadFile.Size.X > 0 && uploadFile.Size.Y > 0)
                {
                    newImageSize = uploadFile.Size;

                }


                var newImage = new Bitmap(newImageSize.X, newImageSize.Y);
                using (var g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, uploadFile.Crop.X, uploadFile.Crop.Y, newImageSize.X, newImageSize.Y);
                }

                Image uploadImage = newImage;
                uploadImage.Save(uploadFile.Url, ImageFormat.Jpeg);

            }
            return uploadFile;

        }

        public static Point GetImageSize(PreDefImageSize preDefImageSize)
        {
            switch (preDefImageSize)
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

        public static string GetDefaultImageLocation(PreDefImageLocation preDefImageLocation)
        {
            switch (preDefImageLocation)
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
