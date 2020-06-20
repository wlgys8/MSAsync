using System;

namespace MS.Async{
    using CompilerServices;

    /// <summary>
    /// 相比于WaitForSeceonds, Delay的实现不会产生GC Alloc. 当代码异步执行时，会运行在Unity的Update周期上
    /// </summary>
    public struct Delay
    {
        private float _seconds;
        public Delay(float seconds){
            _seconds = seconds;
        }   

        public TimeAwaiter GetAwaiter(){
            return new TimeAwaiter(DateTime.Now + TimeSpan.FromSeconds(_seconds));
        }
    }

}
