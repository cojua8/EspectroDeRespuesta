using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.Models
{
    public interface IAccelerationMethod
    {
        float Beta { get; }
        float Gamma { get; }
    }
}
