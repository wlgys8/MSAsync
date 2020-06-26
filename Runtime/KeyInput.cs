using UnityEngine;


namespace MS.Async{
    using CompilerServices;
    public struct KeyInput
    {
       
        private KeyCode _keycode;
        private KeyInputType _type;

        private bool _immediate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <param name="immediate">设为false可以让await KeyInput至少等待一帧。否则如果条件成立，会立即执行await后面的代码</param>
        public KeyInput(KeyCode code,KeyInputType type,bool immediate = false){
            _keycode = code;
            _type = type;
            _immediate = false;
        }

        public KeyInputAwait GetAwaiter(){
            return new KeyInputAwait(_keycode,_type,_immediate);
        }
    }


    public enum KeyInputType{
        Down,
        
        Up,
        Any,
    }
}
