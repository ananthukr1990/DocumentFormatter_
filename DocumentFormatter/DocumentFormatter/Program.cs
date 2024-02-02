using df = DocumentFormatter.DocumentFormatter;
using ddf = DocumentFormatter.DynamicDocumentFormatter;


namespace DocumentFormatter
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //df.DocumentFormatter documentFormatter = new df.DocumentFormatter("..\\DF\\InputOutputFiles\\Input.txt");
            ddf.DynamicDocumentFormatter documentFormatter = new ddf.DynamicDocumentFormatter("C:\\Users\\Aswathy Ananthu\\source\\repos\\DocumentFormatter\\DocumentFormatter\\DF_dynamic\\InputOutputFiles\\Input.txt");
            string outputPath = documentFormatter.Format();
        }
    }
}
