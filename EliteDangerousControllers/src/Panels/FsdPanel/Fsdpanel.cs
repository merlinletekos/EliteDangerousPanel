using EliteAPI.Abstractions;
using EliteAPI.Abstractions.Events;
using EliteAPI.Events;
using EliteAPI.Events.Status.Ship.Events;
using EliteDangerousControllers.src.interfaces;
using EliteDangerousControllers.src.Panels.FsdPanel;

namespace EliteDangerousControllers.src.Panels
{
    internal class Fsdpanel : IControlPanel
    {
        public SerialConnection _SerialConnection { get; set; }
        public EliteConnection _EliteConnection { get; set; }

        private readonly IEliteDangerousApi _api;

        private readonly string[] _scoopableStars = { "O", "B", "A", "F", "G", "K", "M" };

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

            // Caution event
            _api.Events.On<JetConeBoostEvent>(JetConeEvent);
            _api.Events.On<InInterdictionStatusEvent>(InterdictionStatusListener);

            // Status event
            _api.Events.On<FsdJumpStatusEvent>(FsdJumpStatusEvent);
            _api.Events.On<FsdCooldownStatusEvent>(FsdCooldownStatusEvent);
            _api.Events.On<FsdChargingStatusEvent>(FsdChargingStatusEvent);

            // Data for screen
            _api.Events.On<LocationEvent>(LocationEventListener);
            _api.Events.On<FsdJumpEvent>(LocationJumpEventListener);
            _api.Events.On<FsdTargetEvent>(LocationTargetListener);
            _api.Events.On<NavRouteClearEvent>(LocationNavRouteClearListener);

        }

        private void HardpointOpen(HardpointsStatusEvent @event, EventContext context)
        {
            StatusObject hardPointStatus = new("HardPointStatus", @event.Value);
            _SerialConnection.SendMessage(hardPointStatus.toString());
        }

        private void CargoScoopOpen(CargoScoopStatusEvent @event, EventContext context)
        {
            StatusObject cargoScoopObject = new("CargoScoopStatus", @event.Value);
            _SerialConnection.SendMessage(cargoScoopObject.toString());
        }

        private void MassLockedStatus(MassLockedStatusEvent @event, EventContext context)
        {
            StatusObject massLockedObject = new("MassLockedStatus", @event.Value);
            _SerialConnection.SendMessage(massLockedObject.toString());
        }

        private void LandingGearOpen(GearStatusEvent @event, EventContext context)
        {
            StatusObject gearStatusObject = new("LandingGearStatus", @event.Value);
            _SerialConnection.SendMessage(gearStatusObject.toString());

        }

        private void JetConeEvent(JetConeBoostEvent @event, EventContext context)
        {
            Console.WriteLine(@event.BoostValue);
        }

        private void InterdictionStatusListener(InInterdictionStatusEvent @event, EventContext context) {
            StatusObject interdictionStatusObject = new("InterdictionStatus", @event.Value);
            _SerialConnection.SendMessage(interdictionStatusObject.toString());
        }

        private void FsdChargingStatusEvent(FsdChargingStatusEvent @event, EventContext context)
        {
            StatusObject fsdChargingStatusObject = new("FsdChargingEvent", @event.Value);
            _SerialConnection.SendMessage(fsdChargingStatusObject.toString());
        }

        private void FsdJumpStatusEvent(FsdJumpStatusEvent @event, EventContext context)
        {
            StatusObject fsdJumpStatusObject = new("FsdJumpStatusEvent", @event.Value);
            _SerialConnection.SendMessage(fsdJumpStatusObject.toString());
        }

        private void FsdCooldownStatusEvent(FsdCooldownStatusEvent @event, EventContext context)
        {
            StatusObject fsdCooldownStatusObject = new("FsdCooldownEvent", @event.Value);
            _SerialConnection.SendMessage(fsdCooldownStatusObject.toString());
        }

        // LOCATION LISTENER
        private void LocationEventListener(LocationEvent @event, EventContext context)
        {
            LocationObject locationObject = new("LocationCurrent", @event.StarSystem, false);
            _SerialConnection.SendMessage(locationObject.toString());
        }

        private void LocationJumpEventListener(FsdJumpEvent @event, EventContext context)
        {
            LocationObject locationObject = new("LocationCurrent", @event.StarSystem, false);
            _SerialConnection.SendMessage(locationObject.toString());
        }

        private void LocationTargetListener(FsdTargetEvent @event, EventContext context)
        {
            LocationObject locationObject = new("LocationTarget", @event.Name, _scoopableStars.Contains(@event.StarClass));
            _SerialConnection.SendMessage(locationObject.toString());
        }

        private void LocationNavRouteClearListener(NavRouteClearEvent @event, EventContext context)
        {
            StatusObject navRouteClearEvent = new("RouteClearEvent", false);
            _SerialConnection.SendMessage(navRouteClearEvent.toString());
        }
    }
}
