using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFormatter.DF
{
    /// <summary>
    /// Class containing file operations
    /// </summary>
    internal static class FileOperations
    {
        /// <summary>
        /// To validate a given file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>If file path is valid.</returns>
        internal static bool ValidateFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Read document details.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Document details as a string of array with respect to each line.</returns>
        internal static string[] RetrieveContentsFromDocument(string filePath)
        {
            string[] contents = File.ReadAllLines(filePath);
            return contents;
        }

        /// <summary>
        /// Creating a new file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static string CreateNewFile(string filePath)
        {
            string newFileAppendName = ConfigurationManager.AppSettings["newFileName"];

            //string currentFilePath = System.IO.Path.ChangeExtension(filePath, null);

            string newFilePath = Path.GetDirectoryName(filePath) + "\\" + newFileAppendName;

            try
            {
                File.Create(newFilePath).Dispose();
            }
            catch (Exception ex) 
            {
                newFilePath = string.Empty;
            }

            return newFilePath;
        }

        /// <summary>
        /// Create a new file.
        /// </summary>
        /// <param name="directoryPath">Directory name.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>The full path of created file.</returns>
        internal static string CreateFile(string directoryPath, string fileName)
        {
            string newFilePath = directoryPath + "\\" + fileName;
            File.Create(newFilePath).Dispose();

            return newFilePath;
        }
    }
}
