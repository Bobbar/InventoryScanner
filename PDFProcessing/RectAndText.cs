using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace InventoryScanner.PDFProcessing
{
    public class RectAndText
    {
        public Rectangle Rect;
        public string Text;

        public RectAndText(Rectangle rect, string text)
        {
            Rect = rect;
            Text = text;
        }
    }
}
