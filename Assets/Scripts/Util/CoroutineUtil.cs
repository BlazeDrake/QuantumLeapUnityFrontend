using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Provides utility methods for working with Unity coroutines and asynchronous tasks.
/// </summary>
public static class CoroutineUtil
{
    /// <summary>
    /// Yields execution until the specified <see cref="Task"/> has completed.
    /// </summary>
    /// <param name="task">The asynchronous task to wait for.</param>
    /// <returns>An enumerator that can be used with Unity coroutines.</returns>
    public static IEnumerator WaitForTask(Task task)
    {
        yield return new WaitUntil(() => { return task.IsCompleted; });
    }
}
