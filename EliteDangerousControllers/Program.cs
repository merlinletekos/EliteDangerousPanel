
using EliteDangerousControllers.src;
using EliteDangerousControllers.src.interfaces;
using EliteDangerousControllers.src.Panels;

namespace EliteDangerousControllers
{
    class EliteDangerousControllers
    {

        private static string PORT_NAME = "COM5";
        private static int PORT_PIN = 9600;

        private IControlPanel[]? _panels;

        public static void Main()
        {
            EliteDangerousControllers eliteDangerousControllers = new EliteDangerousControllers();
            eliteDangerousControllers.Running();
        }

        private void Running()
        {
            EliteConnection eliteConnection = new EliteConnection();
            eliteConnection.StartApi();

            // Init Panel list
            initFsdPanel(eliteConnection);

            // Wait to kill the programm
            eliteConnection.ListenEvent();
            foreach (IControlPanel panel in _panels)
            {
                panel.StopPanel();
            }
        }

        private void initFsdPanel(EliteConnection eliteConnection)
        {
            SerialConnection fsdSerialConnection = new SerialConnection(PORT_NAME, PORT_PIN);
            IControlPanel[] panels = {
                new Fsdpanel(fsdSerialConnection, eliteConnection)
            };
            _panels = panels;
        }
    }
}