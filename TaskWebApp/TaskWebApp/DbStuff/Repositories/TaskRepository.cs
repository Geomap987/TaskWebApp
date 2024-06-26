﻿using Microsoft.EntityFrameworkCore;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.Models;

namespace TaskWebApp.DbStuff.Repositories
{
    public class TaskRepository : BaseRepository<TaskInfo>
    {
        public TaskRepository(WebDbContext context) : base(context)
        {
        }

        public IEnumerable<TaskInfo> GetTasks(int maxCount = 30)
        {
            return _entyties
                .Include(x => x.Owner)
                .Include(x => x.Assignees)
                .Take(maxCount)
                .ToList();
        }

        public void DeleteTask(int id)
        {
            var task = _context.TaskInfos.First(x => x.Id == id);
            _context.TaskInfos.Remove(task);
            _context.SaveChanges();

        }

        public TaskInfo GetTaskById(int id)
        {
            return _entyties.Include(x => x.Owner).First(x => x.Id == id);
        }

        public void UpdateTask(AddTaskViewModel taskViewModel)
        {
            var task = _context.TaskInfos.First(x => x.Id == taskViewModel.Id);
            task.Name = taskViewModel.Name;
            task.Description = taskViewModel.Description;
            task.Priority = taskViewModel.Priority;
            _context.SaveChanges();
        }

        public void AddTask(TaskInfo task)
        {
            _context.TaskInfos.Add(task);
            _context.SaveChanges();
        }
    }
}
