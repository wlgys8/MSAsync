using UnityEngine;


namespace MS.Async{
    using CompilerServices;
    public struct KeyInput
    {
       
        private KeyCode _keycode;
        private KeyInputType _type;


        /// <summary>
        /// 如果await的时候条件满足，会立即返回并同步执行后面的代码
        /// </summary>
        public KeyInput(KeyCode code,KeyInputType type){
            _keycode = code;
            _type = type;
        }

        public KeyInputAwait GetAwaiter(){
            return new KeyInputAwait(_keycode,_type);
        }
    }


    public enum KeyInputType{
        Down,
        
        Up,
        Any,
    }
}
