using OpcLabs.EasyOpc;
using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domotica
{
    public class clsOPCServer
    {
        public static EasyDAClient _client = new EasyDAClient();
        private ServerElement _server;
        public clsOPCServer(ServerElement element)
        {
            this._server = element;
        }
        public ServerElement Server 
        { 
            get { return _server; }
        }
        public static List<clsOPCServer> GetOPCServers()
        {
            ServerElementCollection servers = _client.BrowseServers(Environment.MachineName);
            List<clsOPCServer> list = new List<clsOPCServer>(servers.Count);
            foreach (ServerElement server in servers)
            {
                list.Add(new clsOPCServer(server));
            }
            return list;
        }
        public List<clsOPCNode> GetOPCNodes()
        {
            DANodeElementCollection nodes = _client.BrowseNodes(Environment.MachineName, _server.ServerClass, "", new DANodeFilter());
            List<clsOPCNode> list = new List<clsOPCNode>(nodes.Count);
            foreach (DANodeElement node in nodes)
            {
                if (!node.ItemId.StartsWith("_"))
                {
                    list.Add(new clsOPCNode(this, node));
                }
            }
            return list;
        }
        public override string ToString()
        {
            return "" + _server.ProgId;
        }
    }
}
