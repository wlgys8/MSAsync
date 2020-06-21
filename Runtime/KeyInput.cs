using UnityEngine;


namespace MS.Async{
    using CompilerServices;
    public struct KeyInput
    {
       
        private KeyCode _keycode;
        private KeyInutType _type;
        public KeyInput(KeyCode code,KeyInutType type){
            _keycode = code;
            _type = type;
        }

        public KeyInputAwait GetAwaiter(){
            return new KeyInputAwait(_keycode,_type);
        }
    }


    public enum KeyInutType{
        Down,
        
        Up,
        Any,
    }
}
