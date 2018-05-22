using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Windows.Forms;

namespace InventoryScanner.PDFProcessing
{
    public class WorksheetProcessor
    {
        public WorksheetProcessor() { }


        public void LoadWorksheet()
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
                    ProcessWorksheetFile(fileDialog.FileName);
                }
            }
        }

        private void ProcessWorksheetFile(string file)
        {
            var fileInfo = new FileInfo(file);
            var origFilename = System.IO.Path.GetFileNameWithoutExtension(file);
            var newFilename = origFilename + "_modified.pdf";
            var newFullFilename = fileInfo.DirectoryName + @"\" + newFilename;

            using (var pdfReader = new PdfReader(file))
            using (var fs = new FileStream(newFullFilename, FileMode.Create, FileAccess.Write))
            {
                var size = pdfReader.GetPageSizeWithRotation(1);
                var document = new Document(size);
                var writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                var content = writer.DirectContent;

                var bFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                content.SetColorFill(BaseColor.BLACK);
                content.SetFontAndSize(bFont, 10);



                //for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                //{

                //var strategy = new SimpleTextExtractionStrategy();
                int page = 2;
                var strategy = new MyLocationTextExtractionStrategy("11600");

                var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                foreach (var p in strategy.myPoints)
                {
                    Console.WriteLine(p.Text + " found at: " + p.Rect.Left + "x" + p.Rect.Bottom);

                    content.BeginText();
                    content.ShowTextAligned(1, "X", size.Right - 60, p.Rect.Top - 10, 0);
                    content.EndText();

                }

                var newPage = writer.GetImportedPage(pdfReader, page);
                content.AddTemplate(newPage, 0, 0);

                //currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                //Console.WriteLine(currentText);

                //  sb.Append(currentText);




                //}
                document.Close();
                fs.Close();
                writer.Close();
                pdfReader.Close();

                //Console.WriteLine(sb.ToString());




                //StringBuilder sb = new StringBuilder();
                //// var de = new KeyValuePair<string, AcroFields.Item>();


                //foreach (KeyValuePair<string, AcroFields.Item> de in pdfReader.AcroFields.Fields)
                //{
                //    sb.Append(de.Key.ToString() + Environment.NewLine);
                //}

                //Console.WriteLine(sb.ToString());
            }
        }



    }
}
