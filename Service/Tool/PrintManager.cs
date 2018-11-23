﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PrintManager
    {
        /// <summary>
        /// 获取打印机的当前状态
        /// </summary>
        /// <param name="PrinterDevice">打印机设备名称</param>
        /// <returns>打印机状态</returns>
        public static PrinterStatus GetPrinterStat(string PrinterDevice)
        {
            PrinterStatus ret = 0;
            string path = @"win32_printer.DeviceId='" + PrinterDevice + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            var status =printer.Properties["PrinterStatus"].Value;
            ret = (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
            return ret;
        }

     
    }

   public enum PrinterStatus
    {
        其他状态 = 1,
        未知,
        空闲,
        正在打印,
        预热,
        停止打印,
        打印中,
        离线
    }
}
