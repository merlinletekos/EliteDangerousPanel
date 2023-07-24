using System.IO.Ports;

namespace EliteDangerousControllers.src
{
    class SerialConnection
    {
        private SerialPort _serialPort;

        public SerialConnection(string l_portName, int l_port)
        {
            _serialPort = new SerialPort(l_portName, l_port);
            startSerialConnection();
        }

        private void startSerialConnection()
        {
            _serialPort?.Open();
        }

        public void SendMessage(string message)
        {
            _serialPort.Write(message);
        }

        public void Close()
        {
            _serialPort?.Close();
        }
    }
}
