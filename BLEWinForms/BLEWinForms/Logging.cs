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
                PCsvFile = new StreamWriter(FileName, true);
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

        volatile string markerStr = "";

        public void WriteMarker(int inp)
        {
            while(bRowInProgress)
            {
                int x = 1;
            }
            bWriteMarkerNext = inp;
            string outStr = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second + "." + System.DateTime.Now.Millisecond + Delimeter;

            markerStr = markerStr + outStr + "\tUDP\t" + inp+"\n";

        }


        public void WriteHeader()
        {
            bRowInProgress = true;
            string outStr = "ElapsedTime" + Delimeter + "MRK" + Delimeter + "SystemTime" + Delimeter + "HR";
            PCsvFile.Write(outStr);
            PCsvFile.Flush();
            bRowInProgress = false;
        }
        public void WriteData(string str)
        {
            bRowInProgress = true;

            string outStr = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second + "." + System.DateTime.Now.Millisecond + Delimeter;

            

            //for (int i = 0; i < data.Length; i++)
            //{
            //   PCsvFile.Write(str + Delimeter);
            //}

            if (bWriteMarkerNext > 0)
            {
                outStr=outStr+"MRK" + bWriteMarkerNext + Delimeter+str;
                bWriteMarkerNext = 0;

                outStr = outStr + markerStr;
                markerStr = "";
            }
            else
            {
                outStr = outStr + Delimeter+str;
            }

            PCsvFile.Write(outStr);
            PCsvFile.Flush();


            bRowInProgress = false;
        }

        public void CloseFile()
        {
            PCsvFile.Close();
        }
    }
}
