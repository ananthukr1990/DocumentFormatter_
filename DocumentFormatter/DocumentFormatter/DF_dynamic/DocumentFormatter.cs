using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;
using DocumentFormatter.DF;
using System.Linq;

namespace DocumentFormatter.DynamicDocumentFormatter
{
    /// <summary>
    /// Document formatter class object
    /// </summary>
    public class DynamicDocumentFormatter
    {
        private string InputFilePath;
        private Collection<string[]> DocumentContent;
        private int[] EachColumnsWordsMaxLength;

        /// <summary>
        /// Document formatter public constructor
        /// </summary>
        /// <param name="inputFilePath">Input file path of document.</param>
        public DynamicDocumentFormatter(string inputFilePath) 
        {
            InputFilePath = inputFilePath;
            this.Initilize();
        }

        private void Initilize()
        {
            this.DocumentContent = new Collection<string[]>();
        }

        /// <summary>
        /// The entry method for document formatting.
        /// </summary>
        /// <returns>Result will contain :-
        ///         Successfull document formatting:
        ///             1. Success message.
        ///             2. Path of the newly created formatted document.
        ///          Unsuccessfull response:
        ///             1. Unsuccessfull message.
        ///             2. Reason.
        /// "</returns>
        public string Format()
        {
            string result = string.Empty;
            if (FileOperations.ValidateFilePath(this.InputFilePath))
            {
                bool hasRetrieved = RetrieveDocumentContents();
                bool isReadyToFormat = hasRetrieved && FindEachColumnWordsMaxLength();
                if (!isReadyToFormat)
                {
                    result = "Unable to retrieve document contents.";
                }
                else
                {
                    string formattedContentsFilePath = CreateNewFileForFormatedContents();

                    if (string.IsNullOrEmpty(formattedContentsFilePath))
                        result = "File creation error!";
                    else
                        result = FormatAndAddContentToFile(formattedContentsFilePath);
                }
            }
            else
            {
                result = "Verify input file path!";
            }

            return result;
        }

        /// <summary>
        /// Retrieving document contents.
        /// </summary>
        /// <returns>Retrieval status.</returns>
        private bool RetrieveDocumentContents()
        {
            string[] contents = FileOperations.RetrieveContentsFromDocument(this.InputFilePath);
            if (contents == null || contents.Count() == 0)
                return false;
            else
            {
                foreach (string content in contents)
                {
                    string[] info = content.Split(' ');
                    if (info != null && info.Length > 0)
                    {
                        this.DocumentContent.Add(info);
                    }
                }
            }

            return this.DocumentContent != null && this.DocumentContent.Count > 0;
        }

        /// <summary>
        /// Finding maximum word length per column from all lines.
        /// </summary>
        /// <returns>If finding max word length is successfull.</returns>
        private bool FindEachColumnWordsMaxLength()
        {
            int maxLineContentCount = this.DocumentContent.Max(each => each.Length);
            this.EachColumnsWordsMaxLength = new int[maxLineContentCount];
            
            for (int i = 0; i < this.EachColumnsWordsMaxLength.Length; i++) 
            {
                this.EachColumnsWordsMaxLength[i] = this.DocumentContent.Max(each => each.Length > i && each[i] != null ? each[i].Length : 0);
            }

            return true;
        }

        /// <summary>
        /// Creating new file for adding formatted contents.
        /// </summary>
        /// <returns>Newly created file for storing formatted contents.</returns>
        private string CreateNewFileForFormatedContents()
        {
            string result = string.Empty;
            string formattedContentsFilePath = FileOperations.CreateFile(Path.GetDirectoryName(InputFilePath), ConfigurationManager.AppSettings["fileNameForStoringFormattedContets"]);

            return formattedContentsFilePath;
        }

        /// <summary>
        /// Format content and add to file.
        /// </summary>
        /// <param name="newFilePath">File path to store formatted content.</param>
        /// <returns></returns>
        private string FormatAndAddContentToFile(string newFilePath)
        {
            int bufferPadRightSpaceCount = Int32.Parse(ConfigurationManager.AppSettings["bufferPadRightSpaceCount"]);

            using (StreamWriter sw = File.CreateText(newFilePath))
            {
                foreach (string[] eachLineContent in this.DocumentContent)
                {
                    for (int i = 0; i < eachLineContent.Length; i++)
                    {
                        sw.Write(eachLineContent[i].PadRight(this.EachColumnsWordsMaxLength[i] + bufferPadRightSpaceCount));
                    }

                    sw.WriteLine("");
                }
            }

            return "Document is formated and is available at " + newFilePath + ". \n";
        }
    }
}
