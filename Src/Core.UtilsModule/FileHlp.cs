using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Core.UtilsModule
{
    public static class FileHlp
    {
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;
                buffer = new byte[length];          
                int count;                          
                int sum = 0;                        
                
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static void AddToArchive(string archiveName, List<string> fileNames)
        {
            if (string.IsNullOrEmpty(archiveName)) throw new ArgumentNullException("Archive name can not be null.");

            using (ZipFile zip = new ZipFile())
            {
                foreach (string name in fileNames)
                {
                    zip.AddFile(name);
                }
                zip.Save(archiveName);
            }
        }

        public static byte[] AddToArchive(List<string> fileNames)
        {            
            using (ZipFile zip = new ZipFile())
            {
                foreach (string name in fileNames)
                {
                    zip.AddFile(name);
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    zip.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
