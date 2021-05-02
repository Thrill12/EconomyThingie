using JsonSubTypes;
using Newtonsoft.Json;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public static class SerializationSettings
    {
        private static JsonSerializerSettings settings;

        public static JsonSerializerSettings current
        {
            get
            {
                if (settings == null)
                    settings = CreateSettings();

                return settings;
            }
        }

        public static JsonSerializerSettings CreateSettings()
        {
            var stg = new JsonSerializerSettings();
            stg.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(BaseShip), "Type") // type property is only defined here
                .RegisterSubtype<Cruiser>("Cruiser")
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());

            return stg;
        }
    }
}
