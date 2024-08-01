using SANYUKT.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SANYUKT.Provider.Shared
{
    public class FileManager
    {
        public String SaveFile( Byte[] filestream, string MobileNo ="", string FileName = "")
        {

            String FolderPath = SANYUKTApplicationConfiguration.Instance.FileUploadPath + "\\PartnerDocument\\" + MobileNo.ToString();
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            String Filename = MobileNo.ToString() + "_Logo"  + Path.GetExtension(FileName);
            MemoryStream ms = new MemoryStream(filestream);
            FileStream file = new FileStream(FolderPath + "\\" + Filename, FileMode.Create, FileAccess.Write);

            ms.WriteTo(file);
            file.Close();
            ms.Close();
            return Filename;
        }
       

        public Byte[] ReadFile(string fileName, string DocumentFolderName)
        {
            String FolderPath = "";
            FolderPath = SANYUKTApplicationConfiguration.Instance.FileDownloadPath + "\\PartnerDocument\\" + DocumentFolderName.ToString();

            String FileToRead = FolderPath + "\\" + fileName;
            if (File.Exists(FileToRead))
            {
                FileStream fs = new FileStream(FileToRead, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Int64 numBytes = new FileInfo(FileToRead).Length;
                return br.ReadBytes((Int32)numBytes);
            }

            return null;
        }
    }
}
