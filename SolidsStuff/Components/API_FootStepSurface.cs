using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SolidLib.Components
{

    public class API_FootStepSurface : MonoBehaviour
    {
        [Tooltip("Footsteps")]
        public AudioClip[] clips;

        [Tooltip("Landing")]
        public AudioClip hitSurfaceSFX;

    }
}
