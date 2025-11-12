using System.Diagnostics;
      using System.Linq;
      using System.Reflection;
      using LethalLib;
      using SolidLib.Registry;
      using Unity.Netcode;
      using UnityEngine;

      namespace SolidLib.Components
      {

          public class ExtendedEnemyAI : EnemyAI
          {
              //
              // public System.Random enemyRandom = null!;
              //
              // [SerializeField] private AudioSource movementSource;
              //
              // public override void Start()
              // {
              //     base.Start();
              //     enemyRandom = new System.Random(StartOfRound.Instance.randomMapSeed + thisEnemyIndex);
              // }
              //
              // public void LogIfDebugBuild(string text)
              // {
              //     // Assembly callingAssembly = Assembly.GetCallingAssembly();
              //     // bool isDebug = callingAssembly
              //     //     .GetCustomAttributes(false)
              //     //     .OfType<DebuggableAttribute>()
              //     //     .Any(da => da.IsJITTrackingEnabled);
              //     //
              //     // if (isDebug)
              //     // {
              //         SolidLib.Log.LogInfo(text);
              //     //}
              // }
              //
              public virtual void AnimationEventC()
              {
                  
              }
              
              public virtual void AnimationEventD()
              {
                  
              }
              
              public virtual void AnimationEventE()
              {
                  
              }
              
              public virtual void AnimationEventStringA(string value)
              {
                  
              }
              
              public virtual void AnimationEventStringB(string value)
              {
                  
              }
              
              public virtual void AnimationEventStringC(string value)
              {
                  
              }
              
              public virtual void AnimationEventFloatA(float value)
              {
                  
              }
              
              public virtual void AnimationEventFloatB(float value)
              {
                  
              }
              
              public virtual void AnimationEventFloatC(float value)
              {
                      
              }
              
              public virtual void AnimationEventBoolA(bool value)
              {
                  
              }
              
              public virtual void AnimationEventBoolB(bool value)
              {
                      
              }
              
              public virtual void AnimationEventBoolC(bool value)
              {
                          
              }
              
              // [ClientRpc]
              // public void DoAnimFloatClientRpc(string parameter, float value)
              // {
              //     LogIfDebugBuild($"Set AnimFloat: {parameter} To {value}");
              //     creatureAnimator.SetFloat(parameter, value);
              // }
              //
              // [ClientRpc]
              // public void DoAnimIntegerClientRpc(string parameter, int value)
              // {
              //     LogIfDebugBuild($"Set AnimInteger: {parameter} To {value}");
              //     creatureAnimator.SetInteger(parameter, value);
              // }
              //
              // [ClientRpc]
              // public void DoAnimBoolClientRpc(string parameter, bool value)
              // {
              //     LogIfDebugBuild($"Set AnimBool: {parameter} To {value}");
              //     creatureAnimator.SetBool(parameter, value);
              // }
              //
              // [ClientRpc]
              // public void DoAnimationTriggerClientRpc(string animationName)
              // {
              //     LogIfDebugBuild($"AnimTrigger: {animationName}");
              //     creatureAnimator.SetTrigger(animationName);
              // }
              //
              // [ServerRpc(RequireOwnership = false)]
              // public void PlayCreatureSFXServerRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     PlayCreatureSFXClientRpc(clipName, volume, pitch, maxDistance, oneShot, loop);
              // }
              //
              // [ClientRpc]
              // public void PlayCreatureSFXClientRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     PlayCreatureSFX(clipName, volume, pitch, maxDistance, oneShot, loop);
              // }
              //
              // public void PlayCreatureSFX(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     creatureSFX.loop = loop;
              //     creatureSFX.maxDistance = maxDistance;
              //     creatureSFX.pitch = pitch;
              //     if (oneShot)
              //         creatureSFX.PlayOneShot(GetAudioClipFromNameRegistrar(clipName), volume);
              //     else
              //     {
              //         creatureSFX.Play(volume);
              //     }
              // }
              //
              // [ServerRpc(RequireOwnership = false)]
              // public void PlayCreatureVoiceServerRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     PlayCreatureVoiceClientRpc(clipName, volume, pitch, maxDistance, oneShot, loop);
              // }
              //
              // [ClientRpc]
              // public void PlayCreatureVoiceClientRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     PlayCreatureVoice(clipName, volume, pitch, maxDistance, oneShot, loop);
              // }
              //
              // public void PlayCreatureVoice(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f, bool oneShot = false, bool loop = false)
              // {
              //     creatureVoice.loop = loop;
              //     creatureVoice.maxDistance = maxDistance;
              //     creatureVoice.pitch = pitch;
              //     if (oneShot)
              //         creatureVoice.PlayOneShot(GetAudioClipFromNameRegistrar(clipName), volume);
              //     else
              //     {
              //         creatureVoice.Play(volume);
              //     }
              // }
              //
              // [ServerRpc(RequireOwnership = false)]
              // public void PlayMovementAudioServerRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f)
              // {
              //     PlayMovementAudioClientRpc(clipName, volume, pitch, maxDistance);
              // }
              //
              // [ClientRpc]
              // public void PlayMovementAudioClientRpc(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f)
              // {
              //     PlayMovementAudio(clipName, volume, pitch, maxDistance);
              // }
              //
              // public void PlayMovementAudio(string clipName, float volume = 1f, float pitch = 1f, float maxDistance = 25f)
              // {
              //     movementSource.maxDistance = maxDistance;
              //     movementSource.pitch = pitch;
              //     movementSource.PlayOneShot(GetAudioClipFromNameRegistrar(clipName), volume);
              // }
              //
              // private AudioClip GetAudioClipFromNameRegistrar(string clipName)
              // {
              //     if (!Registries.SoundRegistry.TryGetValue(clipName, out AudioClip clip))
              //     {
              //         SolidLib.Log.LogWarning(
              //             $"[SoundManager] Failed to get {clipName} from SoundRegistry, Are you sure it exists?");
              //         return null;
              //     }
              //
              //     return clip;
              // }
              //
          }
      }