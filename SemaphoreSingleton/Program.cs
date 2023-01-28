var myClass = new MyClass();

var threads = new List<Thread>();

// list for output of threads
var outputList = new List<string>();

for (int i = 0; i < 3; i++)
{
    // tmp is used to have correct i value (when start for parallel, i is 3 for all threads)
    var tmp = i;

    threads.Add(new Thread(() =>
        {
            outputList.Add(myClass.GetValue(tmp));
        })
    {
        Name = $"Thread: {i}"
    });
}

Parallel.ForEach(threads, thread =>
{
    // execute GetValue methods
    thread.Start();
    // get output of thread and set into outputList
    thread.Join();
});

foreach (var item in outputList)
{
    Console.WriteLine(item);
}



class MyClass
{
    readonly Semaphore semaphore = new(1, 1);

    private int? _Value = null;

    public string GetValue(int value)
    {
        var threadName = Thread.CurrentThread.Name;

        Console.WriteLine($"{threadName} is waiting to enter the critical section.");
        semaphore.WaitOne();

        Console.WriteLine($"{threadName} is inside the critical section now.");
        if (_Value is null)
        {
            Console.WriteLine($"{threadName} is setting the value");
            _Value = value;
        }


        Console.WriteLine($"{threadName} is releasing the critical section.");
        semaphore.Release();

        Console.WriteLine($"{threadName} is using value = {_Value}");
        return $"{threadName} output is {_Value}";
    }
}

