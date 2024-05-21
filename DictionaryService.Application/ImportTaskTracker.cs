using System.Collections.Concurrent;
using Common.Exceptions;

namespace DictionaryService.Application;

public class ImportTaskTracker
{
    private readonly ConcurrentDictionary<Guid, Task> _importTasks = new ConcurrentDictionary<Guid, Task>();

    public void AddTask(Guid taskId, Task importTask)
    {
        _importTasks.TryAdd(taskId, importTask);

    }

    public TaskStatus GetTaskStatus(Guid taskId)
    {
        if (_importTasks.TryGetValue(taskId, out var task))
        {
            return task.Status;
        }

        throw new NotFound($"Task with id = {taskId} does not exist");
    }

    private void RemoveTask(Guid taskId)
    {
        _importTasks.TryRemove(taskId, out _);
    }
}