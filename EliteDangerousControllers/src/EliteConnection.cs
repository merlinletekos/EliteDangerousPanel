
using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Events;

namespace EliteDangerousControllers.src
{
    class EliteConnection
    {
        private readonly IEliteDangerousApi _api;

        public EliteConnection()
        {
            _api = EliteDangerousApi.Create();
        }

        public IEliteDangerousApi GetApiConnection()
        {
            return _api;
        }

        public async void StartApi()
        {
            await _api.StartAsync();
            Console.WriteLine("EliteDangerousControllers started");
        }

        public void ListenEvent()
        {
            _api.Events.WaitFor<ShutdownEvent>();
            StopApi();
        }

        private async void StopApi()
        {
            Console.WriteLine("EliteDangerousControllers stopped");
            await _api.StopAsync();
        }
    }
}
