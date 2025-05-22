using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpvPlayerUI
{
    interface ISourceStrategy
    {
        List<string> LoadMusic();
    }
}
