using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.Models
{
    public class LinearAcceleration : IAccelerationMethod
    {
        public float Beta => (float)1/6;
        public float Gamma => (float)1/2;
    }
}
