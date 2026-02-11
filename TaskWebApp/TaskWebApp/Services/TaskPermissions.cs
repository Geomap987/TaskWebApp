using System.Diagnostics.CodeAnalysis;
using TaskWebApp.DbStuff.Models;

namespace TaskWebApp.Services
{
    public class TaskPermissions
    {
        private AuthService _authService;

        public TaskPermissions(AuthService authService)
        {
            _authService = authService;
        }
        public bool CanDeleteTask(TaskInfo task)
        {
            bool isEvenMinute = DateTime.Now.Minute % 2 == 0;

            return isEvenMinute && task.Owner?.Id == _authService.GetCurrentUserId();
        }
    }
}
