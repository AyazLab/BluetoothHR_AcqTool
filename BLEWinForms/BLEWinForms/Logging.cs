using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLEWinForms
{
    class Logging
    {
        private StreamWriter PCsvFile = null;
        private String FileName;
        private String Delimeter = ",";
        private Boolean FirstWrite = true;

        public Logging(String fileName, String delimeter)
        {
            Delimeter = delimeter;
            FileName = fileName;
            try
            {
                PCsvFile = new StreamWriter(FileName, false);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Save to CSV",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        volatile bool bRowInProgress = false;
        volatile int bWriteMarkerNext = 0;
        public long mrkReceivedTime = 0;

        public void WriteMarker(int inp)
        {
            bWriteMarkerNext = inp;

        }



        public void WriteData(String str)
        {
            bRowInProgress = true;

            PCsvFile.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second + "." + System.DateTime.Now.Millisecond + Delimeter);

            //for (int i = 0; i < data.Length; i++)
            //{
            //   PCsvFile.Write(str + Delimeter);
            //}

            if (bWriteMarkerNext > 0)
            {
                PCsvFile.Write("MRK" + bWriteMarkerNext + Delimeter);
                bWriteMarkerNext = 0;
            }
            else
            {
                PCsvFile.Write(Delimeter);
            }

            PCsvFile.WriteLine();


            bRowInProgress = false;
        }

        public void CloseFile()
        {
            PCsvFile.Close();
        }
    }
}
