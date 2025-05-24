
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
        MpvGetPropertystring GetPropertyString { get; }
        MpvSetProperty SetProperty   { get; }
        MpvFree Free { get; }
    }
}
