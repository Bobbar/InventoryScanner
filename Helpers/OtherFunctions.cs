using AdvancedDialog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace InventoryScanner.Helpers
{
    internal static class OtherFunctions
    {
        private static RichTextBox rtfBox = new RichTextBox();
        public static Stopwatch stpw = new Stopwatch();

        private static int stpwHits = 0;

        public static void StartTimer()
        {
            stpw.Stop();
            stpw.Reset();
            stpw.Start();
        }

        public static string StopTimer()
        {
            stpw.Stop();
            stpwHits++;
            string results = stpwHits + "  Stopwatch: MS:" + stpw.ElapsedMilliseconds + " Ticks: " + stpw.ElapsedTicks;
            Console.WriteLine(results);
            return results;
        }

        public static string ElapTime()
        {
            string results = stpwHits + "  Elapsed: MS:" + stpw.ElapsedMilliseconds + " Ticks: " + stpw.ElapsedTicks;
            Console.WriteLine(results);
            return results;
        }

        public static string NotePreview(string text, int maxChars = 50)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > maxChars)
                {
                    return text.Substring(0, maxChars) + "...";
                }
                else
                {
                    return text;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static DialogResult Message(string prompt, MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information, string title = null, Form parentForm = null)
        {
            SetWaitCursor(false, parentForm);
            using (var newMessage = new Dialog(parentForm))
            {
                return newMessage.DialogMessage(prompt, button, icon, title, parentForm);
            }
        }

        public static void SetWaitCursor(bool waiting, Form parentForm)
        {
            if (parentForm == null)
            {
                Application.UseWaitCursor = waiting;
                return;
            }

            if (parentForm.InvokeRequired)
            {
                var del = new Action(() => SetWaitCursor(waiting, parentForm));
                parentForm.BeginInvoke(del);
            }
            else
            {
                if (waiting)
                {
                    parentForm.UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                }
                else
                {
                    parentForm.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public static string RTFToPlainText(string rtfText)
        {
            try
            {
                if (rtfText.StartsWith("{\\rtf"))
                {
                    rtfBox.Rtf = rtfText;
                    return rtfBox.Text;
                }
                else
                {
                    return rtfText;
                }
            }
            catch (ArgumentException)
            {
                //If we get an argument error, that means the text is not RTF so we return the plain text.
                return rtfText;
            }
        }
    }
}