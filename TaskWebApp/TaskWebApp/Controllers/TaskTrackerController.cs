using Microsoft.AspNetCore.Mvc;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.DbStuff.Repositories;
using TaskWebApp.DbStuff;
using TaskWebApp.Models;
using TaskWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using TaskWebApp.DbStuff.Models.DTOs;

namespace TaskWebApp.Controllers
{
    public class TaskTrackerController : Controller
    {
        public static List<TaskViewModel> taskViewModels = new List<TaskViewModel>();
        private TaskRepository _taskRepository;
        private AuthService _authService;
        private TaskPermissions _taskPermissions;
        private UserRepository _userRepository;

        public TaskTrackerController(TaskRepository taskRepository, AuthService authService, TaskPermissions taskPermissions, UserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _authService = authService;
            _taskPermissions = taskPermissions;
            _userRepository = userRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            var a = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "Привет";
            var dbTasks = _taskRepository.GetTasks();
            var viewModels = dbTasks.Select(dbTask =>
            {
                return new TaskViewModel
                {
                    Id = dbTask.Id,
                    Name = dbTask.Name,
                    Description = dbTask.Description,
                    Priority = dbTask.Priority,
                    Owner = dbTask.Owner?.Login ?? "",
                    Assignees = dbTask.Assignees,
                    CanDelete = _taskPermissions.CanDeleteTask(dbTask)
                };
            }).ToList();
            return View(viewModels);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTask()
        {
            var viewModel = new AddTaskViewModel
            {
                PriorityOptions = new List<int> { 1, 2, 3 },
                AvailableAssignees = _userRepository
                .GetAllUsersBasicInfo()
                .Select(u => new UserViewModel { Id = u.Id, Name = u.Name })
            .ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddTask(AddTaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                taskViewModel.PriorityOptions = new List<int> { 1, 2, 3 };
                taskViewModel.AvailableAssignees = _userRepository.GetAllUsersBasicInfo()
                    .Select(u => new UserViewModel { Id = u.Id, Name = u.Name })
                    .ToList();
                return View(taskViewModel);
            }

            var task = new TaskInfo
            {
                Name = taskViewModel.Name,
                Description = taskViewModel.Description,
                Priority = taskViewModel.Priority,
                Owner = _authService.GetCurrentUser(),
                Assignees = _userRepository.GetUsersByIds(taskViewModel.SelectedAssigneeIds)
            };

            _taskRepository.AddTask(task);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdateTask(int id)
        {
            var dbTask = _taskRepository.GetTaskById(id);
            var viewModel = new AddTaskViewModel
            {
                Id = dbTask.Id,
                Name = dbTask.Name,
                Description = dbTask.Description,
                Priority = dbTask.Priority,
                PriorityOptions = new List<int> { 1, 2, 3 },
                AvailableAssignees = _userRepository
                .GetAllUsersBasicInfo()
                .Select(u => new UserViewModel { Id = u.Id, Name = u.Name })
            .ToList()

            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateTask(AddTaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                taskViewModel.PriorityOptions = new List<int> { 1, 2, 3 };
                taskViewModel.AvailableAssignees = _userRepository
                .GetAllUsersBasicInfo()
                .Select(u => new UserViewModel { Id = u.Id, Name = u.Name })
            .ToList();
                return View(taskViewModel);
            }

            _taskRepository.UpdateTask(taskViewModel);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTask(int id)
        {
            var dbTask = _taskRepository.GetTaskById(id);
            if (!_taskPermissions.CanDeleteTask(dbTask))
            {
                throw new Exception("you don't have access");
            }
            _taskRepository.DeleteTask(id);

            return RedirectToAction("Index");
        }

    }
}
