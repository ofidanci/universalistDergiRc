using System.IO;
using UniversalistDergiRC.DataAccess;
using UniversalistDergiRC.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileOperations))]
namespace UniversalistDergiRC.Droid
{
    public class FileOperations : IFileOperations
    {
        public void SaveText(string filename, string text)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }

        public string ReadAllText(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            if (File.Exists(filePath))
                return File.ReadAllText(filePath);
            return string.Empty;
        }

        public string[] LoadFileLines(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            if (File.Exists(filePath))
                return File.ReadAllLines(filePath);
            return new string[] { string.Empty };
        }
    }

}