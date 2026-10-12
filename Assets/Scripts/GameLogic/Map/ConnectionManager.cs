using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class ConnectionManager : MonoBehaviour
    {
        public static ConnectionManager Instance;

        [ShowInInspector]
        public List<Connection> connections;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
