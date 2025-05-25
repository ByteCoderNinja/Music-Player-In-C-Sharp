// File: Mpv.cs (din ClassLibrary1)
// Functionalitate: Wrapper peste apeluri native MPV
// Testare: Testare integrată indirect prin controller
// Sablon: Wrapper

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.SqlServer.Server;
using MpvAPI.Interfaces;

namespace MpvAPI
{
    public class MpvFacade : IDisposable
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
        public MpvFacade(string dllPath)
        {
            if (dllPath == null) throw new ArgumentNullException("Null dll path");
            if (dllPath.Trim().Length == 0) throw new ArgumentException("Empty dll path");
            Function = new MpvFunction(dllPath);
            
        }

        public void Initialize()
        {
            try
            {
                Handle = Function.Create();
                if (Handle == IntPtr.Zero)
                    throw new Exception("Eșec la creare context MPV.");
                if (Function.Initialize(Handle) != 0)
                    throw new Exception("Eșec la inițializarea funcțiilor MPV.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Eroare la inițializare MPV.", ex);
            }
        }
        /*
[DllImport("mpv-1.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern int mpv_get_property(IntPtr handle, string name, int format, out IntPtr data);

        [DllImport("mpv-1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mpv_free(IntPtr data);

        public bool IsPausedRaw()
        {
            if (Handle == IntPtr.Zero)
                return true;

            IntPtr ptr;
            int result = mpv_get_property(Handle, "pause", 1, out ptr);

            if (result != 0 || ptr == IntPtr.Zero)
                return false;

            string value = Marshal.PtrToStringAnsi(ptr);
            mpv_free(ptr);

            return value == "yes";
        }
        */
        public bool isPaused()
        {
            try
            {
                if (Handle == IntPtr.Zero) return true;
                IntPtr lpBuffer;
                Function.GetProperty(Handle, "pause", 1, out lpBuffer);
                string paused = Marshal.PtrToStringAnsi(lpBuffer);
                Function.Free(lpBuffer);
                return paused == "yes";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nu s-a putut obține starea de pauză.", ex);
            }
        }

        public void Pause()
        {
            try
            {
                if (Handle == IntPtr.Zero) return;
                SetPropertyString("pause", "yes");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nu s-a putut pune pauză.", ex);
            }
        }

        public void Resume()
        {
            try
            {
                if (Handle == IntPtr.Zero) return;
                SetPropertyString("pause", "no");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nu s-a putut relua redarea.", ex);
            }
        }

        public void Load(string path, IntPtr windowHandle)
        {
            try
            {
                if (Handle != IntPtr.Zero)
                    Function.TerminateDestroy(Handle);
                Handle = Function.Create();
                if (Handle == IntPtr.Zero) return;
                Function.Initialize(Handle);
                SetPropertyString("keep-open", "yes");
                var windowId = windowHandle.ToInt64();
                Function.SetOption(Handle, GetUtf8Bytes("wid"), 4, ref windowId);
                Command("loadfile", path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nu s-a putut încărca fișierul media.", ex);
            }
        }

        public void Command(params string[] args)
        {
            IntPtr[] byteArrayPointers = null;
            IntPtr mainPtr = IntPtr.Zero;

            try
            {
                mainPtr = AllocateUtf8IntPtrArrayWithSentinel(args, out byteArrayPointers);
                Function.Command(Handle, mainPtr);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Eroare la trimiterea comenzii către MPV.", ex);
            }
            finally
            {
                if (byteArrayPointers != null)
                {
                    foreach (var ptr in byteArrayPointers)
                        Marshal.FreeHGlobal(ptr);
                }
                if (mainPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(mainPtr);
            }
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
                //Function.SetProperty(Handle, GetUtf8Bytes(name), 1, ref bytes);
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
            bytes = (IntPtr)Function.GetPropertyString(Handle, name, 4, out bytes);
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
            Function.SetPropertyString(Handle, "speed", speedStr);
        }

        public void SetTime(double seconds)
        {
            if (Handle == IntPtr.Zero)
                return;

            string timeStr = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Function.SetPropertyString(Handle, "time-pos", timeStr);
        }

        public double GetTime()
        {
            if (Handle == IntPtr.Zero)
                return -1;

            double time = 0;
            Function.GetPropertyDouble(Handle, "time-pos", 5, ref time);

            if (time == 0)
            {
                return -1;
            }
            return time;
        }

        public double GetDuration()
        {
            if (Handle == IntPtr.Zero)
                return -1;

            double duration = 0;
            Function.GetPropertyDouble(Handle, "duration", 5, ref duration);

            if (duration == 0)
            {
                return -1;
            }
            return (double)duration;
        }

        public void SetVolume(double volume)
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            string volumeStr = volume.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Function.SetPropertyString(Handle, "volume", volumeStr);
        }

        public double GetVolume()
        {
            if (Handle == IntPtr.Zero) return -1;

            IntPtr result = IntPtr.Zero;
            Function.GetPropertyString(Handle, "volume", 4, out result);
            if (result == IntPtr.Zero) return -1;

            string volStr = Marshal.PtrToStringAnsi(result);
            Function.Free(result);

            if (double.TryParse(volStr, out double volume))
                return volume;

            return -1;
        }


        ~MpvFacade()
        {
            Dispose(false);
        }
    }
}
