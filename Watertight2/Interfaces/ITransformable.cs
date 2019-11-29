using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Watertight.Interfaces
{
    public interface ITransformable
    {
        public Vector3 Location
        {
            get;
            set;
        }

        public Quaternion Rotation
        {
            get;
            set;
        }

        public Vector3 Scale
        {
            get;
            set;
        }

        public Vector3 GetLocation_WorldSpace();
        public Vector3 GetLocation_Relative();
        public Quaternion GetRotation_WorldSpace();
        public Quaternion GetRotation_Relative();
        public Vector3 GetScale_WorldSpace();
        public Vector3 GetScale_Relative();
    }
}
