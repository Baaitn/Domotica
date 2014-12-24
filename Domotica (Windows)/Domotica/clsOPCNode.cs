using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpcLabs.EasyOpc;
using OpcLabs.EasyOpc.DataAccess;

namespace Domotica
{
    public class clsOPCNode
    {
        private clsOPCServer _server;
        private DANodeElement _node;
        private Type _ValueType;
        public clsOPCNode(clsOPCServer server, DANodeElement node)
        {
            this._server = server;
            this._node = node;
            if (IsLeaf)
            {
                object o = (object)(clsOPCServer._client.ReadItemValue(Environment.MachineName, _server.Server.ProgId, _node.ItemId));
                this._ValueType = o.GetType();
            }
        }
        public Boolean IsLeaf 
        { 
            get { return _node.IsLeaf; } 
        }
        public String ItemId 
        { 
            get { return _node.ItemId; } 
        }
        public String Name 
        { 
            get { return _node.Name; }
        }
        public List<clsOPCNode> GetOPCNodes()
        {
            if (IsLeaf) return new List<clsOPCNode>();
            DANodeFilter filter = new DANodeFilter(DABrowseFilter.All);
            DANodeElementCollection nodes = clsOPCServer._client.BrowseNodes(Environment.MachineName, _server.Server.ProgId, _node.ItemId, filter);
            List<clsOPCNode> list = new List<clsOPCNode>(nodes.Count);
            foreach (DANodeElement node in nodes)
            {
                if (!node.Name.StartsWith("_") && !node.Name.EndsWith("Write"))
                {
                    list.Add(new clsOPCNode(_server, node));
                }
            }
            List<clsOPCNode> sortedlist = list.OrderBy(node => node.ItemId).ToList(); //Sorteren om nodes met dezelfde prefix te groeperen.
            return sortedlist;
        }
        public override string ToString()
        {
            if (IsLeaf)
            {
                return "" + _node.ItemId + ": " + Value;
            }
            else
            {
                return "" + _node.ItemId;
            }
        }
        public object Value
        {
            get
            {
                if (!IsLeaf) return null;
                object o = clsOPCServer._client.ReadItemValue(Environment.MachineName, _server.Server.ProgId, _node.ItemId, VarType.Empty, 1); //ValueAge is belangrijk! Erzonder worden waarden veranderd in KEPServer maar word de ingestelde waarde ook terug vervangen door de oude.
                return o;
            }
            set
            {
                if (_ValueType == typeof(Boolean))
                {
                    Boolean waarde;
                    if (value.GetType() == typeof(Boolean))
                    {
                        waarde = (Boolean)value;
                    }
                    else if (value.GetType() == typeof(String))
                    {
                        if (value.Equals("1") | value.Equals("0")) { waarde = Convert.ToBoolean(Convert.ToInt16(value)); }
                        else { waarde = Boolean.Parse(value as String); }
                    }
                    else
                    {
                        throw new Exception("Ongeldige waarde: " + value.ToString() + Environment.NewLine + "Geldige waarden zijn 0, 1, True of False.");
                    }
                    clsOPCServer._client.WriteItemValue(Environment.MachineName, _server.Server.ProgId, Truncate(_node.ItemId) + "Write", waarde);
                }
                else if (_ValueType == typeof(Double))
                {
                    Double waarde;
                    if (value.GetType() == typeof(Double))
                    {
                        waarde = (Double)value;
                    }
                    else if (value.GetType() == typeof(String))
                    {
                        waarde = Double.Parse(value as String);
                    }
                    else
                    {
                        throw new Exception("Ongeldige waarde: " + value.ToString());
                    }
                    clsOPCServer._client.WriteItemValue(Environment.MachineName, _server.Server.ProgId, Truncate(_node.ItemId) + "Write", waarde);
                }
                else if (_ValueType == typeof(Int32))
                {
                    Int32 waarde;
                    if (value.GetType() == typeof(Int32))
                    {
                        waarde = (Int32)value;
                    }
                    else if (value.GetType() == typeof(String))
                    {
                        waarde = Int32.Parse(value as String);
                    }
                    else
                    {
                        throw new Exception("Ongeldige waarde: " + value.ToString() + Environment.NewLine + "Geldige waarden gaan van −2,147,483,648 tot 2,147,483,647.");
                    }
                    clsOPCServer._client.WriteItemValue(Environment.MachineName, _server.Server.ProgId, Truncate(_node.ItemId) + "Write", waarde);
                }
                else 
                {
                    throw new Exception("Onbekend type: " + value.GetType());
                }
            }
        }
        private String Truncate(String text)
        {
            //Suffix: Vanaf 1ste tot en met X voor het laatste teken overhouden.
            if (text.EndsWith("Read"))
            {
                text = text.Substring(0, text.Length - 4); //-4 omdat 'Read' 4 tekens is
            }
            if (text.EndsWith("Write"))
            {
                text = text.Substring(0, text.Length - 5);
            }
            //Return wat overblijft
            return text;
        }
    }
}
