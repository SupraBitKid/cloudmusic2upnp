using System;

namespace cloudmusic2upnp.DeviceController
{
	/// <summary>
	/// Interface for DeviceControllers, like a UPnP Control Point.
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// Gets a list with all avalible devices (eg UPnP Renderers).
		/// </summary>
		IDevice[] GetDevices();

        /// <summary>
        /// Raises if a usefull device is found or removed from the network.
        /// </summary>
        event EventHandler<DeviceEventArgs> DeviceDiscovery;
	}

    

	public interface IDevice
	{
        /// <summary>
        /// Raises if the playstate of a device is changed.
        /// e.g. a device stopped playback because it reached the end of a song
        /// </summary>
        event EventHandler<DevicePlaystateEventArgs> PlaystateChanged;
        
        /// <summary>
        /// Returns a friendly name of the device.
        /// </summary>
        String FriendlyName
        { get; }
		/// <summary>
		/// Play the currently selected media.
		/// </summary>
		void Play();

		/// <summary>
		/// Pause the currently playing media.
		/// </summary>
		void Pause();

		/// <summary>
		/// Stops the current playback.
		/// </summary>
		void Stop();

		/// <summary>
		/// Sets the media ressource to a specific URL.
		/// </summary>
		/// <param name='url'>
		/// The URL, that should be played.
		/// </param>
		void SetMediaUrl(String url);
    }
}

