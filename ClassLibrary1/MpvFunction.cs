using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MpvAPI.Interfaces;

namespace MpvAPI
{
    internal class MpvFunction : IMpvFunction, IDisposable
    {
        public MpvCreate Create {  get; private set; }
        public MpvInitialize Initialize { get; private set; }
        public MpvCommand Command { get; private set; }
        public MpvTerminateDestroy TerminateDestroy { get; private set; }
        public MpvSetOption SetOption { get; private set; }
        public MpvSetOptionString SetOptionString { get; private set; }
        public MpvGetPropertyString GetPropertyString { get; private set; }
        public MpvSetProperty SetProperty { get; private set; }
        public MpvGetProperty GetProperty { get; private set; }
        public MpvGetPropertyDouble GetPropertyDouble { get; private set; }
        public MpvSetPropertyString SetPropertyString { get; private set; }
        public MpvFree Free { get; private set; }

        private IntPtr dllHandle;

        private bool disposed = false;
        public MpvFunction(string dllPath)
        {
            LoadDll(dllPath);
            LoadFunctions();
        }
        private void LoadDll(string dllPath)
        {
            if (dllPath is null) throw new ArgumentException("Dll path is null");
            dllHandle = WindowsImportedFunctions.LoadLibrary(dllPath);
            if (dllHandle == IntPtr.Zero) throw new Exception("Failed to load dll.");
        }
        private void LoadFunctions()
        {
            Create = LoadFunction<MpvCreate>("mpv_create");
            Initialize = LoadFunction<MpvInitialize>("mpv_initialize");
            Command = LoadFunction<MpvCommand>("mpv_command");
            TerminateDestroy = LoadFunction<MpvTerminateDestroy>("mpv_terminate_destroy");
            SetOption = LoadFunction<MpvSetOption>("mpv_set_option");
            SetOptionString = LoadFunction<MpvSetOptionString>("mpv_set_option_string");
            GetPropertyString = LoadFunction<MpvGetPropertyString>("mpv_get_property_string");
            GetProperty = LoadFunction<MpvGetProperty>("mpv_get_property");
            GetPropertyDouble = LoadFunction<MpvGetPropertyDouble>("mpv_get_property");
            SetProperty = LoadFunction<MpvSetProperty>("mpv_set_property");
            SetPropertyString = LoadFunction<MpvSetPropertyString>("mpv_set_property_string");
            Free = LoadFunction<MpvFree>("mpv_free");
        }
        private TDelegate LoadFunction<TDelegate>(string name) where TDelegate: class
        {
            IntPtr address = WindowsImportedFunctions.GetProcAddress(dllHandle, name);
            if (address != IntPtr.Zero)
                return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(address, typeof(TDelegate));
            throw new Exception("Failed to load function: " + name);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    WindowsImportedFunctions.FreeLibrary(dllHandle);
                }
                disposed = true;
            }
        }
    }
}
