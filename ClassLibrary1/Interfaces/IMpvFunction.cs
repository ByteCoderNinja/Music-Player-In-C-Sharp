
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MpvAPI.Interfaces
{
    public interface IMpvFunction
    {
        MpvCreate Create { get; }
        MpvInitialize Initialize { get; }
        MpvCommand Command { get; }
        MpvTerminateDestroy TerminateDestroy { get; }
        MpvSetOption SetOption { get; }
        MpvSetOptionString SetOptionString { get; }
        MpvGetPropertyString GetPropertyString { get; }
        MpvSetPropertyString SetPropertyString { get; }
        MpvSetProperty SetProperty { get; }
        MpvGetProperty GetProperty { get; }
        MpvGetPropertyDouble GetPropertyDouble { get;  }
        MpvFree Free { get; }
    }
}
