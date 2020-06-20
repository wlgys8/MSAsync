using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MS.Async.Awaiters.Examples{
    public class AwaitersExample : MonoBehaviour{

        public Button button;
        private ManualSingal _manualSingal = new ManualSingal();

        async void Start(){
            await button.onClick; //wait UnityEvent
            Debug.Log("click button success");
            await new Delay(1);  //wait delay (no gc alloc)
            Debug.Log("after delay");
            await new WaitForSeconds(1); //wait yield instruction
            Debug.Log("after wait for seconds");
            await _manualSingal; //wait manual singal
            Debug.Log("after manual singal");
        }

        void Update(){
            if(Input.GetKeyDown(KeyCode.Space)){
                _manualSingal.SetResult();
            }
        }

    }
}
