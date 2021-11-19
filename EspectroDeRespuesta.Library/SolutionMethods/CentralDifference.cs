using EspectroDeRespuesta.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.SolutionMethods
{
    public class CentralDifference : SolverBase
    {
        private float _previousPosition = 0;
        private float _deltaTSquared = 0;
        private float _factor1 = 0;
        private float _factor2 = 0;
        private float _factor3 = 0;

        public CentralDifference(StructuralProperties structuralProperties, ForceRecordings forces) 
            : base(structuralProperties, forces)
        {            
        }

        public override void Calculate()
        {
            CalculateFactors();
            InitialConditions();

            for (int i = 0; i < _forces.TotalRecords - 1; i++)
            {
                Position[i + 1] = (_forces.Forces[i].Load + _factor2 * Position[i] - _factor3 * _previousPosition) / _factor1;

                _previousPosition = Position[i];
            }
        }

        private void InitialConditions()
        {
            var initialPosition = Position[0];
            var initialVelocity = 0f;
            var initialAcceleration = 0f;

            _previousPosition = initialPosition - _forces.DeltaT * initialVelocity + _deltaTSquared / 2 * initialAcceleration;
        }

        private void CalculateFactors()
        {
            var _deltaTSquared = MathF.Pow(_forces.DeltaT, 2);

            _factor1 = _structuralProperties.Mass / _deltaTSquared + _structuralProperties.Damping / (2 * _forces.DeltaT);
            _factor2 = 2 * _structuralProperties.Mass / _deltaTSquared - _structuralProperties.Stiffness;
            _factor3 = _structuralProperties.Mass / _deltaTSquared - _structuralProperties.Damping / (2 * _forces.DeltaT);
        }
    }
}
