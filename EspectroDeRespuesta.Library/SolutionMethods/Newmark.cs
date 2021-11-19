using EspectroDeRespuesta.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EspectroDeRespuesta.Library.SolutionMethods
{
    public class Newmark : SolverBase
    {
        private readonly IAccelerationMethod _accelerationMethod;

        private float _factor1 = 0;
        private float _factor2 = 0;
        private float _factor3 = 0;
        private float _factor4 = 0;

        private float _factor5 = 0;
        private float _factor6 = 0;
        private float _factor7 = 0;
        private float _factor8 = 0;

        public Newmark(StructuralProperties structuralProperties, ForceRecordings forces, IAccelerationMethod accelerationMethod)
            : base(structuralProperties, forces)
        {
            _accelerationMethod = accelerationMethod;
        }

        public override void Calculate()
        {
            CalculateFactors();

            var velocity = 0f;
            var acceleration = InitialAcceleration(velocity);

            for (int i = 0; i < _forces.TotalRecords - 1; i++)
            {
                Position[i + 1] = (
                    _accelerationMethod.Beta * _forces.Forces[i + 1].Load +
                    _factor2 * Position[i] +
                    _factor3 * velocity +
                    _factor4 * acceleration) / _factor1;

                var nextAcceleration = _factor5 * (Position[i + 1] - Position[i]) - 
                    _factor6 * velocity - 
                    _factor7 * acceleration;

                velocity += (
                    _factor8 * acceleration + 
                    _accelerationMethod.Gamma * nextAcceleration
                    ) * _forces.DeltaT;

                acceleration = nextAcceleration;
            }
        }

        private float InitialAcceleration(float velocity)
        {
            var initialPosition = Position[0];
            var initialVelocity = velocity;

            return (_forces.Forces[0].Load -
                _structuralProperties.Stiffness * initialPosition -
                _structuralProperties.Damping * initialVelocity) / _structuralProperties.Mass;
        }

        private void CalculateFactors()
        {
            var deltaTSquared = MathF.Pow(_forces.DeltaT, 2);

            _factor1 = _structuralProperties.Mass / deltaTSquared +
                _accelerationMethod.Gamma * _structuralProperties.Damping / _forces.DeltaT +
                _accelerationMethod.Beta * _structuralProperties.Stiffness;

            _factor2 = _structuralProperties.Mass / deltaTSquared +
                _accelerationMethod.Gamma * _structuralProperties.Damping / _forces.DeltaT;

            _factor3 = _structuralProperties.Mass / _forces.DeltaT +
                (_accelerationMethod.Gamma - _accelerationMethod.Beta) * _structuralProperties.Damping;

            _factor4 = (0.5f - _accelerationMethod.Beta) * _structuralProperties.Mass +
                0.5f * _forces.DeltaT * (_accelerationMethod.Gamma - 2 * _accelerationMethod.Beta) * _structuralProperties.Damping;

            _factor5 = 1 / (_accelerationMethod.Beta * deltaTSquared);

            _factor6 = 1 / (_accelerationMethod.Beta * _forces.DeltaT);

            _factor7 = 0.5f / _accelerationMethod.Beta - 1;

            _factor8 = 1 - _accelerationMethod.Gamma;
        }
    }
}
