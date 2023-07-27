using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousControllers.src.interfaces
{
    interface IJsonObject
    {
        public string _name { set; get; }

        public string ToString();
    }
}
