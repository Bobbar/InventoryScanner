using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace InventoryScanner.PDFProcessing
{
    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<RectAndText> TextLocations = new List<RectAndText>();

        public string SearchText { get; set; }

        public CompareOptions CompareOptions { get; set; }

        public MyLocationTextExtractionStrategy(string searchText, CompareOptions compareOptions = CompareOptions.None)
        {
            SearchText = searchText;
            CompareOptions = compareOptions;
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            // Get the current block of text.
            var currentText = renderInfo.GetText();

            // Compare the current block of text to our search text and get the start position.
            var startPosition = CultureInfo.CurrentCulture.CompareInfo.IndexOf(currentText, this.SearchText, this.CompareOptions);

            // If no start position or the current block does not match our search text, return silently.
            // The second condition ensures that duplicate locations are not returned for
            // very similar text values. For example: "1234" exists within the block 
            // of text "12345", but is not actually a match.
            //
            if (startPosition < 0 || currentText != SearchText) return;

            // Get the list of char values in the block of text.
            var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(this.SearchText.Length).ToList();

            // Select the first and last chars.
            var firstChar = chars.First();
            var lastChar = chars.Last();

            // Get the two corner vectors of the rectangle containing the text.
            var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
            var topRight = lastChar.GetAscentLine().GetEndPoint();

            // Create a new rectangle instance from the vector data.
            var rect = new Rectangle(bottomLeft[Vector.I1], bottomLeft[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);

            // Create a new container instance and add it to the collection.
            TextLocations.Add(new RectAndText(rect, this.SearchText));

            //oh boy....
        }
    }
}