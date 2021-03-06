using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class PlayRequest : Message
    {
        [DataMember]
        public String
            TrackID;

        public ITrack Track
        {
            get
            {
                return Core.Providers.GetTrackById(TrackID);
            }

        }

        public PlayRequest()
        {
        }

        public override String ToJson()
        {
            return Header<PlayRequest>.ToJson(this);
        }
    }
}

