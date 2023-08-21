using EliteAPI.Abstractions;
using EliteAPI.Abstractions.Events;
using EliteAPI.Events.Status.Ship.Events;
using EliteDangerousControllers.src.interfaces;
using EliteDangerousControllers.src.Panels.FsdPanel;

namespace EliteDangerousControllers.src.Panels
{
    internal class Fsdpanel : IControlPanel
    {
        public SerialConnection _SerialConnection { get; set; }
        public EliteConnection _EliteConnection { get; set; }

        private IEliteDangerousApi _api;

        public Fsdpanel(SerialConnection serialConnection, EliteConnection eliteConnection)
        {
            _SerialConnection = serialConnection;
            _EliteConnection = eliteConnection;

            _api = _EliteConnection.GetApiConnection();

            ListenEvents();
        }

        public void StopPanel()
        {
            _SerialConnection.Close();
        }

        public void ListenEvents()
        {
            // Status event
            _api.Events.On<HardpointsStatusEvent>(HardpointOpen);
            _api.Events.On<CargoScoopStatusEvent>(CargoScoopOpen);
            _api.Events.On<MassLockedStatusEvent>(MassLockedStatus);
            _api.Events.On<GearStatusEvent>(LandingGearOpen);
        }

        private void HardpointOpen(HardpointsStatusEvent @event, EventContext context)
        {
            StatusObject harPointStatus = new StatusObject("HardPointStatus", @event.Value);
            Console.WriteLine(harPointStatus.toString());
        }

        private void CargoScoopOpen(CargoScoopStatusEvent @event, EventContext context)
        {
            StatusObject cargoScoopObject = new StatusObject("CargoScoopStatus", @event.Value);
            Console.WriteLine(cargoScoopObject.toString());
        }

        private void MassLockedStatus(MassLockedStatusEvent @event, EventContext context)
        {
            StatusObject massLockedObject = new StatusObject("MassLockedStatus", @event.Value);
            Console.WriteLine(massLockedObject.toString());
        }

        private void LandingGearOpen(GearStatusEvent @event, EventContext context)
        {
            StatusObject gearStatusObject = new StatusObject("LandingGearStatus", @event.Value);
            Console.WriteLine(gearStatusObject.toString());

        }
    }
}
