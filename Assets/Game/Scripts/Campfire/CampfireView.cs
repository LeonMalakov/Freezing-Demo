using UnityEngine;

namespace WGame
{
    public class CampfireView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _logs;
        private int _activeLogsCount;

        public void Init()
        {
            foreach (var log in _logs)
                log.enabled = false;
        }

        public void UpdateLogs(int lifetime, int maxLifetime)
        {
            float lifetimeUnitsPerLog = _logs.Length / (float)maxLifetime;
            int activeLogs = Mathf.CeilToInt(lifetimeUnitsPerLog * lifetime);

            EnableLogs(activeLogs);
        }

        private void EnableLogs(int targetActiveLogsCount)
        {
            if (_activeLogsCount < targetActiveLogsCount)
                for (int i = _activeLogsCount; i < targetActiveLogsCount; i++)
                    _logs[i].enabled = true;
            else if (_activeLogsCount > targetActiveLogsCount)
                for (int i = targetActiveLogsCount; i < _activeLogsCount; i++)
                    _logs[i].enabled = false;

            _activeLogsCount = targetActiveLogsCount;
        }
    }
}
