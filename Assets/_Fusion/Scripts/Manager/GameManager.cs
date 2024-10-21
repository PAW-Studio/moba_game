namespace FusionNetwork
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        #region PUBLIC_VAR
        public CameraFollow cameraFollow;
        public int selectedCharacterIndex = 0;
        public int maxPlayer = 2;
        #endregion
    }
}