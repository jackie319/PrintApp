using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PrinterQueue
    {

        public static string GetStatus(string printerName)
        {
            string path = @"win32_printer.DeviceId='" + printerName + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            var statusStr= printer.Properties["PrinterStatus"].Value.ToString();
            return statusStr;
        }

        public static PrintQueueStatus GetPrintQueue(string PrinterName)
        {
            PrintQueue printer = LocalPrintServer.GetDefaultPrintQueue();
            var isOut=printer.IsOutOfPaper;
            var isOpen = printer.IsDoorOpened;
            switch (printer.QueueStatus)
            {
                case PrintQueueStatus.PaperOut:
                    break;
            }

            LocalPrintServer pr = new LocalPrintServer();
            pr.Refresh();
            EnumeratedPrintQueueTypes[] enumerationFlags = {EnumeratedPrintQueueTypes.Local,
                                                            EnumeratedPrintQueueTypes.Connections,
                                                           };
            foreach (PrintQueue pq in pr.GetPrintQueues(enumerationFlags))
            {
                if (pq.Name == PrinterName)
                {
                    Boolean flag = pq.IsOutOfPaper;
                    return pq.QueueStatus;
                }
            }
            return PrintQueueStatus.None;
        }


        public static List<string> GetList(string printerName)
        {
            List<string> result = new List<string>();
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection coll = searcher.Get())
            {
                try
                {
                    foreach (ManagementObject printer in coll)
                    {
                        foreach (PropertyData property in printer.Properties)
                        {
                           var str=string.Format("{0}: {1}", property.Name, property.Value);
                            result.Add(str);
                        }
                    }
                }
                catch (ManagementException ex)
                {
                }
            }
            return result;
        }


    }
}
