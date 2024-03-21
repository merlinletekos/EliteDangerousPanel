using EliteDangerousControllers.src.interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EliteDangerousControllers.src.Panels.FsdPanel
{
    class StatusObject: IJsonObject
    {
        public string _name { set; get; }
        public bool Value { set; get; }

        public StatusObject(string name, bool value)
        {
            _name = name;
            Value = value;
        }

        public string toString()
        {
            var jsonObject = new
            {
                name = _name,
                value = Value
            };
            return JsonSerializer.Serialize(jsonObject);
        }
    }
}
