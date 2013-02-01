using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;


namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class ProviderNotification : Message
    {
        [DataContract]
        public class ProviderData
        {
            [DataMember]
            public readonly string
                ID;

            [DataMember]
            public readonly string
                Name;

            public ProviderData(IContentProvider provider)
            {
                ID = provider.Name;
                Name = provider.Name;
            }
        }

        [DataMember]
        public readonly ProviderData[]
            Providers;


        public ProviderNotification(Providers providers)
        {
            var list = new List<ProviderData>();
            foreach (KeyValuePair<string, IContentProvider> kvp in providers.Plugins)
            {
                list.Add(new ProviderData(kvp.Value));
            }

            Providers = list.ToArray();
        }

        public override String ToJson()
        {
            return Header<ProviderNotification>.ToJson(this);
        }
    }
}

