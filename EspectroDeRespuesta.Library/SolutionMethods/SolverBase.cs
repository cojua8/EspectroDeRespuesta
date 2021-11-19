using EspectroDeRespuesta.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.SolutionMethods
{
    public abstract class SolverBase
    {
        protected readonly StructuralProperties _structuralProperties;
        protected readonly ForceRecordings _forces;
        public IList<float> Position { get; private set; }

        public SolverBase(StructuralProperties structuralProperties, ForceRecordings forces)
        {
            _structuralProperties = structuralProperties;
            _forces = forces;

            Position = new List<float>(Enumerable.Repeat(0f, forces.TotalRecords));
        }
        public abstract void Calculate();
    }
}
