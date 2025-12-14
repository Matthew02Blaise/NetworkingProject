using Unity.Netcode.Components;
using UnityEngine;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{



    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        /// <summary>
        /// Determines who can write to this transform. In this case, only the owner client.
        /// This imposes state to the server, placing trust on your clients.
        /// Ensure no security-sensitive features use this transform.
        /// </summary>
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }

}