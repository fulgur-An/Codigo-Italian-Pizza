using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using Backend.Service;

namespace Server
{
    public class ServerItalianPizzaProxy : DuplexClientBase<IItalianPizzaService>
    {
        public ServerItalianPizzaProxy(IItalianPizzaServiceCallback callbackInstance) : base(callbackInstance)
        { }

    }
}