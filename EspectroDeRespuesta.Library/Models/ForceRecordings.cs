using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.Models
{
    public class ForceRecordings
    {
        public IList<TimeForcePair> Forces { get; set; }

        public int TotalRecords => Forces.Count;

        public float DeltaT => Forces[1].Time - Forces[0].Time;
    }
}
