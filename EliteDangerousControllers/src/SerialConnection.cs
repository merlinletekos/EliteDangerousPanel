using System.IO.Ports;

namespace EliteDangerousControllers.src
{
    class SerialConnection
    {
        private readonly SerialPort _serialPort;

        public SerialConnection(string l_portName, int l_port)
        {
            _serialPort = new SerialPort(l_portName, l_port);
            StartSerialConnection();
        }

        private void StartSerialConnection()
        {
            _serialPort?.Open();
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
            _serialPort.Write(message);
        }

        public void Close()
        {
            _serialPort?.Close();
        }
    }
}
