using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services
{
    /// <summary>
    /// All the business logic should go via service hence we need to call repository from servcie and service
    /// should be in Application layer.
    /// </summary>

    public class ToDoItemService : IToDoItemService
    {
        private IToDoItemRepository _toDoItemRepository;
        public ToDoItemService(IToDoItemRepository toDoItemRepository)
        {
                this._toDoItemRepository = toDoItemRepository;
        }
        public async Task<IEnumerable<ToDoItem>> GetAllItemsAsync()
        {
            return await _toDoItemRepository.GetAllItemsAsync();
        }

        public async Task<ToDoItem> GetItemByIdAsync(int id)
        {
            return await _toDoItemRepository.GetItemByIdAsync(id); ;
        }

        public async Task AddItemAsync(ToDoItem item)
        {
            await _toDoItemRepository.AddItemAsync(item);
        }

        public async Task UpdateItemAsync(ToDoItem item)
        {
            await _toDoItemRepository.UpdateItemAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _toDoItemRepository.DeleteItemAsync(id);
        }
    }
}
