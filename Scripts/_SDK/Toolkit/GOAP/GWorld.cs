using System.Collections;
using UnityEngine;

namespace Assets._SDK.GOAP
{
    public class GWorld : GameSingleton<GWorld>
    {
        public GStateManager stateManager;
        protected override void OnAwake()
        {
            base.OnAwake();
            stateManager = new GStateManager();

            stateManager.SetState(GStateEnum.Init, 0);
        }
    }
}