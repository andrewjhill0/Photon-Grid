using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.Buttons {
    public abstract class VehicleActionButton : Button {
        // Use this for initialization
        protected override void Start() {

        }

        // Update is called once per frame
        public void Update() {
            //A public function in the selectable class which button inherits from.
            if(IsPressed()) {
                WhilePressed();
            }
        }

        public abstract void WhilePressed();
    }
}
