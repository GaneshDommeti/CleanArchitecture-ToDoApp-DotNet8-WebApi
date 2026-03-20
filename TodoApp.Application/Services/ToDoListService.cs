using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;   
        public ToDoListService(IToDoListRepository toDoListRepository)
        {
           this._toDoListRepository = toDoListRepository;
        }
        public async Task<IEnumerable<ToDoList>> GetAllListsAsync()
        {
            return await _toDoListRepository.GetAllListsAsync();
        }

        public async Task<IEnumerable<ToDoList>> GetListsByUserIdAsync(int userId)
        {
            return await _toDoListRepository.GetListsByUserIdAsync(userId);
        }

        public async Task<ToDoList> GetListByIdAsync(int id)
        {
            return await _toDoListRepository.GetListByIdAsync(id);
        }


        public async Task AddListAsync(ToDoList list)
        {
            await _toDoListRepository.AddListAsync(list);
        }

        public async Task UpdateListAsync(ToDoList list)
        {
            await _toDoListRepository.UpdateListAsync(list);
        }

        public async Task DeleteListAsync(int id)
        {
            await _toDoListRepository.DeleteListAsync(id);
        }
    }
}
