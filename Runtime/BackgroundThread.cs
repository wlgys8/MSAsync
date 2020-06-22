using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Async{
    using CompilerServices;
    public struct BackgroundThread 
    {
        private BackgroundThreadType _type;
        public BackgroundThread(BackgroundThreadType type){
            _type = type;
        }

        public BackgroundThreadAwaiter GetAwaiter(){
            return new BackgroundThreadAwaiter(_type);
        }
    }

    /// <summary>
    /// 后台线程分配策略
    /// </summary>
    public enum BackgroundThreadType{
        /// <summary>
        /// 仅当在Unity主线程上await时，才分配新线程。
        /// 如果本身已经运行在后台线程上，那么仍然继续在当前线程上运行。
        /// </summary>
        SwitchOnlyInUnityThread,

        /// <summary>
        /// 无论当前运行在什么线程上，都尽量切换一个新的线程运行上下文. 并不一定保证切换成功(比如当前线程已经闲置)，取决于.Net内部的线程调度。
        /// </summary>      
        SwitchAnyway,
    }
}
