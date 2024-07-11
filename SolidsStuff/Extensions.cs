using UnityEngine;

namespace SolidLib;

public static class WalkieTalkieExtensions
{
    //zeekerss wtf lmao why not use a transform haha
    public static void TransmitOneShotAudio(Transform audioSource, AudioClip clip, float vol = 1f, float pitch = 1f)
    {
        if (clip == null || audioSource == null)
        {
            return;
        }
        for (int i = 0; i < WalkieTalkie.allWalkieTalkies.Count; i++)
        {
            if (WalkieTalkie.allWalkieTalkies[i].playerHeldBy == null || !WalkieTalkie.allWalkieTalkies[i].clientIsHoldingAndSpeakingIntoThis || !WalkieTalkie.allWalkieTalkies[i].isBeingUsed)
            {
                continue;
            }
            float num = Vector3.Distance(WalkieTalkie.allWalkieTalkies[i].transform.position, audioSource.transform.position);
            if (!(num < WalkieTalkie.allWalkieTalkies[i].recordingRange))
            {
                continue;
            }
            for (int j = 0; j < WalkieTalkie.allWalkieTalkies.Count; j++)
            {
                if (j != i && WalkieTalkie.allWalkieTalkies[j].isBeingUsed)
                {
                    float num2 = Mathf.Lerp(WalkieTalkie.allWalkieTalkies[i].maxVolume, 0f, num / (WalkieTalkie.allWalkieTalkies[i].recordingRange + 3f));
                    WalkieTalkie.allWalkieTalkies[j].target.pitch = pitch;
                    WalkieTalkie.allWalkieTalkies[j].target.PlayOneShot(clip, num2 * vol);
                }
            }
        }
    }
}