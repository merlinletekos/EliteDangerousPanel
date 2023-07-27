
using EliteDangerousControllers.src.interfaces;
using System.Text.Json;

namespace EliteDangerousControllers.src.Panels.FsdPanel
{
    class LocationObject: IJsonObject
    {
        public string _name { get; set; }
        public string _value { get; set; }
        public bool _scoopable { get; set; }

        public LocationObject(string name, string value, bool scoopable)
        {
            _name = name;
            _value = value;
            _scoopable = scoopable;
        }

        string IJsonObject.ToString()
        {
            var jsonObject = new
            {
                name = _name,
                value = _value,
                scoopable = _scoopable
            };
            return JsonSerializer.Serialize(jsonObject);
        }
    }
}
