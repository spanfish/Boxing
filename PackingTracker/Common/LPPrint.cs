using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace com.newtronics.Common
{
    public class LPPrint
    {
        #region kernel32.dll
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }

        [DllImport("kernel32.dll")]
        private static extern int CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            int dwShareMode,
            int lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            int hTemplateFile
        );

        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(
            int hFile,
            byte[] lpBuffer,
            int nNumberOfBytesToWrite,
            ref int lpNumberOfBytesWritten,
            ref OVERLAPPED lpOverlapped
        );

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        #endregion
        private int iHandle;

        #region 打印机是否已连接
        public bool IsOpened
        {
            get
            {
                return iHandle != -1;
            }
        }
        #endregion

        #region 连接LPT1打印机
        public bool Open()
        {
            iHandle = CreateFile("lpt1", 0x40000000/*GENERIC_WRITE*/, 0/*no share*/, 0, 3, 0, 0);
            return iHandle != -1;
        }
        #endregion

        #region 打印内容
        public bool Write(String Mystring)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                byte[] mybyte = System.Text.Encoding.Default.GetBytes(Mystring);
                bool b = WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
                return b;
            }
            else
            {
                //throw new Exception("不能连接到打印机!");
                return false;
            }
        }

        public bool Write(byte[] mybyte)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                bool b = WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
                return b;
            }
            else
            {
                //throw new Exception("不能连接到打印机!");
                return false;
            }
        }
        #endregion

        #region 关闭打印机连接
        public bool Close()
        {
            if (iHandle != -1)
            {
                return CloseHandle(iHandle);
            }
            return true;
        }
        #endregion
    }
}
