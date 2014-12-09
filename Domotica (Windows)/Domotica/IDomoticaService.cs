using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Domotica
{
    [ServiceContract]
    public interface IDomoticaService
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<clsWCFNode> GetWCFNodes();
    }
}
