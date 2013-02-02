using System;
using System.Net.Sockets;

using Bauglir.Ex;


namespace cloudmusic2upnp.UserInterface.Web.WebSocket
{
    public class HackedWebSocketServer : WebSocketServer
    {
        public override WebSocketServerConnection GetConnectionInstance(
		    TcpClient aClient, WebSocketHeaders aHeaders, string aHost,
			string aPort, string aResourceName, string aOrigin, string aCookie,
			string aVersion, ref string aProtocol, ref string aExtension,
		    ref int aHttpCode
        )
        {
            aProtocol = "-";
            aExtension = "-";
            return new WebSocketServerConnection(aClient, this);
        }
    }


    public class Manger : IWebManager
    {
        private HackedWebSocketServer Server;
        private int Port;

        public event EventHandler<ConnectionOpenEventArgs> OnConnectionOpen;

        public Manger(int port)
        {
            Port = port;

            Server = new HackedWebSocketServer();
            Server.AfterAddConnection += HandleAfterAddConnection;
        }

        public void Start()
        {
            Server.Start(System.Net.IPAddress.Any, Port);
        }

        private void HandleAfterAddConnection(WebSocketServer aServer, WebSocketServerConnection aConnection)
        {
            aConnection.ConnectionOpen += HandleConnectionOpen;
        }

        private void HandleConnectionOpen(WebSocketConnection aConnection)
        {
            var client = new Client(aConnection);
            OnConnectionOpen(this, new ConnectionOpenEventArgs(client));
        }

        public void Stop()
        {
            Server.Stop();
        }
    }
}
