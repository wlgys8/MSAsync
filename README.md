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

* await BackgroundThread

    将运行上下文切换为后台线程。

* await Func\<bool>

    等待自定义条件函数


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

## 5. Wait UnityThread & BackgroundThread

对线程环境进行切换

```csharp

async void Start(){
    var unityThreadId = Thread.CurrentThread.ManagedThreadId;
    await new BackgroundThread();
    //below code will run in background thread, and won't block unity thread.
    Debug.Log("run at background thread");
    Debug.Assert(unityThreadId != Thread.CurrentThread.ManagedThreadId);
    Thread.Sleep(1000);
    await new UnityThread(); 
    //go back to unity thread
    Debug.Assert(unityThreadId == Thread.CurrentThread.ManagedThreadId);
    Debug.Log("go back to unity thread");
}

```

## 6. Wait Func<bool>

等待任何自定义的条件函数，直到其返回true. 系统会在每帧的Update调用条件函数，检查其返回值.

```csharp
async void Start(){
    Func<bool> condition = ()=>{
        return Input.GetKeyDown(KeyCode.R);
    };
    await condition;
    //execute code after press R
}

```

## 7. Wait KeyInput

```csharp

async void Start(){
    Debug.Log("press A to continue");
    await new KeyInput(KeyCode.A,KeyInutType.Down);
    Debug.Log("KeyCode.A Down");    
}

```


