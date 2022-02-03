using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace CardTestC
{
    class Functions
    {
        //---------------------------------------------------------------------------
        //     [MSComm.Settings] Property setting function procedure
        //       input   SourceParam :User definition variable at [CommParameterType]
        //       Output              :Result string
        //---------------------------------------------------------------------------
        public static string GetCommSettings(Module.CommParameterType SourceParam)
        {
            try
            {
                string TempStr = "";
                TempStr = SourceParam.Baudrate + ",";
                switch (SourceParam.Parity) {
                    case Parity.None:
                        TempStr = TempStr + "n" + ",";
                        break;
                    case Parity.Odd:
                        TempStr = TempStr + "o" + ",";
                        break;
                    case Parity.Even:
                        TempStr = TempStr + "e" + ",";
                        break;
                }
                TempStr = TempStr + SourceParam.BitLength + "," + SourceParam.StopBit;
                return TempStr;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return "";
            }
        }

        //---------------------------------------------------------------------------
        //       BCC calculation check function on received data
        //           input   Source  :Received data
        //           Output          :Result check (True/False)
        //---------------------------------------------------------------------------
        public static bool CheckBCC(string Source, Module.CommParameterType SourceParam)
        {
            try
            {
                string TempStr = "";
                string BCCStr = "";
                int BccStart = 0;

                if (SourceParam.STXEnable) {                                            //STX exist check
                    switch (SourceParam.BCCRange) {                                     //BCC start position check
                        case 1:
                            BccStart = 1;                                               //Set position on STX
                            break;
                        case 2:
                            BccStart = SourceParam.STX.Length + 1;                      //Set position on next STX
                            break;
                    }
                } else {
                    BccStart = 1;                                                       //Set position on STX
                }

                switch (SourceParam.BCCByte) {                                          //BCC length check
                    case 0:                                                             //case of no BCC
                        return true;
                    case 1:                                                             //case of 1 byte BCC
                        TempStr = Source.Substring(0, Source.Length - 1);
                        BCCStr = Source.Substring(Source.Length - 1, 1);
                        break;
                    case 2:                                                             //case of 2 byte BCC
                        TempStr = Source;
                        if (TempStr.Substring(TempStr.Length - 1, 1) == "\r\n") { 
                            TempStr = TempStr.Substring(0, TempStr.Length - 1); 
                        }
                        TempStr = TempStr.Substring(0, TempStr.Length - 2);
                        BCCStr = TempStr.Substring(TempStr.Length - 2, 2);
                        break;
                }

                if (SetBCC(TempStr, BccStart, 0, SourceParam.BCCByte) == Source) {      //Comparison BCC
                    return true;
                } else {
                    return false;
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return false;
            }
        }

        //---------------------------------------------------------------------------
        //       BCC calculation function procedure
        //       Input   Source      :Calculate string source
        //               StartPos    :BCC calculate start position 1:STX, 2:STX Next)
        //               EndPos      :BCC calculate end position 0:ETX, 1:Data end)
        //               BccByte     :BCC byte length (1:1 byte BCC, 2:2 byte BCC)
        //       Output              :Add BCC string
        //---------------------------------------------------------------------------
        public static string SetBCC(string Source, int StartPos, int EndPos, int BCCByte)
        {
            try
            {
                int i;
                int test;
                int Bcc;
                string WorkStr;

                Bcc = 0;
                for (i = StartPos; i < Source.Length - EndPos; i++)
                {
                    test = Asc(Convert.ToChar(Source.Substring(i, 1)));
                    Bcc = Bcc ^ test;                                    //Calculate BCC: XOR
                }

                switch (BCCByte) {                                       //Check BCC length
                    case 1:                                              //1 byte BCC
                        return Source + char.ConvertFromUtf32((int)Bcc);
                    case 2:                                              //2 byte BCC
                        if (Bcc < 10) {
                            WorkStr = "0" + Bcc.ToString("X");
                        } else {
                            WorkStr = Bcc.ToString("X");
                        }
 //                       WorkStr = char.ConvertFromUtf32(Asc(WorkStr.Substring(1, 1))) + char.ConvertFromUtf32(Asc(WorkStr.Substring(2, 1)));
                        return Source + WorkStr + "\r";                  //Add CR character
                    default:
                        return "";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return "";
            }
        }

        //---------------------------------------------------------------------------
        //       Change character type function procedure (ASII type or HEX type)
        //       Input   Source  :Sorce string
        //               Mode    :Change mode ("HEX":HEX mode, "ASCII":ASCII mode)
        //       Output          :Result string
        //---------------------------------------------------------------------------
        public static string ChangeCharType(string Source, string Mode)
        {
            try
            {
                string SourceStr;
                string TempStr;
                string S;
                string WorkStr;
                int i;

                switch (Mode)
                {
                    case "HEX":                                          //HEX mode
                        TempStr = "";
                        WorkStr = "";
                        for (i = 0; i < Source.Length; i++) { //Change character
                            S = Source.Substring(i, 1);
                            if (Asc(Convert.ToChar(S)) < 0x10) {
                                WorkStr = "0" + (Asc(Convert.ToChar(S))).ToString("X"); 
                            } else {
                                WorkStr = (Asc(Convert.ToChar(S))).ToString("X");
                            }
                            TempStr = TempStr + " " + WorkStr;
                        }
                        return TempStr.Trim();
                    case "ASCII":                                        //ASCII mode
                        SourceStr = Source;
                        SourceStr = ReplaceString(SourceStr, char.ConvertFromUtf32((int)0), char.ConvertFromUtf32((int)1));
                        //Replace Null with viewable character.
                        SourceStr = ReplaceString(SourceStr, "\r\n", char.ConvertFromUtf32((int)1));
                        //Replace CR with viewable character.
                        return SourceStr;
                }
                return "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return "";
            }
        }

        //---------------------------------------------------------------------------
        //       Replace string function procedure
        //       Input   Source  :Source string
        //               Target  :Target string
        //               Change  :Replace string
        //       Output          :Result string
        //---------------------------------------------------------------------------
        public static string ReplaceString(string Source, string Target, string Change)
        {
            try
            {
                string TempStr;
                string WorkStr;
                int LenTarget;
                int P;
                
                if (Source.IndexOf(Target) == 0) {                 //If target not found, return source
                    return Source;
                }

                TempStr = Source;
                LenTarget = Target.Length;
                WorkStr = "";

                while (TempStr.IndexOf(Target) != 0) {              //Replace until no exist target
                    P = TempStr.IndexOf(Target);
                    if (LenTarget == Change.Length) {               //Case of Same length
                        TempStr = Mid(TempStr, P, Change);
                    } else {                                          //Case of different length
                        WorkStr = WorkStr + TempStr.Substring(1, P - 1) + Change;
                        P = P + LenTarget;
                        TempStr = TempStr.Substring(P, TempStr.Length - (P - 1));
                    }
                }
                return WorkStr + TempStr;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Warning: " + e.GetType().FullName, MessageBoxButtons.OK);
                return "";
            }
        }

        //---------------------------------------------------------------------------
        //       
        //       Mid function to emulate VB Mid statement
        //       
        //---------------------------------------------------------------------------
        public static string Mid(string input, int index, string newString)
        {
            List<char> b1 = new List<char>(input.ToCharArray()) ;
            List<char> b2 = new List<char>(newString.ToCharArray());
            b1.InsertRange(index, b2);

            return new string(b1.ToArray());
        }

        //---------------------------------------------------------------------------
        //       Set send string block function procedure (Add STX,ETX,BCC)
        //       Input   Source  :Source string
        //       Output          :Result string
        //---------------------------------------------------------------------------
        public static string SetSendBlock(string Source)
        {
            try
            {
                string TempStr = "";
                int BccStart = 0;

                TempStr = Source;
                if (formMain.CommParam.STXEnable == true)
                {                       //Case of STX exist
                    TempStr = formMain.CommParam.STX + TempStr;                   //Add STX
                    switch (formMain.CommParam.BCCRange)
                    {
                        case 1:                                          //Case of 1 byte BCC
                            BccStart = 1;
                            break;
                        case 2:                                          //Case of 2 byte BCC
                            BccStart = formMain.CommParam.STX.Length + 1;
                            break;
                    }
                }
                else
                {
                    BccStart = 1;
                }

                switch (formMain.CommParam.ETXEnable)
                {
                    case true:                                           //Case ETX exist
                        TempStr = TempStr + formMain.CommParam.ETX;               //Add ETX
                        break;
                    case false:                                          //Case ETX is CR+LF
                        TempStr = TempStr + "\r\n";                      //Add CR+LF
                        break;
                }

                //Add BCC
                if (formMain.CommParam.BCCByte > 0) {
                    TempStr = SetBCC(TempStr, BccStart, 0, formMain.CommParam.BCCByte);
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
        //       
        //       ASCII conversion function for single char
        //       
        //---------------------------------------------------------------------------
        public static int Asc(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }

    }
}
