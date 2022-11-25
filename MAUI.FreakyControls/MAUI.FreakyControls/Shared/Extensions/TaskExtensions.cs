using System;
namespace Maui.FreakyControls.Extensions;

public static class TaskExtensions
{
    /// <summary> 
    /// Runs the Task in a concurrent thread without waiting for it to complete. This will start the task if it is not already running. 
    /// </summary> 
    /// <param name="task">The task to run.</param> 
    /// <remarks>This is usually used to avoid warning messages about not waiting for the task to complete.</remarks> 
    public static void RunConcurrently(this Task task)
    {
        if (task == null)
            throw new ArgumentNullException("task", "task is null.");

        if (task.Status == TaskStatus.Created)
            task.Start();
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        {
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }
            throw new TimeoutException();
        }
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task task, TimeSpan timeout)
    {
        using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        {
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                await task;
            }
            throw new TimeoutException();
        }
    }
}