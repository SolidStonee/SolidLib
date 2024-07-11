using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SolidLib.Components
{
    /// <summary>
    /// Put this on a empty gameobject in the scene.
    /// Sets the music to play for this moon (Happens Whenever the DayMode gets changed, Dawn, Noon, Sundown, Midnight)
    /// </summary>
    public class API_MoonMusic : MonoBehaviour
    {

        public AudioClip[] dawnMusicCues;

        public AudioClip[] noonMusicCues;

        public AudioClip[] sundownMusicCues;

        public AudioClip[] midnightMusicCues;

        public AudioClip[] ambientDayCues;

        public AudioClip[] ambientEveningCues;

    }
}
