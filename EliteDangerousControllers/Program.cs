
using EliteAPI.Events.Status.Ship.Events;
using EliteDangerousControllers.src;

namespace EliteDangerousControllers
{
    class EliteDangerousControllers
    {
        public static void Main()
        {
            EliteDangerousControllers eliteDangerousControllers = new EliteDangerousControllers();
            eliteDangerousControllers.Running();
        }

        private void Running()
        {
            EliteConnection eliteConnection = new EliteConnection();
            eliteConnection.StartApi();
            eliteConnection.ListenEvent();
        }
    }
}