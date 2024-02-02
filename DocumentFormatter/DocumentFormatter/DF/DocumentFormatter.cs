using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;
using DocumentFormatter.DF;

namespace DocumentFormatter.DocumentFormatter
{
    /// <summary>
    /// Document formatter class object
    /// </summary>
    public class DocumentFormatter
    {
        private string InputFilePath;
        
        private int MaxLengthOfName;
        private int MaxLengthOfFavSports;
        private int MaxLengthOfFavSportsIcon;
        private int MaxLengthOfState;

        /// <summary>
        /// Document formatter public constructor
        /// </summary>
        /// <param name="inputFilePath">Input file path of document.</param>
        public DocumentFormatter(string inputFilePath) 
        {
            InputFilePath = inputFilePath;
        }

        /// <summary>
        /// The entry method for document formating.
        /// </summary>
        /// <returns>Result will contain :-
        ///         Successfull document formating:
        ///             1. Path of the newly created formatted document
        ///             2. Red flag document path with missing values
        ///          Unsuccessfull response:
        ///             1. Document path is invalid
        ///             2. Message for unsuccessfull file creation
        /// "</returns>
        public string Format()
        {
            string result = string.Empty;
            if (FileOperations.ValidateFilePath(InputFilePath))
            {
                string[] contents = FileOperations.RetrieveContentsFromDocument(InputFilePath);
                Collection<LineDetails> lineInfomation = ModelDocumentContent(contents);
                if (lineInfomation == null || lineInfomation.Count == 0)
                {
                    result = "Unable to format the document content.";
                }
                else
                {
                    result = WriteLineDetailsToNewFile(lineInfomation);
                }
            }
            else
            {
                result = "Verify input file path!";
            }

            return result;
        }

        /// <summary>
        /// Model document details to LineDetails collection.
        /// </summary>
        /// <param name="contents">Contents from document.</param>
        /// <returns>Modeled LineDetails collection.</returns>
        private Collection<LineDetails> ModelDocumentContent(string[] contents)
        {
            Collection<LineDetails> lineDetails = new Collection<LineDetails>();
            string redflagFile = FileOperations.CreateFile(Path.GetDirectoryName(InputFilePath), ConfigurationManager.AppSettings["redflagFileName"]);
            if (string.IsNullOrEmpty(redflagFile))
                return null;
            else
            {
                foreach (string content in contents)
                {
                    string[] info = content.Split(' ');
                    if (info.Length != 4)
                    {
                        using (StreamWriter sw = File.CreateText(redflagFile))
                        {
                            sw.WriteLine(content);
                        }
                        continue;
                    }

                    MaxLengthOfName = Math.Max(info[0].Length, MaxLengthOfName);
                    MaxLengthOfFavSports = Math.Max(info[1].Length, MaxLengthOfFavSports);
                    MaxLengthOfFavSportsIcon = Math.Max(info[2].Length, MaxLengthOfFavSportsIcon);
                    MaxLengthOfState = Math.Max(info[3].Length, MaxLengthOfState);

                    lineDetails.Add(new LineDetails
                    {
                        Name = info[0],
                        FavouriteSports = info[1],
                        FavouriteSportsIcon = info[2],
                        State = info[3]
                    });
                }

                return lineDetails;
            }
        }

        /// <summary>
        /// Creating new file for adding formatted content.
        /// </summary>
        /// <param name="lineDetails">Details of each line from input document.</param>
        /// <returns>Result on formatted content.</returns>
        private string WriteLineDetailsToNewFile(Collection<LineDetails> lineDetails)
        {
            string result = string.Empty;
            string formattedContentsFilePath = FileOperations.CreateFile(Path.GetDirectoryName(InputFilePath), ConfigurationManager.AppSettings["fileNameForStoringFormattedContets"]);

            if (string.IsNullOrEmpty(formattedContentsFilePath))
                result = "File creation error!";
            else
            {
                result = AddContentsToFile(lineDetails, formattedContentsFilePath);
            }

            return result;
        }

        /// <summary>
        /// Format content and append to file.
        /// </summary>
        /// <param name="lineDetails">Details of each line from input document.</param>
        /// <param name="newFilePath">File path of Formatted content storage.</param>
        /// <returns></returns>
        private string AddContentsToFile(Collection<LineDetails> lineDetails, string newFilePath)
        {
            int bufferPadRightSpaceCount = Int32.Parse(ConfigurationManager.AppSettings["bufferPadRightSpaceCount"]);

            using (StreamWriter sw = File.CreateText(newFilePath))
            {
                foreach (LineDetails person in lineDetails)
                {
                    sw.Write(person.Name.PadRight(MaxLengthOfName + bufferPadRightSpaceCount));
                    sw.Write(person.FavouriteSports.PadRight(MaxLengthOfFavSports + bufferPadRightSpaceCount));
                    sw.Write(person.FavouriteSportsIcon.PadRight(MaxLengthOfFavSportsIcon + bufferPadRightSpaceCount));
                    sw.WriteLine(person.State.PadRight(MaxLengthOfState + bufferPadRightSpaceCount));
                }
            }

            return "Document is formated and is available at " + newFilePath + ". \n" + "RedFlagDocument.txt is available at same location.";
        }
    }
}
