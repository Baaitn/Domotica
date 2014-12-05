using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Domotica
{
    public class clsOPCTRVNode : TreeViewItem
    {
        private clsOPCNode _node;
        public clsOPCTRVNode(clsOPCNode node)
        {
            this._node = node;
            this.Header = node.ToString();
            ToonSubNodes();
        }
        private void ToonSubNodes() 
        {
            foreach (clsOPCNode node in _node.GetOPCNodes())
            {
                this.Items.Add(new clsOPCTRVNode(node));
            }
        }
    }
}
