using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.Globalization;

namespace InventoryScanner.PDFProcessing
{
    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<RectAndText> myPoints = new List<RectAndText>();

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

            var startPosition = CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), this.SearchText, this.CompareOptions);

            if (startPosition < 0) return;

            var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(this.SearchText.Length).ToList();

            var firstChar = chars.First();
            var lastChar = chars.Last();

            var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
            var topRight = lastChar.GetAscentLine().GetEndPoint();

            var rect = new Rectangle(bottomLeft[Vector.I1], bottomLeft[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);

            myPoints.Add(new RectAndText(rect, this.SearchText));

            //oh boy....

        }

    }
}
