using MobMentality.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MobMentality.UI
{
    /// <summary>Renders the current run state into a simple text HUD.</summary>
    public sealed class HudController : MonoBehaviour
    {
        private Text statusText;

        /// <summary>Connects the runtime-created HUD label.</summary>
        public void Initialize(Text label) => statusText = label;

        /// <summary>Refreshes all values shown by the HUD.</summary>
        public void Refresh(GameManager game)
        {
            if (statusText == null || game == null)
                return;

            statusText.text =
                $"Wave: {game.Waves.CurrentWave}\n" +
                $"State: {game.State}\n" +
                $"Mob: Lv {game.MobArmy.Level}  Strength {game.MobArmy.Stats.Strength:0}\n" +
                $"Boss: Lv {game.BossWizard.Level}  Strength {game.BossWizard.Stats.Strength:0}";
        }
    }
}
