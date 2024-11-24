﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationUtilYwh.Communication;

namespace CommunicationUtilYwh.Device
{
    public class Scanner_RS232:SerialCommunication
    {
        private string TriggerCmd = "";

        public Scanner_RS232(SerialPort serialPort) :base(serialPort)
        {
            
        }

        public void Trigger()
        {
            this.SendData(TriggerCmd);
        }

        public string GetResult()
        {
           return ReadStr();
        }
    }
}
