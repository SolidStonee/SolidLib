using LethalLib;
using SolidLib.Registry;
using Unity.Netcode;
using UnityEngine;

namespace SolidLib.Components.Sound
{
    public class SSoundManager : NetworkBehaviour
    {
        [SerializeField]
        private AudioSource[] audioSources;
        [SerializeField]
        private GameObject audioSourcePrefab;

        private AudioSourceHandle[] handles;
        private int handleIndex = 0;

        public static SSoundManager main;

        private void Awake()
        {
            main = this;
            handles = new AudioSourceHandle[audioSources.Length];
            for (int i = 0; i < handles.Length; i++)
            {
                handles[i] = new AudioSourceHandle(audioSources[i], Vector3.zero, null);
            }
        }

        public AudioSourceHandle RequestHandle(Vector3 localPosition, Transform parent = null)
        {
            SolidLib.LogExtended("Requesting Sound Handle A");
            for (int i = 0; i < audioSources.Length; i++)
            {
                handleIndex = (handleIndex + 1) % audioSources.Length;
                var candidate = handles[handleIndex];
                if (!candidate.exists || !candidate.playing)
                {
                    break;
                }
            }

            SolidLib.LogExtended("Requesting Sound Handle B");
            
            var handle = handles[handleIndex];
            if (!handle.exists)
            {
                var audioSourcePrefabInstance = GameObject.Instantiate(audioSourcePrefab);
                var newAudioSource = audioSourcePrefabInstance.GetComponent<AudioSource>();
                audioSources[handleIndex] = newAudioSource;
                handle = new AudioSourceHandle(newAudioSource, localPosition, parent);
                handles[handleIndex] = handle;
            }
            else
            {
                handle.Setup(localPosition, parent);
            }

            SolidLib.LogExtended("Requesting Sound Handle C");
            return handle;
        }

