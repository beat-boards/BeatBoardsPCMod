using System;    
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRUI;
using UnityEngine.UI;
using CustomUI.BeatSaber;

namespace BeatBoards.UI.ViewControllers
{
    class BackButtonNavigationController : VRUINavigationController
    {
        public event Action didFinishEvent;

        private Button _backButton;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                _backButton = BeatSaberUI.CreateBackButton(rectTransform, () => { didFinishEvent?.Invoke(); });
            }
        }
    }
}
