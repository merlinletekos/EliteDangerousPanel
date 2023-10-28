using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousControllers.src.interfaces
{
    interface IControlPanel
    {
        SerialConnection _SerialConnection { get; set; }
        EliteConnection _EliteConnection { get; set; }

        void ListenEvents();
        void StopPanel();
    }
}
