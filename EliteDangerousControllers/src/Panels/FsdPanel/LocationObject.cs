
using EliteDangerousControllers.src.interfaces;
using System.Text.Json;

namespace EliteDangerousControllers.src.Panels.FsdPanel
{
    class LocationObject: IJsonObject
    {
        public string _name { get; set; }
        public string Value { get; set; }
        public bool Scoopable { get; set; }

        public LocationObject(string name, string value, bool scoopable)
        {
            _name = name;
            Value = value;
            Scoopable = scoopable;
        }

        public string toString()
        {
            var jsonObject = new
            {
                name = _name,
                value = Value,
                scoopable = Scoopable
            };
            return JsonSerializer.Serialize(jsonObject);
        }
    }
}
