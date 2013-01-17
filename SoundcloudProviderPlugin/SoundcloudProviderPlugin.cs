using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.ContentProvider.Plugins.Soundcloud
{
	public class Track : ITrack
	{
		public Track (JObject json)
		{
			_TrackName = (String)json ["title"];
			_MediaUrl = (String)json ["stream_url"];
		}

		private String _TrackName;
		private String _MediaUrl;

		public String TrackName {
			get {
				return _TrackName;
			}
		}

		public String MediaUrl {
			get {
				return _MediaUrl;
			}
		}
	}

	/// <summary>
	/// 	Provider.
	/// </summary>
	/// <example>
	/// 	List<ITrack> tracks = ContentProvider.Providers.Instance.AllPlugins ["Soundcloud"].Search ("flume");
	/// </example>
	/// <description>
	/// 	<see cref="http://developers.soundcloud.com/docs/api/reference"/>
	/// </description>
	public class Provider : IContentProvider
	{

		public String Name {
			get {
				return "Soundcloud";
			}
		}

		public String Url {
			get {
				return "http://soundcloud.com/";
			}
		}

		private const String API_KEY = "apigee";
		private const String API_URL = "http://api.soundcloud.com/";
		private const String API_FORMAT = "json";

		public Provider ()
		{
		}

		/// <summary>
		/// Search the specified term.
		/// </summary>
		/// <param name='term'>
		/// The search query term.
		/// </param>
		public List<ITrack> Search (String term)
		{
			List<ITrack> tracks = new List<ITrack> ();

			JArray array = ApiRequest ("tracks", "q", term);
			foreach (JObject jobj in array) {
				tracks.Add (new Track (jobj));
			}

			return tracks;
		}

		/// <summary>
		/// 	Sends a GET request to the Soundcloud REST API.
		/// </summary>
		/// <returns>
		/// 	A deserialized JSON array, with Soundcloud results.
		/// </returns>
		/// <param name='ressource'>
		/// 	The requested ressource. See API docs.
		/// </param>
		/// <param name='filters'>
		/// 	The filters for the requested ressource. All filters are noted
		/// 	in API docs. The params alternate between key and value, so its
		/// 	length must be a modulo of two.
		/// </param>
		private JArray ApiRequest (String ressource, params String[] filters)
		{
			if ((filters.Length % 2) != 0)
				// Throw an exception, if the filters aren't a modulo of 2,
				// because it alternates between key and value. I don't like
				// it neither.
				throw new ArgumentException ("Filters must have key and value");

			String url = API_URL + ressource + "." + API_FORMAT + "?consumer_key=" + API_KEY;

			for (var i=0; i<filters.Length; i=i+2) {
				url += "&" + filters [i];
				url += "=" + filters [i + 1];
			}

			var request = HttpWebRequest.Create (url);
			WebResponse response = request.GetResponse ();
			StreamReader reader = new StreamReader (response.GetResponseStream ());
			String json = reader.ReadToEnd ();

			return JArray.Parse (json);
		}
	}
}
