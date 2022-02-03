using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardTestC
{
    public class Module
    {
        //Attribute VB_Name = "modResorce"
        //****************************************************************************
        //*
        //*          CT/FT Series communication demo software
        //*              Copyright(c) Neuron Corporation
        //*              Version 0.6 Beta     Date:   03/28/2000
        //*
        //*          Development Language    : Visual Basic 5.0 (Japanese)
        //*          ActiveX Control         : Microsoft Comm Control 5.0 (MSComm.ocx)
        //*
        //*          Project file name       : DemoCTFT.vbp  [proDemoCTFT]
        //*          Form module file name   : Main.frm      [frmMain]
        //*          Common module file name : Function.bas  [modFunction]
        //*                                  : Resource.bas  [modResource]
        //*
        //*          Supported model : CT-2xxN/CT-6xxN/CT-7xx/CT-8xx Series
        //*                            FT-2xx/FT-4xx/FT-5xx/FT-7xx/FT-8xx Series
        //*
        //****************************************************************************
        //*
        //*          Updated by Carleton Peltz for Desert Diamond Casinos
        //*          James 1:17
        //*
        //****************************************************************************

        //---------------------------------------------------------------------------
        //           Communication parameter user type declaration
        //---------------------------------------------------------------------------
        public class CommParameterType {
            public string Port;                         //Port number
            public int Baudrate;                        //Baud rate          [bit/sec]
            public int BitLength;                       //Bit length
            public System.IO.Ports.Parity Parity;       //Parity             [0:NONE, 1:ODD, 2:EVEN]
            public System.IO.Ports.StopBits StopBit;    //Stop bit           [1:1bit, 2:2bit, 3:1.5bit]
            public bool RTSEnable;                      //RTS line setting   [0:OFF, 1:ON]
            public bool DTREnable;                      //DTR line setting   [0:OFF, 1:ON]
            public int CTSControl;                      //CTS line control   [0:no control, 1:control]
            public int DSRControl;                      //DSR line control   [0:no control, 1:control]
            public bool STXEnable;                       //STX enable         [0:no STX, 1:STX]
            public bool ETXEnable;                       //ETX enable         [0:no ETX, 1:ETX, 2:CR+LF]
            public string STX;                          //STX code
            public string US;                           //US code
            public string ETX;                          //ETX code
            public int BCCByte;                         //BCC length
            public int BCCRange;                        //BCC calculate start position [1:STX, 2:STX Next]
            public string Password;                     //EEPROM open password [6 digit]
            public int TimeOut;                         //Receive waiting time [msec]
        }

        //---------------------------------------------------------------------------
        //       Public variable declaration
        //---------------------------------------------------------------------------
        public static int Res;                     //Result error message value
        public const string ACK = "\u0006";                  //ACK Code (06h)
        public const string NAK = "\u0015";                  //NAK Code (15h)
        public const string CAN = "\u0018";                  //CAN Code (18h)
        public const string DLE = "\u0010";                  //DLE Code (10h)
        public const string ENQ = "\u0005";                  //ENQ Code (05h)
        public const string EOT = "\u0004";                  //EOT Code (04h)

        //---------------------------------------------------------------------------
        //       CT/FT command code and response code declaration
        //           Model   :FT-2xx,FT-4xx,FT-5xx,CT-2xxN,CT-6xxN series protocol (mode 2)
        //                   :FT-7xx,8xx, CT-7xx-5050,8xx-5050 series protocol
        //
        //           Command data block  [ STX+CMD+(DATA)+ETX+BCC ]      CMD: commnad
        //           Response data block [ STX+CMD+{RC+DATA}+ETX+BCC ]   RC : return code
        //           Send times CTS control. Receive times no control
        //           StopBitF‚Pbit default ***********Chacter after t, F is symbol(box)
        //---------------------------------------------------------------------------
        public const string CMD_Prohibit = "0";               //30h      Prohibit command
        public const string CMD_Read = "1";                   //31h      Read command
        public const string CMD_ReadHold = "2";               //32h      Read and card hold command
        public const string CMD_Write = "3";                  //33h      Write command
        public const string CMD_WriteHold = "4";              //34h      Write and card hold command
        public const string CMD_CommCheck = ":";              //3Ah      Communication check command
        public const string CMD_Eject = ";";                  //3Bh      Eject card command
        public const string CMD_Reset = "<";                  //3Ch      Reset unit command
        public const string CMD_Status = "=";                 //3Dh      Status command
        public const string CMD_EjectHold = ">";              //3Eh      Eject and card hold command
        public const string CMD_LowCo300 = "62";              //36h + 32h    Low coercivity set command 1
        public const string CMD_LowCo650 = "61";              //36h + 31h    Low coercivity set command 2
        public const string CMD_HiCo = "60";                  //36h + 30h    Hi coercivity set command
        //------------- EEPROM Access Command ( FT-7xx,8xx Only) -------------------
        public const string CMD_EROM_Password = "NEURON";     //         Default password "NEURON"
        public const string CMD_EROM_Open = "p";              //70h      EEPROM open command
        public const string CMD_EROM_Close = "v";             //76h      EEPROM close command
        public const string CMD_EROM_Read = "s";              //73h      EEPROM read command
        public const string CMD_EROM_WriteEnable = "q";       //71h      EEPROM write enable command
        public const string CMD_EROM_Write = "t";             //74h      EEPROM write command
        public const string CMD_EROM_WriteDisable = "r";      //72h      EEPROM write disable command
        //------------- Response ---------------------------------------------------
        public const string RES_Read = "A";                   //41h      Read response
        public const string RES_ReadHold = "B";               //42h      Read card hold response
        public const string RES_Write = "C";                  //43h      Write response
        public const string RES_WriteHold = "D";              //44h      Write card hold response
        public const string RES_WriteVerify = "W";            //57h      Write verify response (Only manual type)
        public const string RES_CommCheck = "J";              //4Ah      Communication check response
        public const string RES_Status = "M";                 //4Dh      Status response
        public const string RES_RW_OK = "0";                  //30h      Read or write Ok response code
        //------------- EEPROM Access Command ( FT-7xx,8xx Only) -------------------
        public const string RES_EROM = "s";                   //73h      EEPROM read response

        //---------------------------------------------------------------------------
        //       Application messages const declaration
        //---------------------------------------------------------------------------
        public const string MSG_Comm_RecDataError = "Communication Data Error" + "\r\n" + "NAK send over flow";
        public const string MSG_Comm_SendDataError = "Communication Data Error. Data send over flow.";
        public const string MSG_Comm_BCCError = "   <BCC Error>";
        public const string MSG_Comm_ConfirmData = " Please confirm data box";
        public const string MSG_Comm_CommSetError = "Comm port setting error : ";
        public const string MSG_Comm_CommPortClose = "Comm port status : closed";
        public const string MSG_Comm_CommPortOpen = "Comm port status : opened";
        public const string MSG_Comm_CommPortStatus = "Comm port closed! Please open comm port.";

        //---------------------------------------------------------------------------
        //       Response messages const declaration
        //---------------------------------------------------------------------------
        public const string MSG_Response_OK = "Command and response OK";
        public const string MSG_Response_Error = "Communication error.";
        public const string MSG_Response_CAN = "[CAN] Received";
        public const string MSG_Response_DLE = "[DLE] Received";
        public const string MSG_Response_NAK = "[NAK] Received";
        public const string MSG_Response_ENQ = "[ENQ] Received";
        public const string MSG_Response_None = "No response";
        public const string MSG_ResponseMode_Read = "Read response";
        public const string MSG_ResponseMode_ReadHold = "Read data and card hold";
        public const string MSG_ResponseMode_Write = "Write verify response";
        public const string MSG_ResponseMode_WriteHold = "Write data and card hold";
        public const string MSG_ResponseMode_WriteVerify = "Write verify start";
        public const string MSG_ResponseMode_EEPRom = "EEPROM Data response";
        public const string MSG_ResponseMode_CommCheck = "Communication check response";

        public const string MSG_Communication_Monitor_Default = "Thank you for using the Desert Diamond Neuron software. v1.0\r\n" +
            "[Memo]\r\n" +
            "Monitor description\r\n" + 
            "TX: Data transmitted to unit\r\n" +
            "RX: Data received from unit\r\n";

        public const string MSG_Instructions = "How to use the encoder program:\r\n" +
            "1) Make sure that the comm port is open: select a comm port from the available ports. " +
            "The comm port is usually COM1 on serial, can be a different number on USB.\r\n\r\n" +
            "2) To read a card: Press the Read button, there is a timout of 60 seconds to insert the card to be read. " +
            "The card information will be displayed in the appropriate track boxes.\r\n\r\n" +
            "3) To write a card: Select an option from the Quick Select dropdown. " +
            "For 1's, 2's and 9's, once selected, click the Write button, then insert a blank card within 60 seconds. " +
            "For custom cards, enter the information into the desired tracks, click the Write button, then insert a blank card within 60 seconds.\r\n\r\n" +
            "Track Info - Tracks 1 & 3 can contain up to 38 characters, Track 2 can contain up to 13 characters.  Track 1 can use CAPITAL letters and numbers, Tracks 2 & 3 only use numbers.\r\n\r\n" +
            "4) To exit: Click the Exit button or the X on the top right of the window.\r\n\r\n" +
            "5) The communication monitor is for informational purposes only.";
    }
}

