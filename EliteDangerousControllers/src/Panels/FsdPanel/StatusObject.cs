﻿using EliteDangerousControllers.src.interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EliteDangerousControllers.src.Panels.FsdPanel
{
    class StatusObject: IJsonObject
    {
        public string _name { set; get; }
        public string _value { set; get; }

        public StatusObject(string name, string value)
        {
            _name = name;
            _value = value;
        }

        string IJsonObject.ToString()
        {
            var jsonObject = new
            {
                name = _name,
                value = _value
            };
            return JsonSerializer.Serialize(jsonObject);
        }
    }
}