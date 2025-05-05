// File: Mpv.cs (din ClassLibrary1)
// Functionalitate: Wrapper peste apeluri native MPV
// Testare: Testare integrată indirect prin controller
// Sablon: Wrapper

using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.SqlServer.Server;
using MpvAPI.Interfaces;

namespace MpvAPI
{
    public class Mpv : IDisposable
    {
        public IMpvFunction Function
        {
            get => function;
            set
            {
                if(value is null) throw new ArgumentNullException("value is null");
                function = value;
            }
        }
        public IntPtr Handle
        {
            get => handle;
            private set
            {
                if (value == IntPtr.Zero) throw new ArgumentException("Invalid handle", nameof(handle));
                handle = value;
            }
        }
        private IMpvFunction function;
        private IntPtr handle;
        private bool disposed = false;
        public Mpv(string dllPath)
        {
            if (dllPath == null) throw new ArgumentNullException("Null dll path");
            if (dllPath.Trim().Length == 0) throw new ArgumentException("Empty dll path");
            Function = new MpvFunction(dllPath);
            
        }

        public bool isPaused()
        {
            if (Handle == IntPtr.Zero) return true;
            var lpBuffer = IntPtr.Zero;
            //lpBuffer = GetPropertyString("pause");
            //_mpvGetPropertyString(_mpvHandle, GetUtf8Bytes("pause"), MpvFormatString, ref lpBuffer);
            Function.GetPropertystring(Handle, GetUtf8Bytes("pause"), 1, ref lpBuffer);
            var isPaused = Marshal.PtrToStringAnsi(lpBuffer) == "yes";
            Function.Free(lpBuffer);
            return isPaused;

        }
        public void Pause()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }
            SetPropertyString("pause", "yes");
        }
        public void Resume()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }
            SetPropertyString("pause", "no");
        }
        public void Initialize()
        {
            Handle = Function.Create();
            if (Handle == IntPtr.Zero)
                throw new Exception("Failed create mpv context.");
            if (Function.Initialize(Handle) != 0)
                throw new Exception("Failed initialize functions");
        }

        public void Load(string path, IntPtr windowHandle)
        {
            if (Handle != IntPtr.Zero)
                Function.TerminateDestroy(Handle);
            Handle = Function.Create();
            if (Handle == IntPtr.Zero)
            {
                return;
            }
            Function.Initialize(Handle);
            SetPropertyString("keep-open", "yes");
            var windowId = windowHandle.ToInt64();
            Function.SetOption(Handle, GetUtf8Bytes("wid"), 4, ref windowId);
            Command("loadfile", path);
        }

        public void Command(params string[] args)
        {
            IntPtr[] byteArrayPointers;
            var mainPtr = AllocateUtf8IntPtrArrayWithSentinel(args, out byteArrayPointers);
            Function.Command(Handle, mainPtr);
            foreach (var ptr in byteArrayPointers)
            {
                Marshal.FreeHGlobal(ptr);
            }
            Marshal.FreeHGlobal(mainPtr);
        }
        public void SetProperty(string name, byte[] data, int format = 9)
        {
            if (data.Length >= 1)
            {
                var dataLength = data.Length;
                var dataPtr = Marshal.AllocCoTaskMem(dataLength);
                Marshal.Copy(data, 0, dataPtr, dataLength);
                Function.SetProperty(Handle, GetUtf8Bytes(name), format, ref data);
                
            }
        }

        public void SetPropertyString(string name, string data)
        {
            if((name != null && name.Trim().Length>0)&&(data != null && data.Length > 0))
            {
                var bytes = GetUtf8Bytes(data);
                Function.SetProperty(Handle, GetUtf8Bytes(name), 1, ref bytes);
                SetProperty(name, bytes, 1);
            }
        }

        public void SetPropertyLong(string name, long data)
        {
            if ((name != null && name.Trim().Length > 0))
            {
                var bytes = BitConverter.GetBytes(data);
                SetProperty(name, bytes, 4);
            }
        }
        
        public IntPtr GetPropertyString(string name)
        {
            if (Handle == IntPtr.Zero) return IntPtr.Zero;
            var bytes = IntPtr.Zero;
            bytes = (IntPtr)Function.GetPropertystring(Handle, GetUtf8Bytes(name), 4, ref bytes);
            return bytes;
        }
        public static IntPtr AllocateUtf8IntPtrArrayWithSentinel(string[] arr, out IntPtr[] byteArrayPointers)
        {
            int numberOfStrings = arr.Length + 1; // add extra element for extra null pointer last (sentinel)
            byteArrayPointers = new IntPtr[numberOfStrings];
            IntPtr rootPointer = Marshal.AllocCoTaskMem(IntPtr.Size * numberOfStrings);
            for (int index = 0; index < arr.Length; index++)
            {
                var bytes = GetUtf8Bytes(arr[index]);
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
                byteArrayPointers[index] = unmanagedPointer;
            }
            Marshal.Copy(byteArrayPointers, 0, rootPointer, numberOfStrings);
            return rootPointer;
        }
        private static byte[] GetUtf8Bytes(string s)
        {
            return Encoding.UTF8.GetBytes(s + "\0");
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                Function.TerminateDestroy(Handle);
                if (disposing && Function is IDisposable disposableFunctions)
                {
                    disposableFunctions.Dispose();
                }
                disposed = true;
            }
        }

        public void SetSpeed(float speed)
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }
            string speedStr = speed.ToString(System.Globalization.CultureInfo.InvariantCulture);
            MpvFunction.mpv_set_property_string(Handle, "speed", speedStr);
        }


        ~Mpv()
        {
            Dispose(false);
        }
    }
}
