#if RELEASE
using System.Text;
using Fallen_LE_Mods.Dev;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using static Fallen_LE_Mods.Dev.GameStatsTracker;

namespace Fallen_LE_Mods.MonoScripts
{
    [RegisterTypeInIl2Cpp]
    public class CornerUpdater : MonoBehaviour
    {
        public TextMeshProUGUI? textToUpdate;
        public GameObject? backgroundObject;
        public float updateInterval = 1f;

        private float timer = 0f;
        private bool isVisible = true;
        private const KeyCode ToggleKey = KeyCode.F1;

        public CornerUpdater(IntPtr ptr) : base(ptr) { }

        private void Update()
        {
            if (Input.GetKeyDown(ToggleKey))
            {
                ToggleVisibility();
            }

            if (!isVisible || textToUpdate == null) return;

            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                UpdateUIText();
            }
        }

        private void ToggleVisibility()
        {
            isVisible = !isVisible;

            if (textToUpdate != null)
                textToUpdate.gameObject.SetActive(isVisible);

            if (backgroundObject != null)
                backgroundObject.SetActive(isVisible);
        }

        private void UpdateUIText()
        {
            StringBuilder infos = new();

            int expPerHour = Mathf.Max(Mathf.RoundToInt(totalExp / GetElapsedTime() * 3600f), 0);
            TimeSpan estimatedTimeToLevelUp = expPerHour > 0
                ? TimeSpan.FromHours((double)expToNextLevel / expPerHour)
                : TimeSpan.Zero;

            infos.AppendLine($"Status  : {(IsPaused ? "Paused" : "Active")}");
            infos.AppendLine($"Gold/h  : {Mathf.Max(Mathf.RoundToInt(totalGold / GetElapsedTime() * 3600f), 0)}");
            infos.AppendLine($"Exp/h   : {expPerHour}");
            infos.AppendLine($"lvl↑ in : {estimatedTimeToLevelUp:hh\\:mm\\:ss}");
            infos.AppendLine($"Rep/h   : {Mathf.Max(Mathf.RoundToInt(totalRep / GetElapsedTime() * 3600f), 0)}");
            infos.AppendLine($"Favor/h : {Mathf.Max(Mathf.RoundToInt(totalFavor / GetElapsedTime() * 3600f), 0)}");
            infos.AppendLine($"DPS     : {Mathf.RoundToInt(dps)}");
            infos.AppendLine($"DPS(avg): {Mathf.RoundToInt(DamageTracker.averageDps)}");
            infos.AppendLine($"DPS(max): {Mathf.RoundToInt(DamageTracker.maxDps)}");
            infos.AppendLine($"DMG Ttl : {Mathf.RoundToInt(DamageTracker.totalDamageDealt)}");

            textToUpdate.text = infos.ToString();
            InfoCorner.ResizeBackgroundToText();
        }
    }
}
#endif