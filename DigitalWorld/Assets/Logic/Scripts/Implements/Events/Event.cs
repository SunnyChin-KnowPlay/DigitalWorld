using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic.Events
{
    public partial struct Event
    {
        public EEvent EventId
        {
            get
            {
                return (EEvent)Id;
            }
        }
    }
}
