using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using InventoryScanner.Data.Classes;

namespace InventoryScanner.PDFProcessing
{
    public class WorksheetProcessor
    {
        public WorksheetProcessor()
        {
        }

        public string FillWorksheet(List<ScanItem> itemList)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.ShowHelp = true;
                fileDialog.Title = "Select MUNIS Worksheet";
                fileDialog.Filter = "PDF File (.PDF)|*.pdf";
                fileDialog.FilterIndex = 1;
                fileDialog.Multiselect = false;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    return ProcessWorksheetFile(fileDialog.FileName, itemList);
                }
                return null;
            }
        }

        /// <summary>
        /// Processes the specified worksheet file. Populates the 'verified' fields with the specified tag list.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="itemList"></param>
        /// <remarks>
        /// Credits and HUGE thanks to:
        /// https://stackoverflow.com/questions/23909893/getting-coordinates-of-string-using-itextextractionstrategy-and-locationtextextr
        /// and
        /// https://stackoverflow.com/questions/3992617/itextsharp-insert-text-to-an-existing-pdf
        /// </remarks>
        private string ProcessWorksheetFile(string file, List<ScanItem> itemList)
        {
            // Create a modfied filename and path for the new PDF file.
            var fileInfo = new FileInfo(file);
            var origFilename = System.IO.Path.GetFileNameWithoutExtension(file);
            var newFilename = origFilename + "_completed.pdf";
            var newFullFilename = fileInfo.DirectoryName + @"\" + newFilename;

            // Open a reader and file stream for reading the original file and creating the new one.
            using (var pdfReader = new PdfReader(file))
            using (var fStream = new FileStream(newFullFilename, FileMode.Create, FileAccess.Write))
            {
                // Get the original document size and start a new document.
                var size = pdfReader.GetPageSizeWithRotation(1);
                var document = new Document(size);
                var writer = PdfWriter.GetInstance(document, fStream);

                document.Open();

                // New PDF content instance.
                var content = writer.DirectContent;

                // Iterate through the pages of the original document.
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    // Set font for current page.
                    //var markFont = BaseFont.CreateFont(BaseFont.SYMBOL, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    var markFont = BaseFont.CreateFont(@"C:\Windows\Fonts\wingding.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    var infoFont = BaseFont.CreateFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    content.SetColorFill(BaseColor.BLACK);
                    content.SetFontAndSize(markFont, 10);

                    // Iterate through each tag in the tag list.
                    foreach (var item in itemList)
                    {
                        // Create a new custom text location strategy instance for the current tag.
                        var strategy = new MyLocationTextExtractionStrategy(item.AssetTag);

                        // Search the page using the strategy. This will collect the coordinates
                        // of each instance of the asset tag.
                        PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        // Check for results and select the first one. (The sheets have columns with duplicate tag strings at the same Y coordinate)
                        if (strategy.TextLocations.Count > 0)
                        {
                            var location = strategy.TextLocations[0];

                            // Add a bullet point and scan info aligned with the "Verified" column.
                            int verifyColumnXOffset = 85;
                            int verifyColumnYOffset = 9;

                            content.BeginText();

                            // Bullet point.
                            content.SetFontAndSize(markFont, 10);
                            // content.ShowTextAligned(1, "·", size.Right - verifyColumnXOffset, location.Rect.Top - verifyColumnYOffset, 0);
                            content.ShowTextAligned(1, "ü", size.Right - verifyColumnXOffset, location.Rect.Top - verifyColumnYOffset, 0);

                            // Scan user and datestamp.
                            content.SetFontAndSize(infoFont, 6);
                            content.ShowTextAligned(0, item.ScanUser, size.Right - verifyColumnXOffset + 8, location.Rect.Top - 3, 0);
                            content.ShowTextAligned(0, item.Datestamp.ToString(@"MM/dd/yy HH:mm tt"), size.Right - verifyColumnXOffset + 8, location.Rect.Top - 9, 0);

                            content.EndText();
                        }
                    }

                    // Get the modified page and add it to the new document.
                    var newPage = writer.GetImportedPage(pdfReader, page);
                    content.AddTemplate(newPage, 0, 0);

                    // Start the next page.
                    document.NewPage();
                }

                // Release and cleanup.
                document.Close();
                fStream.Close();
                writer.Close();
                pdfReader.Close();

                return newFullFilename;
            }
        }
    }
}