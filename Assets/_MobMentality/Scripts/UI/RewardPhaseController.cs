using System;
using System.Collections.Generic;
using MobMentality.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace MobMentality.UI
{
    /// <summary>Shows three reward choices and forwards the selected card.</summary>
    public sealed class RewardPhaseController : MonoBehaviour
    {
        private GameObject panel;
        private Text title;
        private Button[] buttons;

        /// <summary>Connects the runtime-created reward controls.</summary>
        public void Initialize(GameObject rewardPanel, Text rewardTitle, Button[] rewardButtons)
        {
            panel = rewardPanel;
            title = rewardTitle;
            buttons = rewardButtons;
            Hide();
        }

        /// <summary>Displays cards and invokes the callback once a choice is made.</summary>
        public void Show(IReadOnlyList<UpgradeCard> cards, Action<UpgradeCard> onSelected)
        {
            panel.SetActive(true);
            title.text = "ARMY WINS!\nChoose a mob upgrade";
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                button.onClick.RemoveAllListeners();
                bool hasCard = i < cards.Count;
                button.gameObject.SetActive(hasCard);
                if (!hasCard)
                    continue;

                UpgradeCard card = cards[i];
                button.GetComponentInChildren<Text>().text = $"{card.Name}\n{card.Description}\n[{card.Rarity}]";
                button.onClick.AddListener(() => onSelected(card));
            }
        }

        /// <summary>Displays the terminal result after the wizard wins.</summary>
        public void ShowGameOver()
        {
            panel.SetActive(true);
            title.text = "WIZARD WINS\nGAME OVER";
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].gameObject.SetActive(false);
            }
        }

        /// <summary>Closes the reward panel.</summary>
        public void Hide()
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}
