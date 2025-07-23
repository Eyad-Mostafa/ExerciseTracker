using ExerciseTracker.Core.Models;

namespace ExerciseTracker.API.Authorization;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CheckPermissionAttribute : Attribute
{
    public readonly Permission permission;

    public CheckPermissionAttribute(Permission permission)
    {
        this.permission = permission;
    }
}
