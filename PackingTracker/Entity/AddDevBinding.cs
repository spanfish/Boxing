using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.newtronics.Entity
{
    public class AddDevBinding
    {
        public int Status { get; set; }

        public string Msg { get; set; }

        public Retdata Retdata { get; set; }
    }

    public class Retdata
    {
        public List<Errdid> ErrdidList { get; set; }
        
    }

    public class Errdid
    {
        public string Did { get; set; }
        public int DidStatus { get; set; }
        public string StatusInfo { get; set; }
    }
}