        private AudioClip GetAudioClipFromNameRegistrar(string clipName)
        {
            if (!Registries.SoundRegistry.TryGetValue(clipName, out AudioClip clip))
            {
                SolidLib.Log.LogWarning(
                    $"[SoundManager] Failed to get {clipName} from SoundRegistry, Are you sure it exists?");
                return null;
            }

            return clip;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SyncTransformServerRpc(int handleIndex, Vector3 localPosition, ulong parentNetworkObjectId)
        {
            SyncTransformClientRpc(handleIndex, localPosition, parentNetworkObjectId);
        }

        [ClientRpc]
        private void SyncTransformClientRpc(int handleIndex, Vector3 localPosition, ulong parentNetworkObjectId)
        {
            var handle = handles[handleIndex];
            Transform parent = null;

            // Find the parent transform using the NetworkObjectId
            if (parentNetworkObjectId != ulong.MaxValue)
            {
                var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parentNetworkObjectId];
                parent = networkObject ? networkObject.transform : null;
            }

            // Set up the handle with the correct local position and parent
            handle.Setup(localPosition, parent);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlaySoundServerRpc(int handleIndex, string clipName, float volume, float pitch, float maxDistance, bool oneShot = false, float delay = 0f)
        {
            SolidLib.LogExtended($"Calling PlaySoundServerRpc {clipName}");
            PlaySoundClientRpc(handleIndex, clipName, volume, pitch, maxDistance, oneShot, delay);
        }

        [ClientRpc]
        private void PlaySoundClientRpc(int handleIndex, string clipName, float volume, float pitch, float maxDistance, bool oneShot = false, float delay = 0f, ClientRpcParams clientRpcParams = default)
        {
            var handle = handles[handleIndex];
            SolidLib.LogExtended($"Calling PlaySoundClientRpc {clipName}");
            handle.Play(GetAudioClipFromNameRegistrar(clipName), volume, pitch, maxDistance, oneShot, delay);
        }

        [ServerRpc(RequireOwnership = false)]
        public void StopSoundServerRpc(int handleIndex)
        {
            StopSoundClientRpc(handleIndex);
        }

        [ClientRpc]
        public void StopSoundClientRpc(int handleIndex)
        {
            var handle = handles[handleIndex];
            handle.Stop();
        }

        public class AudioSourceHandle
        {
            private AudioSource source;
            public bool exists { get { return source != null; } }
            public float volume { get { if (source) { return source.volume; } else { return 0.0f; } } set { if (source) { source.volume = value; } } }
            public float pitch { set { if (source) { source.pitch = value; } } }
            public bool loop { set { if (source) { source.loop = value; } } }
            public float maxDistance { set { if (source) { source.maxDistance = value; } } }
            public bool playing { get { return source ? source.isPlaying : false; } }

            public AudioSourceHandle(AudioSource _source, Vector3 localPosition, Transform parent)
            {
                source = _source;
                Setup(localPosition, parent);
            }

            public void Setup(Vector3 localPosition, Transform parent)
            {
                source.transform.position = localPosition;
                source.transform.SetParent(parent, true);
                source.loop = false;
                SolidLib.LogExtended($"Initialize sound at {localPosition}");
            }

            public void Play(AudioClip clip, float sVolume = 1f, float sPitch = 1f, float sMaxDistance = 16f, bool oneShot = false, float delay = 0f)
            {
                if (source && clip)
                {
                    volume = sVolume;
                    pitch = sPitch;
                    maxDistance = sMaxDistance;
                    SolidLib.LogExtended($"Playing Sound {clip.name} at: {source.transform.position} OneShot: {oneShot}");
                    if (!oneShot)
                    {
                        source.clip = clip;
                        if (delay > 0f)
                        {
                            source.PlayDelayed(delay);
                        }
                        else
                        {
                            source.Play();
                        }
                        
                    }
                    else
                    {
                        source.PlayOneShot(clip, volume);
                    }
                        
                }
            }

            public void Stop()
            {
                if (source)
                {
                    source.Stop();
                }
            }
        }

        public AudioSourceHandle PlaySound(string clipName, Vector3 position, float volume = 1f, float pitch = 1f, float maxDistance = 15f, bool useRpc = true, Transform parent = null, NetworkObject netObj = null, float delay = 0f, bool oneShot = false)
        {
            var handle = RequestHandle(position, parent);
            if (useRpc)
            {
                if (IsServer)
                {
                    SolidLib.LogExtended($"Is Server So Calling ClientRPC to play sound {clipName}");
                    handle.Play(GetAudioClipFromNameRegistrar(clipName), volume, pitch, maxDistance, oneShot, delay);
                    PlaySoundClientRpc(handleIndex, clipName, volume, pitch, maxDistance, oneShot, delay);
                }
                else
                {
                    SolidLib.LogExtended($"Else is not server So Calling ServerRPC to play sound {clipName}");
                    PlaySoundServerRpc(handleIndex, clipName, volume, pitch, maxDistance, oneShot, delay);
                }
                if (netObj != null && parent != null)
                {
                    if (IsServer)
                    {
                        SyncTransformClientRpc(handleIndex, position, netObj.NetworkObjectId);
                    }
                    else
                    {
                        SyncTransformServerRpc(handleIndex, position, netObj.NetworkObjectId);
                    }
                }
            }
            else
            {
                handle.Play(GetAudioClipFromNameRegistrar(clipName), volume, pitch, maxDistance, oneShot, delay);
            }
            
            return handle;
        }
        
        public AudioSourceHandle PlaySoundForClient(ulong clientId, string clipName, Vector3 position, float volume = 1f, float pitch = 1f, float maxDistance = 15f, Transform parent = null, float delay = 0f, bool oneShot = false)
        {
            var handle = RequestHandle(position, parent);
            int handleIndex = System.Array.IndexOf(handles, handle);

            if (IsServer)
            {
                handle.Play(GetAudioClipFromNameRegistrar(clipName), volume, pitch, maxDistance, oneShot, delay);
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientId }
                    }
                };
                PlaySoundClientRpc(handleIndex, clipName, volume, pitch, maxDistance, oneShot, delay, clientRpcParams);
            }

            return handle;
        }
        
        public void StopSound(AudioSourceHandle handle)
        {
            int handleIndex = System.Array.IndexOf(handles, handle);
            if (handleIndex == -1) return;

            if (IsServer)
            {
                handle.Stop();
                StopSoundClientRpc(handleIndex);
            }
            else
            {
                StopSoundServerRpc(handleIndex);
            }
        }
    }
}