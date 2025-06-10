using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineUtil
{
    public static IEnumerator WaitForTask(Task task)
    {
        yield return new WaitUntil(() => { return task.IsCompleted; });
    }
}
