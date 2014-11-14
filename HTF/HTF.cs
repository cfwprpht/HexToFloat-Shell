using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTF
{
    class HTF
    {
        #region Var
        public static string[] badHexLetter = 
        { 
            ",", ";", ".", ":", "-", "_", "#", "'", "*", "+", "~", "`", "´", "?", "ß", @"\", "=", "}", ")", "]", "(", "[", "{", "/", "&", "%", "$", "§", "!", "<", ">", "|", "€", "@", "°", "^",
            "Ä", "Ö", "Ü", "ä", "ü", "ö", "²", "³", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "G", "H", "I", "J", "K", "L", "M", "N", 
            "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        };
        static bool reverse = false;
        static bool error = false;
        static int arg = 0;
        static int arg2 = 0;
        static string _arg2 = "";
        #endregion Var

        // Used to know if a valid argument is used
        public static bool IsArg(string overload)
        {
            if (overload == "-H")
                return true;
            if (overload == "-HH")
                return true;
            if (overload == "-S")
                return true;
            if (overload == "-D")
                return true;
            return false;
        }

        // Show Version and such....
        public static void ShowVersion()
        {
            Console.WriteLine("Hex2Float & Float2Hex Tool\nv1.00\nby cfwprophet\n\n");
        }

        // Show Help Screen
        public static void ShowUsage()
        {
            Console.WriteLine( "Usage: HTF.exe {[option]} [option] <input>" + 
                              "\n\n-v          will print version info\n-h          will print Help, so actually this one you read right now\n-r          will reverse a Hex value as you wish\n" +
                              "-H          convert a WORD Hex value (x4 bytes) to a Single Float\n-S          convert a Single Float value to a WORD Hex value (x4 bytes)\n" +
                              "-HH         convert a DWORD Hex value (x8 bytes) to a Double Float\n-D          convert a Double Float value to a DWORD Hex value (x8 bytes)\n\n");
        }

        // Check User Input
        private static int CheckInput(string[] args)
        {
            if (args.Length == 3 && args[0] == "-r")
            {
                arg = 1;
                arg2 = 2;
                reverse = true;
            }
            else if (args.Length == 3 && args[1] == "-r")
            {
                arg = 0;
                arg2 = 2;
                reverse = true;
            }
            else if (args.Length == 2 && IsArg(args[0]))
            {
                arg = 0;
                arg2 = 1;
            }
            else if (args.Length == 2 && IsArg(args[1]))
            {
                arg = 1;
                arg2 = 0;
            }
            else if (args.Length == 1 && args[0] == "-v")
            {
                ShowVersion();
                Environment.Exit(0);
            }
            else if (args.Length == 1 && args[0] == "-h")
            {
                ShowUsage();
                Environment.Exit(0);
            } 
            else { error = true; }

            if (error || args.Length == 3 && !reverse || args.Length == 3 && args[0] == args[1] ||
                args.Length > 1 && args[0] == "-v" || args.Length > 1 && args[1] == "-v" || args.Length > 1 && args[0] == "-h" || args.Length > 1 && args[1] == "-h" ||
                args.Length > 2 && args[2] == "-H" || args.Length > 2 && args[2] == "-HH" || args.Length > 2 && args[2] == "-S" ||
                args.Length > 2 && args[2] == "-D" || args.Length > 2 && args[2] == "-h" || args.Length > 2 && args[2] == "-v")
            {
                Console.WriteLine("CheckInput: Wrong Input!!\n\n");
                ShowUsage();
                return 0;
            }
            return 1;
        }

        static void CheckInputValue(string _arg, string methode)
        {
            if (methode == "S")
            {
                try { Single singleFloat = Single.Parse(_arg, CultureInfo.InvariantCulture); }
                catch (Exception)
                {
                    Console.WriteLine("This is not a Valid Single Float value!\n\n");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else if (methode == "D")
            {
                try { Double doubleFloat = Double.Parse(_arg, CultureInfo.InvariantCulture); }
                catch (Exception)
                {
                    Console.WriteLine("This is not a Valid Double Float value!\n\n");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else if (methode == "H")
            {
                if (!_arg.Contains("0x"))
                    _arg2 = "0x" + _arg;
                else { _arg2 = _arg; }

                string overload = _arg2;
                bool adjust = false;

                // Check input value for bad letter's and remove them if found
                for (int i = 0; i < badHexLetter.Length; i++)
                {
                    if (overload.Contains(badHexLetter[i]))
                    {
                        Console.WriteLine("This is not a valid Hex value!");
                        Environment.Exit(0);
                    }
                }

                // Check if the input value have the correct length. If not add zero(s) on top for to short and remove values from end if to long.
                if (overload.Length > 8)
                {
                    for (int i = overload.Length; i > 8; i--)
                    {
                        overload = overload.Remove(overload.Length - 1);
                        adjust = true;
                    }
                    if (adjust)
                        Console.WriteLine("Your Hex string would be adjusted please double check!");
                }
                else if (overload.Length < 8)
                {
                    for (int i = overload.Length; i < 8; i++)
                    {
                        overload = overload.Insert(0, "0");
                        adjust = true;
                    }
                    if (adjust)
                        Console.WriteLine("Your Hex string would be adjusted please double check!");
                }
                if (adjust)
                    _arg2 = "0x" + overload;
            }
            else if (methode == "HH")
            {
                if (!_arg.Contains("0x"))
                    _arg2 = "0x" + _arg;
                else { _arg2 = _arg; }

                string overload = _arg2.Replace("0x", "");
                bool adjust = false;

                // Check input value for bad letter's and remove them if found
                for (int i = 0; i < badHexLetter.Length; i++)
                {
                    if (overload.Contains(badHexLetter[i]))
                    {
                        Console.WriteLine("This is not a valid Hex value!\n\n");
                        Environment.Exit(0);
                    }
                }

                // Check if the input value have the correct length. If not add zero(s) on top for to short and remove values from end if to long.
                if (overload.Length > 16)
                {
                    for (int i = overload.Length; i > 16; i--)
                    {
                        overload = overload.Remove(overload.Length - 1);
                        adjust = true;
                    }
                    if (adjust)
                        Console.WriteLine("Your Hex string would be adjusted please double check!\n\n");
                }
                else if (overload.Length < 16)
                {
                    for (int i = overload.Length; i < 16; i++)
                    {
                        overload = overload.Insert(0, "0");
                        adjust = true;
                    }
                    if (adjust)
                        Console.WriteLine("Your Hex string would be adjusted please double check!\n\n");
                }
                if (adjust)
                    _arg2 = "0x" + overload;
            }
        }

        // Little Endian Swap the input long Hex value and return as such
        private static long LittleEndian(long toSwap)
        {
            byte[] _reversed = BitConverter.GetBytes(toSwap);
            Array.Reverse(_reversed);
            long result = BitConverter.ToInt64(_reversed, 0);
            return result;
        }

        // Convert WORD to Float
        private static string ConvertToSingleFloat(string singleHex)
        {
            try
            {
                byte[] singleByte = BitConverter.GetBytes(uint.Parse(singleHex, NumberStyles.AllowHexSpecifier));   // By putting the input into a uint value and reading that to a byte[] we already have reversed the input Hex byte value...
                if (reverse) { Array.Reverse(singleByte); }  // ...So we only need to reverse again if we specifically want to use the little endian value to get our float
                Single singleFloat = BitConverter.ToSingle(singleByte, 0);
                return singleFloat.ToString();
            }
            catch (Exception x) { return x.ToString(); }
        }

        // Convert Float to WORD
        private static string ConvertToSingleHex(string singleFloat)
        {
            try
            {
                byte[] singleHex = BitConverter.GetBytes(Single.Parse(singleFloat, CultureInfo.InvariantCulture));
                if (!reverse) { Array.Reverse(singleHex); }  // Standard : we will reverse the Array cause single float is stored as a little endian value. If Reverse check box is set we will not reverse and use orig hex value.
                return "0x" + BitConverter.ToString(singleHex).Replace("-", "");
            }
            catch (Exception x) { return x.ToString(); }
        }

        // Convert DWORD to Double Float
        private static string ConvertToDoubleFloat(string doubleHex)
        {
            try
            {
                if (reverse) { return BitConverter.Int64BitsToDouble(LittleEndian(long.Parse(doubleHex, NumberStyles.AllowHexSpecifier))).ToString(); }
                else { return BitConverter.Int64BitsToDouble(long.Parse(doubleHex, NumberStyles.AllowHexSpecifier)).ToString(); }
            }
            catch (Exception x) { return x.ToString(); }
        }

        // Convert double Float to a DWORD
        private static string ConvertToDoubleHex(string doubleFloat)
        {
            try
            {
                if (reverse) { return "0x" + LittleEndian(BitConverter.DoubleToInt64Bits(Double.Parse(doubleFloat, CultureInfo.InvariantCulture))).ToString("X"); }
                else { return "0x" + BitConverter.DoubleToInt64Bits(Double.Parse(doubleFloat, CultureInfo.InvariantCulture)).ToString("X"); } // The "X" specification in the ToString() cast will display the Long integer value with there Hex representation
            }
            catch (Exception x) { return x.ToString(); }
        }

        // The Main entry point
        static void Main(string[] args)
        {
            if (CheckInput(args) == 0)
                Environment.Exit(0);

            ShowVersion();

            if (args[arg] == "-H")
            {
                CheckInputValue(args[arg2], "H");
                Console.WriteLine("WORD: " + _arg2 + "\nSingle: " + ConvertToSingleFloat(_arg2.Replace("0x", "")) + "\n\n");
            }
            else if (args[arg] == "-S")
            {
                CheckInputValue(args[arg2], "S");
                Console.WriteLine("Single: " + args[arg2] + "\nWORD: " + ConvertToSingleHex(args[arg2]) + "\n\n");
                
            }
            else if (args[arg] == "-HH")
            {
                CheckInputValue(args[arg2], "HH");
                Console.WriteLine("DWORD: " + _arg2 + "\nDouble: " + ConvertToDoubleFloat(_arg2.Replace("0x", "")) + "\n\n");
                
            }
            else if (args[arg] == "-D")
            {
                CheckInputValue(args[arg2], "D");
                Console.WriteLine("Double: " + args[arg2] + "\nDWORD: " + ConvertToDoubleHex(args[arg2]) + "\n\n");
            }
            Console.WriteLine("THX for using my Tool\nWill EXIT now...\nBy :)");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
