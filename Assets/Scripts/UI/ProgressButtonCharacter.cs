using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Player;
using Devlike.Timing;
using Devlike.Characters;

namespace Devlike.UI
{
    public class ProgressButtonCharacter : ProgressButton
    {
        public Image characterSprite;
        [HideInInspector]
        public Character character;

        public override void GenerateButton()
        {
            buttonText.SetText(character.Profile.FirstName);
            characterSprite.color = character.Profile.Color;
            action = new PlayerAction(character.Profile.FullName, actionContainer.type, character, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }
    }
}