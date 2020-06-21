# Unity Async Awaiters

针对c#的await-async语法，实现了Unity的一些异步等待对象。


# Features

* await all yield instructions 
    
    可以等待所有可yield的对象

* await UnityEvent

    可以等待UnityEvent对象

* await Delay - 0 gc alloc

    0 gc的Delay实现

* await ManualSingal

    可以等待手动激活的信号量

* await UnityThread

    可以在其他线程中，将后续代码切到Unity主线程上执行


# Usage

## 1. Wait UnityEvent

```csharp
async void Start(){
    await button.onClick;
    //this code will be executed after button was clicked
    Debug.Log("click button success");
}
```

## 2. Wait YieldInstruction

```csharp

async void Start(){
    await new WaitForSeconds(1);
    //execute after 1 second
    Debug.Log("After 1 second");
}
```

## 3 . Wait Delay

Delay是无GC的实现，建议用此代替WaitForSeconds

```csharp
async void Start(){
    await new Delay(1); //no gc alloc
    //execute after 1 second
    Debug.Log("After 1 second");
}
```

## 4. Wait ManualSingal

```csharp

ManualSingal singal = new ManualSingal();

async void Start(){
    try{
        await singal; //
        //execute after singal.SetResult() was called
        Debug.Log("get singal result");
    }catch(OperationCanceledException e){
        //goto here if singal.Cancel() or singal.Reset() was called
    }catch(AggregateException e){
        //goto here if singal.SetException() was called
    }
}
```

## 5. Wait UnityThread

```csharp

async void Start(){

    Task.Run(async ()=>{
        Debug.Log("Run in Background Thread:" + Thread.CurrentThread.ManagedThreadId);
        await new UnityThread();
        //change context back to unity thread
        Debug.LogFormat("After await UnityThread, threadId = {0}",Thread.CurrentThread.ManagedThreadId);        
    })
}

```

