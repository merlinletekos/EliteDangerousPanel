
using EliteDangerousControllers.src;
using EliteDangerousControllers.src.interfaces;
using EliteDangerousControllers.src.Panels;

namespace EliteDangerousControllers
{
    class EliteDangerousControllers
    {

        private static readonly string PORT_NAME = "COM6";
        private static readonly int PORT_PIN = 9600;

        private IControlPanel[]? _panels;

        public static void Main()
        {
            EliteDangerousControllers eliteDangerousControllers = new();
            eliteDangerousControllers.Running();
        }

        private void Running()
        {
            EliteConnection eliteConnection = new();
            eliteConnection.StartApi();

            // Init Panel list
            InitFsdPanel(eliteConnection);

            // Wait to kill the programm
            eliteConnection.ListenEvent();

            if (_panels != null)
            {
                foreach (IControlPanel panel in _panels)
                {
                    panel.StopPanel();
                }
            }
        }

        private void InitFsdPanel(EliteConnection eliteConnection)
        {
            SerialConnection fsdSerialConnection = new(PORT_NAME, PORT_PIN);
            IControlPanel[] panels = {
                new Fsdpanel(fsdSerialConnection, eliteConnection)
            };
            _panels = panels;
        }
    }
}