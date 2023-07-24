﻿using EliteAPI.Abstractions;
using EliteAPI.Abstractions.Events;
using EliteAPI.Events.Status.Ship.Events;
using EliteDangerousControllers.src.interfaces;

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
            _api.Events.On<HardpointsStatusEvent>(HardpointOpen);
        }

        private void HardpointOpen(HardpointsStatusEvent @event, EventContext context)
        {
            Console.WriteLine($"HardPoint: {@event.Value}");
        }
    }
}