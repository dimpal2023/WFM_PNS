using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wfm.App.EmailServices
{
    public interface ISender
    {
        void Send(Message message);
    }
}
