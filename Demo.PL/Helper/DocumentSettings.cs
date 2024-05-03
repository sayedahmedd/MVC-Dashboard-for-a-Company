namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            // 1 . Get Located Folder Path 
            // string folderPath = "E:\\Route\\05 MVC\\Session 03\\Demo .Net6\\Demo 03\\Demo.PL\\wwwroot\\files\\" + folderName;
            // string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files" , folderName);

            // 2. Get File name and make it UNIQE 
            string fileName = $"{Guid.NewGuid()}{ file.FileName}"; 

            // 3. Get File path 
            string filePath = Path.Combine(folderPath, fileName);

            // 4.Save File as Streams [Data per Time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }


    }
}
