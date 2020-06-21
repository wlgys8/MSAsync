
namespace MS.Async{
    using CompilerServices;

    /// <summary>
    /// 在非Unity线程中await时，可以将后面的代码切换到Unity线程上运行。
    /// 
    /// 在Unity线程上Await时，后面的代码会同步执行。
    /// </summary>
    public struct UnityThread
    {
        public UnityThreadAwaiter GetAwaiter(){
            return new UnityThreadAwaiter();
        }
    }
}
