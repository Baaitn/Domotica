using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Domotica
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DomoticaService : IDomoticaService
    {
        public List<clsWCFNode> _WCFNodes;
        private List<clsWCFNode> getWCFNodes() 
        {
            if (clsOPCServer.GekozenServer == null) return null;
            _WCFNodes = clsOPCServer.GekozenServer.GetWCFNodes(); //Verandert van property met 'get{ /*CODE*/ }' naar methode.
            return _WCFNodes;
        }
        public IEnumerable<clsWCFNode> GetWCFNodes() //TODO: Waarom 'IEnumerable<>' ipv 'List<>'?
        {
            return getWCFNodes().ToArray(); //TODO: Verschil tussen dit uit cursus (45/88) en combinatie van de 2 methodes (met 'return _WCFNodes.ToArray()' op eind)?;
        }
    }
}
