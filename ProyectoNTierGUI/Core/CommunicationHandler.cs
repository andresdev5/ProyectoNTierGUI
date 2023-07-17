using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ProyectoNTierGUI.Model;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ProyectoNTierGUI.Core
{
    public class CommunicationHandler
    {
        private static CommunicationHandler? _instance;
        private IPEndPoint _ipEndPoint;
        private Socket _client;

        // callback
        public delegate void ReceiveCallbackEventHandler(SocketMessage message);
        private Dictionary<Type, ReceiveCallbackEventHandler> _receiveCallbacks = new();

        public static CommunicationHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommunicationHandler();
                }
                return _instance;
            }
        }

        private CommunicationHandler()
        {
            long address = 0x0100007F; // 127.0.0.1
            int port = 0x15B3; // 5555

            _ipEndPoint = new(address, port);
            _client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async void Connect()
        {
            await _client.ConnectAsync(_ipEndPoint);

            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    var buffer = new byte[1_024];
                    var received = await _client.ReceiveAsync(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);

                    try
                    {
                        if (response != null && response.Trim() != "")
                        {
                            SocketMessage? message = ParseSocketResponse(response);

                            if (message == null || message?.Source != SocketSource.LISTENER)
                            {
                                continue;
                            }

                            Receive(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }).Start();
        }

        public void OnReceive<T>(ReceiveCallbackEventHandler receiveCallback)
        {
            _receiveCallbacks.Remove(typeof(T));
            _receiveCallbacks.Add(typeof(T), receiveCallback);
        }

        public void Send(string message)
        {
            _client.Send(Encoding.UTF8.GetBytes(message));
        }

        public void Send(SocketMessage message)
        {
            Send(BuildSocketMessage(message));
        }

        private void Receive(SocketMessage message)
        {
            foreach (var entry in _receiveCallbacks)
            {
                var callback = entry.Value;
                callback(message);
            }
        }

        public static SocketMessage? ParseSocketResponse(string response)
        {
            if (response == null || response.Trim() == "")
            {
                return null;
            }

            /*
                Socket Format:

                SOURCE:LISTENER|GUI
                METHOD:POST|GET|PUT|DELETE|OUTPUT
                ENTITY:TransactionReason|Employee|etc
                ACTION:<anyString>
                BODY:<anyString>
             */

            try
            {
                var regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline;
                var sourceHeader = (new Regex(@"SOURCE:([^\n]+)", regexOptions)).Match(response).Groups[1].Value;
                var methodHeader = (new Regex(@"METHOD:([^\n]+)", regexOptions)).Match(response).Groups[1].Value;
                var entityHeader = (new Regex(@"ENTITY:([^\n]+)", regexOptions)).Match(response).Groups[1].Value;
                var actionHeader = (new Regex(@"ACTION:([^\n]+)", regexOptions)).Match(response).Groups[1].Value;
                var body = (new Regex(@"BODY:([^$]+)", regexOptions)).Match(response).Groups[1].Value;

                return new SocketMessage()
                {
                    Source = Enum.TryParse(sourceHeader, out SocketSource source) ? source : SocketSource.UNKNOWN,
                    Method = Enum.TryParse(methodHeader, out SocketMethod method) ? method : SocketMethod.UNKNOWN,
                    Entity = entityHeader,
                    Action = actionHeader,
                    Body = body
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static string BuildSocketMessage(SocketMessage message)
        {
            return $"SOURCE:{message.Source}\nMETHOD:{message.Method}\nENTITY:{message.Entity}\nACTION:{message.Action}\nBODY:{message.Body}";
        }
    }
}
