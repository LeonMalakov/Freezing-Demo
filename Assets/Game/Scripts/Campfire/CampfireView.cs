using UnityEngine;

namespace WGame
{
    public class CampfireView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _logs;
        [SerializeField] private CampfireParticleSystem _particleSystem;

        private int _activeLogsCount;

        public void Init()
        {
            foreach (var log in _logs)
                log.enabled = false;
        }

        public void UpdateView(int lifetime, int maxLifetime)
        {
            float lifetimeUnitsPerLog = _logs.Length / (float)maxLifetime;
            int activeLogs = Mathf.CeilToInt(lifetimeUnitsPerLog * lifetime);

            EnableLogs(activeLogs);
            _particleSystem.UpdatePower(lifetime / (float)maxLifetime);
        }

        public void Die()
        {
            _particleSystem.Stop();
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
