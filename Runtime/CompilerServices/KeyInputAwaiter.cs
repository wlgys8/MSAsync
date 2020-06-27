using UnityEngine;
using System;
using System.Runtime.CompilerServices;

namespace MS.Async.CompilerServices{

    public struct KeyInputAwait: ICriticalNotifyCompletion{
        private static Action<object> _invokeContinuation = (p)=>{
            var continuation = (p as Action);
            continuation();
        };

        private KeyCode _keycode;
        private KeyInputType _type;

        public KeyInputAwait(KeyCode keyCode,KeyInputType type){
            _keycode = keyCode;
            _type = type;
        }

        public void GetResult(){

        }

        public bool IsCompleted{
            get{
                switch(_type){
                    case KeyInputType.Down:
                    return Input.GetKeyDown(_keycode);
                    case KeyInputType.Up:
                    return Input.GetKeyUp(_keycode);
                    case KeyInputType.Any:
                    return Input.GetKey(_keycode);
                }
                return false;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            UnityLoops.ScheduleKeyInput(new UnityLoops.KeyInputTask(){
                keyCode = _keycode,
                type = _type,
                action = _invokeContinuation,
                state = continuation,
            });
        }
    }
}
