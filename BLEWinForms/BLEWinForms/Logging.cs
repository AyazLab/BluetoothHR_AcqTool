using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLEWinForms
{
    class Logging
    {
        private StreamWriter PCsvFile = null;
        private StreamWriter MarkerFile = null;
        private String FileName;
        private String Delimeter = ",";
        private Boolean FirstWrite = true;

        public Logging(String fileName, String delimeter)
        {
            Delimeter = delimeter;
            FileName = fileName;
            try
            {
                PCsvFile = new StreamWriter(FileName + ".csv", true);
                MarkerFile = new StreamWriter(FileName + "_Markers.csv", true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Save to CSV",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            WriteHeader();
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
            outStr += "MRK" + bWriteMarkerNext + Delimeter + "\n";
            markerStr = markerStr + outStr + "\tUDP\t" + inp + "\n";
            MarkerFile.Write(outStr);
            MarkerFile.Flush();
        }


        public void WriteHeader(string subject, string app_version, string device_name)
        {
            bRowInProgress = true;
            string date = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second + "." + System.DateTime.Now.Millisecond + Delimeter;
            string outStr = "Subject: " + subject + Delimeter + "Time: " + date + Delimeter + "Version: " + app_version + Delimeter + "Device: " + device_name + "\n";
            outStr += "Time, DataType, Value\n";
            PCsvFile.Write(outStr);
            PCsvFile.Flush();
            outStr = "ElapsedTime" + Delimeter + "MRK" + "\n";
            MarkerFile.Write(outStr);
            MarkerFile.Flush();
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

            /*if (bWriteMarkerNext > 0)
            {
                //outStr=outStr+"MRK" + bWriteMarkerNext + Delimeter+str;
                bWriteMarkerNext = 0;

                outStr = outStr + markerStr;
                markerStr = "";
            }
            else
            {
                outStr = outStr + Delimeter+str;
            }*/
            outStr += Delimeter + str;

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
