
namespace MS.Async{
    using CompilerServices;
    public struct UnityUpdate
    {

        public UpdateAwaiter GetAwaiter(){
            return new UpdateAwaiter();
        }
    }
}
