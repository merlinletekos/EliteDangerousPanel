using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousControllers.src.interfaces
{
    interface IControlPanel
    {
        public SerialConnection _serialConnection { get; set; }
    }
}
