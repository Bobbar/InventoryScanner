﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace InventoryScanner.PDFProcessing
{
    public class WorksheetProcessor
    {
        public WorksheetProcessor()
        {
        }

        public void LoadWorksheet(List<string> tagList)
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
                    ProcessWorksheetFile(fileDialog.FileName, tagList);
                }
            }
        }

        /// <summary>
        /// Processes the specified worksheet file. Populates the 'verified' fields with the specified tag list.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tagList"></param>
        /// <remarks>
        /// Credits and HUGE thanks to: 
        /// https://stackoverflow.com/questions/23909893/getting-coordinates-of-string-using-itextextractionstrategy-and-locationtextextr
        /// and
        /// https://stackoverflow.com/questions/3992617/itextsharp-insert-text-to-an-existing-pdf
        /// </remarks>
        private void ProcessWorksheetFile(string file, List<string> tagList)
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
                    var bFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    content.SetColorFill(BaseColor.BLACK);
                    content.SetFontAndSize(bFont, 10);

                    // Iterate through each tag in the tag list.
                    foreach (var assetTag in tagList)
                    {
                        // Create a new custom text location strategy instance for the current tag.
                        var strategy = new MyLocationTextExtractionStrategy(assetTag);

                        // Search the page using the strategy. This will collect the coordinates
                        // of each instance of the asset tag.
                        PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        // Iterate through the coordinates collected by the strategy.
                        foreach (var p in strategy.myPoints)
                        {
                            //Console.WriteLine(p.Text + " found at: " + p.Rect.Left + "x" + p.Rect.Bottom);

                            // Add an "X" alined with the 'Verified' column on the worksheet.
                            content.BeginText();
                            content.ShowTextAligned(1, "X", size.Right - 60, p.Rect.Top - 10, 0);
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
            }
        }
    }
}