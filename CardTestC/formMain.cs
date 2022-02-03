using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Forms;

namespace CardTestC
{

    public partial class formMain : Form
    {
        public static Module.CommParameterType CommParam = new Module.CommParameterType();        //User type definition
        static bool DataReceived;
        static bool CancelFlag;
        public static SerialPort commPort = new SerialPort();
        //---------------------------------------------------------------------------
        //       initialize main form 
        //---------------------------------------------------------------------------
        public formMain()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = Properties.Resources.magcard;

            string Title = "Next2020";          //application title
//            string Version = "Version 1.0";     //application software version
//            string MDate = "08/21/2020";        //application creation date
            string Caption = Title;

            textMonitor.Text = Module.MSG_Communication_Monitor_Default;        //default message
            optionHex.Checked = true;                                           //default to hex view
            optionHex.Visible = false;
            optionASCII.Visible = false;

            //labels
            labelCommPort.Text = "Available Ports";
            labelTrack1.Text = "Track 1";
            labelTrack2.Text = "Track 2";
            labelTrack3.Text = "Track 3";
            labelQuickSelect.Text = "Quick Select";

            //add items to quick select dropdown
            comboQuickSelect.Items.Add("Ones Card");
            comboQuickSelect.Items.Add("Twos Card");
            comboQuickSelect.Items.Add("Nines Card");
            comboQuickSelect.Items.Add("Custom Card");

            //set initial control states to disabled
            cmdRead.Enabled = false;
            cmdWrite.Enabled = false;
            cmdCancel.Enabled = false;
            cmdTest.Visible = false;
            textTrack1.Enabled = false;
            textTrack2.Enabled = false;
            textTrack3.Enabled = false;
            comboQuickSelect.Enabled = false;

            //initialize user defined types
            CommParam.Port = "COM1";             //default port number
            CommParam.Baudrate = 9600;           //9600 bpi
            CommParam.Parity = Parity.Even;      //even parity
            CommParam.BitLength = 7;             //7 data bits
            CommParam.StopBit = StopBits.One;    //1 stop bit
            CommParam.RTSEnable = true;          //enable RTS line
            CommParam.DTREnable = true;          //enable DTR line
            CommParam.STXEnable = true;          //enable STX code
            CommParam.ETXEnable = true;          //enable ETX code
            CommParam.STX = char.ConvertFromUtf32((int)0x02);            //STX character code (02h)
            CommParam.US = char.ConvertFromUtf32((int)0x1F);             //US character code (1Fh)
            CommParam.ETX = char.ConvertFromUtf32((int)0x03);            //ETX character code (03h)
            CommParam.BCCByte = 1;               //BCC length ( 1 byte BCC)
            CommParam.BCCRange = 1;              //BCC calculate start position (Next STX)
            CommParam.TimeOut = 60000;           //communication response wait time out (msec)

            GetSerialPortNames();                //load available serial ports
            comboPorts.Focus();                  //set focus
        }

        //---------------------------------------------------------------------------
        //       initialize and open port 
        //---------------------------------------------------------------------------
        private void comboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommParam.Port = comboPorts.Text;               //port name
            CommClose();                                    //close port if open
            CommOpen();                                     //open port

            string CommandStr = Module.CMD_Status;          //status command
            CommandStr = Functions.SetSendBlock(CommandStr);          //add STX,ETX,BCC
            ShowMonitor(CommandStr, "TX:");                 //print to monitor
            SendSerialData(CommandStr);                     //send command
            byte[] returnString = ReceiveSerialData();      //get response
            string newReturnString = System.Text.Encoding.UTF8.GetString(returnString);     //convert to ASCII

