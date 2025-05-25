using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MpvAPI
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr MpvCreate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvInitialize(IntPtr mpvHandle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvCommand(IntPtr mpvHandle, IntPtr strings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvTerminateDestroy(IntPtr mpvHandle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetOption(IntPtr mpvHandle, byte[] name, int format, ref long data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetOptionString(IntPtr mpvHandle, byte[] name, byte[] value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvGetPropertyString(IntPtr mpvHandle, string name, int format, out IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetProperty(IntPtr mpvHandle, byte[] name, int format, ref byte[] data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvGetProperty(IntPtr mpvHandle, [MarshalAs(UnmanagedType.LPStr)] string name, int format, out IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvGetPropertyDouble(IntPtr mpvHandle, string name, int format, ref double data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetPropertyString(IntPtr ctx, string name, string data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MpvFree(IntPtr data);
}
