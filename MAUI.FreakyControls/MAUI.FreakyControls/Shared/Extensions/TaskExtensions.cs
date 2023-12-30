namespace Maui.FreakyControls.Extensions;

public static class TaskExt
{
    /// <summary>
    /// Runs the Task in a concurrent thread without waiting for it to complete. This will start the task if it is not already running.
    /// </summary>
    /// <param name="task">The task to run.</param>
    /// <remarks>This is usually used to avoid warning messages about not waiting for the task to complete.</remarks>
    public static void RunConcurrently(this Task task)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task), "task is null.");

        if (task.Status == TaskStatus.Created)
            task.Start();
    }

    /// <summary>
    /// A verison of WhenAll that throws all the exceptions encountered!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tasks"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            return await allTasks;
        }
        catch
        {
            //purposely ignore since we will get the exceptions from allTasks;
        }
        throw allTasks.Exception;
    }

    public static async Task WithAggregateException(this Task source)
    {
        try { await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { source.Wait(); }
    }

    public static async Task<T> WithAggregateException<T>(this Task<T> source)
    {
        try { return await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { return source.Result; }
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            return await task;
        }
        throw new TimeoutException();
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task;
        }
        throw new TimeoutException();
    }
}