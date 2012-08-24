using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AlkampferVsix2012.Utils
{


    public static class ExceptionUtils
    {
        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer,
           uint nSize, IntPtr Arguments);

        // the version, the sample is built upon:
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, IntPtr pArguments);

        // the parameters can also be passed as a string array:
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, string[] Arguments);

        // see the sample code
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, string[] Arguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hMem);

        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
        const uint FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
        const uint FORMAT_MESSAGE_FROM_STRING = 0x00000400;

        public static String GetMessageFromException(Int32 hresult) 
        {
            //Alternative with win32exception.
            //Win32Exception temp = new Win32Exception(hresult);
            //return temp.Message

            IntPtr lpMsgBuf = IntPtr.Zero;
            uint dwChars = FormatMessage(
                FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_HMODULE | FORMAT_MESSAGE_FROM_SYSTEM,
                IntPtr.Zero,
                (uint)hresult,
                0, // Default language
                ref lpMsgBuf,
                0,
                IntPtr.Zero);
                if (dwChars == 0)
                {
                    // Handle the error.
                    int le = Marshal.GetLastWin32Error();
                    return null;
                }

            string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);

            // Free the buffer.
            lpMsgBuf = LocalFree(lpMsgBuf);
            return sRet;

            // add for the forth signature
            try
            {
                StringBuilder msgBuilder = new StringBuilder(101);

                string formatExpression = "%1,%2%!";
                string[] formatArgs = new string[] { "Hello", "world" };

                IntPtr formatPtr = Marshal.StringToHGlobalAnsi(formatExpression);

                //must specify the FORMAT_MESSAGE_ARGUMENT_ARRAY flag when pass an array
                uint length = FormatMessage(FORMAT_MESSAGE_FROM_STRING | FORMAT_MESSAGE_ARGUMENT_ARRAY, formatPtr, 0, 0, msgBuilder, 101, formatArgs);

                if (length == 0)
                {
                    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)Marshal.GetLastWin32Error(), 0, msgBuilder, 101, null);

                    Console.WriteLine("Error:" + msgBuilder.ToString());
                }
                else
                {
                    return msgBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

