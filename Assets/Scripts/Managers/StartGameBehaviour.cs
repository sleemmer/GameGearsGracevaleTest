using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class StartGameBehaviour : MonoBehaviour
    {
        protected List<IGameManager> managers = new List<IGameManager>();

        private void Start()
        {
            CreateManagers();
            StarManagers();
        }

        private void CreateManagers()
        {
            managers.Add(BattleManager.Instance);
            managers.Add(FormManager.Instance);
            managers.Add(SpriteManager.Instance);
        }

        protected void StarManagers()
        {
            for (int i = 0; i < managers.Count; i++)
            {
                IGameManager gm = managers[i];
                if (gm != null)
                {
                    gm.Start();
                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < managers.Count; i++)
            {
                IGameManager gm = managers[i];
                if (gm != null)
                {
                    gm.Update();
                }
            }
        }

        private void UnloadAll()
        {
            for (int i = 0; i < managers.Count; i++)
            {
                IGameManager gm = managers[i];
                if (gm != null)
                {
                    gm.Dispose();
                }
            }
        }
    }
}
