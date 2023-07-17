using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Model
{
    public enum SocketMethod
    {
        POST,
        PUT,
        UPDATE,
        DELETE,
        OUTPUT,
        UNKNOWN
    }

    public enum SocketSource
    {
        GUI,
        LISTENER,
        UNKNOWN
    }

    public class SocketMessage
    {
        public SocketSource Source { get; set; }

        public SocketMethod Method { get; set; }

        public string Entity { get; set; }

        public string Action { get; set; }

        public string Body { get; set; }

        public SocketMessage()
        {
            Source = SocketSource.GUI;
            Method = SocketMethod.UNKNOWN;
            Entity = "";
            Action = "";
            Body = "";
        }
    }
}