            if (newReturnString != "") {                    //response received
                Process_Data_Received(newReturnString);     //process response
                cmdRead.Enabled = true;                     //enable read button
                comboQuickSelect.Enabled = true;            //enable quick select dropdown
            } else {                                        //no response, error message, disable controls
                MessageBox.Show("No device found on " + CommParam.Port + ". Make sure device is connected or try another port.", "Houston we have a problem.", MessageBoxButtons.OK);
                labelMessage.Text = "Message : No device on " + CommParam.Port;
                cmdRead.Enabled = false;
                cmdWrite.Enabled = false;
                comboQuickSelect.Enabled = false;
                cmdClear.PerformClick();
                textTrack1.Enabled = false;
                textTrack2.Enabled = false;
                textTrack3.Enabled = false;
                comboQuickSelect.Text = "";
            }
            formMain.DataReceived = false;                    //reset data flag
        }

        //---------------------------------------------------------------------------
        //       quick select dropdown options 
        //---------------------------------------------------------------------------
        private void comboQuickSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboQuickSelect.Text) {
                case "Ones Card":
                    textTrack1.Text = "";
                    textTrack2.Text = "99999991";
                    textTrack3.Text = "";
                    textTrack1.Enabled = false;
                    textTrack2.Enabled = false;
                    textTrack3.Enabled = false;
                    break;
                case "Twos Card":
                    textTrack1.Text = "";
                    textTrack2.Text = "99999992";
                    textTrack3.Text = "";
                    textTrack1.Enabled = false;
                    textTrack2.Enabled = false;
                    textTrack3.Enabled = false;
                    break;
                case "Nines Card":
                    textTrack1.Text = "";
                    textTrack2.Text = "99999999";
                    textTrack3.Text = "";
                    textTrack1.Enabled = false;
                    textTrack2.Enabled = false;
                    textTrack3.Enabled = false;
                    break;
                case "Custom Card":
                    textTrack1.Text = "";
                    textTrack2.Text = "";
                    textTrack3.Text = "";
                    textTrack1.Enabled = true;
                    textTrack2.Enabled = true;
                    textTrack3.Enabled = true;
                    break;
            }
            cmdRead.Enabled = true;
            cmdWrite.Enabled = true;
        }

        //---------------------------------------------------------------------------
        //       unloading main form
        //---------------------------------------------------------------------------
        private void formMain_FormClosing(Object sender, FormClosingEventArgs e)
        {
            CommClose();
        }

        //---------------------------------------------------------------------------
        //       change display monitor code to ASCII
        //---------------------------------------------------------------------------
        private void optionASCII_Click(object sender, EventArgs e)
        {
            ShowMonitor("------------Next ASCII Mode----------------", "");
        }

        //---------------------------------------------------------------------------
        //       change display monitor code to HEX
        //---------------------------------------------------------------------------
        private void optionHex_Click(object sender, EventArgs e)
        {
            ShowMonitor("------------Next HEX Mode-----------------", "");
        }

        //---------------------------------------------------------------------------
        //       clear communication monitor
        //---------------------------------------------------------------------------
        private void cmdClearMonitor_Click(object sender, EventArgs e)
        {
            textMonitor.Text = "";
        }

        //---------------------------------------------------------------------------
        //       process to get available ports and add to port dropdown
        //---------------------------------------------------------------------------
        private void GetSerialPortNames()
        {
            //show all available COM ports.
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboPorts.Items.Add(port);
            }
        }

        //---------------------------------------------------------------------------
        //       open communication port
        //---------------------------------------------------------------------------
        private void CommOpen()
        {
            try
            {
                if (commPort.IsOpen) { return; }

                string strSettings = Functions.GetCommSettings(CommParam);
                commPort.PortName = CommParam.Port;
                commPort.BaudRate = CommParam.Baudrate;
                commPort.Parity = CommParam.Parity;
                commPort.DataBits = CommParam.BitLength;
                commPort.StopBits = CommParam.StopBit;
                commPort.ReceivedBytesThreshold = 1;
                commPort.RtsEnable = CommParam.RTSEnable;
                commPort.DtrEnable = CommParam.DTREnable;
                commPort.Handshake = Handshake.None;
                commPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                commPort.Open();
                labelCommStatus.Text = Module.MSG_Comm_CommPortOpen + "\r\n" + "Comm " +
                                    commPort.PortName + " : " + strSettings;
                labelMessage.Text = "Message : Comm port open complete";

            }
            catch (Exception e)
            {
                labelMessage.Text = Module.MSG_Comm_CommSetError + e.Message;
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
            }
        }

        //---------------------------------------------------------------------------
        //       close communication port 
        //---------------------------------------------------------------------------
        private void CommClose()
        {
            try
            {
                if (!commPort.IsOpen) { return; }
                commPort.Close();
                labelCommStatus.Text = Module.MSG_Comm_CommPortClose;
            }
            catch (Exception e)
            {
                labelMessage.Text = Module.MSG_Comm_CommSetError + e.Message;
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
            }
        }

        //---------------------------------------------------------------------------
        //       communication port data listener
        //---------------------------------------------------------------------------
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //since this is on a different thread than main, sets a flag to indicate data in buffer
            //this is used to bypass thread invoking to update the form and stops the timer
            //also does not fire on acknowledgments (Messages without STX - ACK, NAK, etc.)
            if (commPort.BytesToRead > 1) {
                formMain.DataReceived = true;
                System.Diagnostics.Debug.WriteLine("Data received - " + commPort.BytesToRead);
            }
        }

        //---------------------------------------------------------------------------
        //       display RS232 communication code procedure
        //           source  :display source code
        //           mode    :send/receive mode or message mode ("SD:","RD:","")
        //---------------------------------------------------------------------------
        private void ShowMonitor(string Source, string Mode)
        {
            try
            {
                string TempStr;
                if (Mode == "") {                                           //if mode is null("") , message mode
                    TempStr = Source;
                } else {
                    if (optionASCII.Checked) {                              //choose view type by option button
                        TempStr = Mode + Functions.ChangeCharType(Source, "ASCII");   //change code sub procedure
                    } else {
                        TempStr = Mode + Functions.ChangeCharType(Source, "HEX");     //change code sub procedure
                    }
                }

                textMonitor.SelectionStart = textMonitor.Text.Length + 1;   //set display position
                textMonitor.SelectedText = "\r\n" + TempStr;
                System.Diagnostics.Debug.WriteLine(TempStr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                textMonitor.Text = "";
                return;
            }
        }

        //---------------------------------------------------------------------------
        //       get command parameter function procedure
        //           input   source  : command string
        //           output          : parameter string
        //---------------------------------------------------------------------------
        private string GetParameter(string Source)
        {
            try
            {
                string TempStr = "";

                switch(Source)
                {
                    case "Write":
                    case "Write Wait":                                              //if write command
                        TempStr = textTrack1.Text + "\r\n" + textTrack2.Text + "\r\n" + textTrack3.Text + "\r\n";
                        TempStr = TempStr.Replace("\r\n", CommParam.US);            //replace string to [US] from [CR+LF]
                        while(TempStr.EndsWith(CommParam.US)) {                     //delete string end [US] code
                            int len = TempStr.Length;
                            TempStr = TempStr.Remove(len - 1);
                        }
                        break;
                    case "Communication Check":
                    case "EEPROM Read":
                    case "EEPROM Write":
                    case "EEPROM Open":
                    case "Send other string":
                        break;

                }

                return TempStr;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return "";
            }
        }

        //---------------------------------------------------------------------------
        //       wait process procedure
        //           WaitTime    :waiting time (msec)
        //---------------------------------------------------------------------------
        public static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private void Wait(int WaitTime)
        {
            if (WaitTime == 0 || WaitTime < 0) return;

            timer.Interval = WaitTime;
            timer.Enabled = true;
            timer.Start();

            timer.Tick += (s, e) =>
            {
                timer.Enabled = false;
                timer.Stop();
            };

            while (timer.Enabled)
            {
                if(formMain.DataReceived) {
                    timer.Enabled = false;
                    timer.Stop();
                }
                Application.DoEvents();
            }
        }

        //---------------------------------------------------------------------------
        //        send data string to the serial port.
        //---------------------------------------------------------------------------
        private void SendSerialData(string data)
        {
            commPort.WriteLine(data);
        }

        //---------------------------------------------------------------------------
        //        receive data string from the serial port.
        //---------------------------------------------------------------------------
        private byte[] ReceiveSerialData()
        {
            System.Threading.Thread.Sleep(600);

            int intCount = 0;
            intCount = commPort.BytesToRead;
            byte[] buffer = new byte[intCount];
            int totalReadBytes = intCount;
            int offset = 0;
            int remaining = intCount;
            System.Diagnostics.Debug.WriteLine("Testing");

            try
            {
                commPort.Read(buffer, offset, remaining);
            }
            catch (TimeoutException e) {
                Array.Resize(ref buffer, totalReadBytes);
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
            }

            string newReturnStr = System.Text.Encoding.UTF8.GetString(buffer);
            ShowMonitor(newReturnStr, "RX:");
            commPort.ReadTimeout = CommParam.TimeOut;
            return buffer;
        }

        //---------------------------------------------------------------------------
        //        send read command
        //---------------------------------------------------------------------------
        private void cmdRead_Click(object sender, EventArgs e)
        {
            string CommandStr = Functions.SetSendBlock(Module.CMD_Read);           //Add STX,ETX,BCC

            //lock controls and clear any data in the buffer
            cmdRead.Enabled = false;
            cmdWrite.Enabled = false;
            cmdCancel.Enabled = true;
            textTrack1.Enabled = false;
            textTrack2.Enabled = false;
            textTrack3.Enabled = false;
            textTrack1.Text = "";
            textTrack2.Text = "";
            textTrack3.Text = "";
            comboQuickSelect.Text = "";

            ShowMonitor("", "Clearing Receive Buffer");
            byte[] TempBuffer = ReceiveSerialData();

            formMain.DataReceived = false;
            formMain.CancelFlag = false;
            labelMessage.Text = "Message : Please insert card to read now! Right now!";
            ShowMonitor("Read Command Sent - Insert Card", "");

            SendSerialData(CommandStr);
            ShowMonitor(CommandStr, "TX:");
            byte[] returnStr = ReceiveSerialData();
            string newReturnStr = System.Text.Encoding.UTF8.GetString(returnStr);

            if (newReturnStr == Module.ACK) {
                Wait(CommParam.TimeOut);
                byte[] returnString = ReceiveSerialData();
                string newReturnString = System.Text.Encoding.UTF8.GetString(returnString);

                Process_Data_Received(newReturnString);
                formMain.DataReceived = false;
            }

            if (newReturnStr == Module.NAK || newReturnStr == Module.DLE || newReturnStr == "") {
                labelMessage.Text = "Message : Something went wrong, please try again.";
                formMain.DataReceived = false;
            }

            //unlock controls
            cmdRead.Enabled = true;
            cmdCancel.Enabled = false;
        }

        //---------------------------------------------------------------------------
        //        send write command
        //---------------------------------------------------------------------------
        private void cmdWrite_Click(object sender, EventArgs e)
        {
            if(textTrack1.Text == "" && textTrack2.Text == "" && textTrack3.Text == "") {
                MessageBox.Show("Track Data is empty.", "Hello!", MessageBoxButtons.OK);
                return;
            }

            if (textTrack1.Text.Length > 38) {
                MessageBox.Show("Track 1 must be less than or equal to 38 characters.", "Track 1", MessageBoxButtons.OK);
                return;
            }

            if (textTrack2.Text.Length > 13) {
                MessageBox.Show("Track 2 must be less than or equal to 13 characters.", "Track 2", MessageBoxButtons.OK);
                return;
            }

            if (textTrack3.Text.Length > 38) {
                MessageBox.Show("Track 3 must be less than or equal to 38 characters.", "Track 3", MessageBoxButtons.OK);
                return;
            }

            string CommandStr = Module.CMD_Write + GetParameter("Write");

            //lock controls and clear any data in the buffer
            cmdRead.Enabled = false;
            cmdWrite.Enabled = false;
            cmdCancel.Enabled = true;
            ShowMonitor("Clearing Receive Buffer", "");
            byte[] TempBuffer = ReceiveSerialData();

            formMain.DataReceived = false;
            formMain.CancelFlag = false;
            labelMessage.Text = "Message : Please insert card to write now! Right now!";
            ShowMonitor("Write Command Sent - Insert Blank Card", "");

            CommandStr = Functions.SetSendBlock(CommandStr);           //Add STX,ETX,BCC
            SendSerialData(CommandStr);
            ShowMonitor(CommandStr, "TX:");
            byte[] returnStr = ReceiveSerialData();
            string newReturnStr = System.Text.Encoding.UTF8.GetString(returnStr);

            if (newReturnStr == Module.ACK) {
                Wait(CommParam.TimeOut);
                byte[] returnString = ReceiveSerialData();
                string newReturnString = System.Text.Encoding.UTF8.GetString(returnString);

                Process_Data_Received(newReturnString);
                formMain.DataReceived = false;
            }

            if (newReturnStr == Module.NAK || newReturnStr == Module.DLE || newReturnStr == "") {
                labelMessage.Text = "Message : Something went wrong, please try again.";
                formMain.DataReceived = false;
            }

            //unlock controls
            cmdRead.Enabled = true;
            cmdWrite.Enabled = true;
            cmdCancel.Enabled = false;
        }

        //---------------------------------------------------------------------------
        //        process response data
        //---------------------------------------------------------------------------
        private void Process_Data_Received(string data)
        {
            string responseType = "";
            if (data != "") {
                //check if message is valid - BCC
                bool validBCC = Functions.CheckBCC(data, CommParam);
                if (!validBCC)
                {
                    MessageBox.Show("BCC Error - Bad data recieved, please try again!", "BCC Error", MessageBoxButtons.OK);
                    return;
                }

                //check for TLA response
                if (data.IndexOf(CommParam.STX) != 0 && data != "") {
                    //MessageBox.Show("No STX Received", "STX", MessageBoxButtons.OK);
                    return;
                } else {
                    responseType = data.Substring(1, 1);
                }
            }

            switch (responseType) {
                //read response received
                case Module.RES_Read:   //always read all three tracks
                                        //<STX><Response Type (A)><Return Code (0 or N)><Track1><US>
                                        //<Return Code(0 Or N)><US><Track2><US>
                                        //<Return Code(0 Or N)><US><Track3><ETX><BCC>
                    string current = "";
                    string track1 = "";
                    string track2 = "";
                    string track3 = "";
                    int i = 2;

                    //track 1
                    string errorCode = data.Substring(i, 1);
                    i += 1;
                    if (errorCode == "0") {
                        while (current != CommParam.US && i < data.Length - 2) {
                            current = data.Substring(i, 1);
                            i += 1;
                            if (current == CommParam.US) {
                                current = "";
                                break;
                            } else {
                                track1 += current;
                            }
                        }
                    }
                    if (errorCode == "1") { track1 = "No Data - STX"; i += 1; }
                    if (errorCode == "2") { track1 = "No Data - ETX"; i += 1; }
                    if (errorCode == "3") { track1 = "LRC data invalid"; i += 1; }
                    if (errorCode == "4") { track1 = "VRC data invalid"; i += 1; }
                    if (errorCode == "6") { track1 = "Data format invalid"; i += 1; }

                    //track 2
                    errorCode = data.Substring(i, 1);
                    i += 1;
                    if (errorCode == "0") {
                        while (current != CommParam.US && i < data.Length - 2) {
                            current = data.Substring(i, 1);
                            i += 1;
                            if (current == CommParam.US) {
                                current = "";
                                break;
                            } else {
                                track2 += current;
                            }
                        }
                    }

                    if (errorCode == "1") { track2 = "No Data - STX"; i += 1; }
                    if (errorCode == "2") { track2 = "No Data - ETX"; i += 1; }
                    if (errorCode == "3") { track2 = "LRC data invalid"; i += 1; }
                    if (errorCode == "4") { track2 = "VRC data invalid"; i += 1; }
                    if (errorCode == "6") { track2 = "Data format invalid"; i += 1; }

                    //track 3
                    errorCode = data.Substring(i, 1);
                    i += 1;
                    if (errorCode == "0") {
                        while (current != CommParam.US && i < data.Length - 2) {
                            current = data.Substring(i, 1);
                            i += 1;
                            if (current == CommParam.US) {
                                current = "";
                                break;
                            } else {
                                track3 += current;
                            }
                        }
                    }

                    if (errorCode == "1") { track3 = "No Data - STX"; }
                    if (errorCode == "2") { track3 = "No Data - ETX"; }
                    if (errorCode == "3") { track3 = "LRC data invalid"; }
                    if (errorCode == "4") { track3 = "VRC data invalid"; }
                    if (errorCode == "6") { track3 = "Data format invalid"; }

                    textTrack1.Text = track1;
                    textTrack2.Text = track2;
                    textTrack3.Text = track3;
                    labelMessage.Text = "Message : Card data received! Congratulations!";
                    break;

                //write response received
                case Module.RES_Write:
                    SendSerialData(Module.ACK);

                    i = 2;
                    errorCode = "";
                    string stringWriteMessage = "";

                    //track 1
                    if (i < data.Length - 1) {
                        errorCode = data.Substring(i, 1);
                        stringWriteMessage = "Track 1 - ";
                        if (errorCode == char.ConvertFromUtf32((int)0x1F)) { stringWriteMessage += "No Data. "; i += 1; }
                        if (errorCode == "0") { stringWriteMessage += "OK. "; i += 2; }
                        if (errorCode == "1") { stringWriteMessage += "STX digit missing. "; i += 2; }
                        if (errorCode == "2") { stringWriteMessage += "ETX digit missing. "; i += 2; }
                        if (errorCode == "3") { stringWriteMessage += "LRC data invalid. "; i += 2; }
                        if (errorCode == "4") { stringWriteMessage += "VRC data invalid. "; i += 2; }
                        if (errorCode == "6") { stringWriteMessage += "Data format invalid. "; i += 2; }
                        if (errorCode == "7") { stringWriteMessage += "Encoding invalid. "; i += 2; }
                    }

                    //track 2
                    if (i < data.Length - 1) {
                        errorCode = data.Substring(i, 1);
                        stringWriteMessage += "Track 2 - ";
                        if (errorCode == char.ConvertFromUtf32((int)0x1F)) { stringWriteMessage += "No Data. "; i += 1; }
                        if (errorCode == "0") { stringWriteMessage += "OK. "; i += 2; }
                        if (errorCode == "1") { stringWriteMessage += "STX digit missing. "; i += 2; }
                        if (errorCode == "2") { stringWriteMessage += "ETX digit missing. "; i += 2; }
                        if (errorCode == "3") { stringWriteMessage += "LRC data invalid. "; i += 2; }
                        if (errorCode == "4") { stringWriteMessage += "VRC data invalid. "; i += 2; }
                        if (errorCode == "6") { stringWriteMessage += "Data format invalid. "; i += 2; }
                        if (errorCode == "7") { stringWriteMessage += "Encoding invalid. "; i += 2; }
                    } else {
                        stringWriteMessage += "Track 2 - No Data. ";
                    }

                    //track 3
                    if (i < data.Length - 1) {
                        errorCode = data.Substring(i, 1);
                        stringWriteMessage += "Track 3 - ";
                        if (errorCode == char.ConvertFromUtf32((int)0x1F)) { stringWriteMessage += "No Data. "; i += 1; }
                        if (errorCode == "0") { stringWriteMessage += "OK. "; i += 1; }
                        if (errorCode == "1") { stringWriteMessage += "STX digit missing. "; i += 1; }
                        if (errorCode == "2") { stringWriteMessage += "ETX digit missing. "; i += 1; }
                        if (errorCode == "3") { stringWriteMessage += "LRC data invalid. "; i += 1; }
                        if (errorCode == "4") { stringWriteMessage += "VRC data invalid. "; i += 1; }
                        if (errorCode == "6") { stringWriteMessage += "Data format invalid. "; i += 1; }
                        if (errorCode == "7") { stringWriteMessage += "Encoding invalid. "; i += 1; }
                    } else {
                        stringWriteMessage += "Track 3 - No Data. ";
                    }
                    labelMessage.Text = "Message : Card written successfully! " + stringWriteMessage;
                    break;

                case Module.RES_CommCheck:
                    break;

                case Module.RES_Status:
                    ShowMonitor("Status Message", "");
                    //MessageBox.Show("Found the Status response type", "Status response", MessageBoxButtons.OK);
                    break;

                case Module.RES_RW_OK:
                    break;

                default:
                    if (formMain.CancelFlag) {
                        //Cancel message and cancel command to card reader
                        labelMessage.Text = "Message : Command cancelled.";
                        string CommandStr = Functions.SetSendBlock(Module.CAN);           //Add STX,ETX,BCC
                        ShowMonitor(CommandStr, "TX:");
                        ShowMonitor("Cancel Sent To Device", "");
                        SendSerialData(CommandStr);
                        byte[] returnStr = ReceiveSerialData();
                        formMain.DataReceived = false;
                        formMain.CancelFlag = false;
                    } else {
                        //Timeout message and cancel command to card reader
                        labelMessage.Text = "Message : Card Timeout. You were too slow.";
                        string CommandStr = Functions.SetSendBlock(Module.CAN);           //Add STX,ETX,BCC
                        ShowMonitor("Timeout - Cancel Sent To Device", "");
                        ShowMonitor(CommandStr, "TX:");
                        SendSerialData(CommandStr);
                        byte[] returnStr = ReceiveSerialData();
                        formMain.DataReceived = false;
                        cmdCancel.Enabled = false;
                        MessageBox.Show("Application Timeout. Please enter a card within 60 seconds. Thanks!", "Card Timeout", MessageBoxButtons.OK);
                    }
                    break;
            }
        }

        //---------------------------------------------------------------------------
        //        exit program
        //---------------------------------------------------------------------------
        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //---------------------------------------------------------------------------
        //        clear track information
        //---------------------------------------------------------------------------
        private void cmdClear_Click(object sender, EventArgs e)
        {
            textTrack1.Text = "";
            textTrack2.Text = "";
            textTrack3.Text = "";
        }

        //---------------------------------------------------------------------------
        //        open instructions dialog box
        //---------------------------------------------------------------------------
        private void cmdInstructions_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Module.MSG_Instructions, "Instructions", MessageBoxButtons.OK);
        }

        //---------------------------------------------------------------------------
        //        cancel read/write commands
        //---------------------------------------------------------------------------
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //stop timer
            timer.Enabled = false;
            timer.Stop();
            cmdCancel.Enabled = false;
            formMain.DataReceived = true;
            formMain.CancelFlag = true;

            //reset form
            cmdClear.PerformClick();
            comboQuickSelect.Text = "";
            cmdRead.Enabled = true;
            cmdWrite.Enabled = false;
            textTrack1.Enabled = false;
            textTrack2.Enabled = false;
            textTrack3.Enabled = false;
            comboQuickSelect.Text = "";
        }

        //---------------------------------------------------------------------------
        //        test button commands
        //---------------------------------------------------------------------------
        private void cmdTest_Click(object sender, EventArgs e)
        {
            string CommandStr = "";
            //CommandStr = SetSendBlock(Module.ENQ);
            //CommandStr = SetSendBlock(Module.ACK);
            //CommandStr = Module.NAK;
            //CommandStr = SetSendBlock(Module.DLE);
            //CommandStr = Module.ENQ;
            //CommandStr = SetSendBlock(Module.EOT);
            CommandStr = Functions.SetSendBlock(Module.CMD_Status);

            formMain.DataReceived = false;
            labelMessage.Text = "Message : Test Command Sent.";
            textTrack1.Text = "";
            textTrack2.Text = "";
            textTrack3.Text = "";

            SendSerialData(CommandStr);
            ShowMonitor(CommandStr, "TX:");
            byte[] returnStr = ReceiveSerialData();
            string newReturnStr = System.Text.Encoding.UTF8.GetString(returnStr);

            if (newReturnStr.IndexOf(CommParam.STX) != 0 && newReturnStr != "") {

            }

            if (newReturnStr == Module.ACK) {
                Wait(CommParam.TimeOut);
                byte[] returnString = ReceiveSerialData();
                string newReturnString = System.Text.Encoding.UTF8.GetString(returnString);

                Process_Data_Received(newReturnString);
                formMain.DataReceived = false;
            }

            if (newReturnStr == Module.NAK || newReturnStr == Module.DLE || newReturnStr == "") {
                labelMessage.Text = "Message : Something went wrong, please try again.";
                formMain.DataReceived = false;
            }

        }

    }
}





