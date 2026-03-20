using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly AppDbContext _context;

        public ToDoListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoList>> GetAllListsAsync()
        {
            return await _context.ToDoLists.ToListAsync();
        }

        public async Task<IEnumerable<ToDoList>> GetListsByUserIdAsync(int userId)
        {
            return await _context.ToDoLists.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<ToDoList> GetListByIdAsync(int id)
        {
            //var list = await _context.ToDoLists.FindAsync(id);
            var list = await _context.ToDoLists.Include(x => x.Items).FirstOrDefaultAsync(x => x.ToDoListId == id);
            if (list == null)
            {
                throw new InvalidOperationException($"ToDoList with id {id} was not found.");
            }
            return list;
        }

        public async Task AddListAsync(ToDoList list)
        {
            _context.ToDoLists.Add(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListAsync(ToDoList list)
        {
            _context.ToDoLists.Update(list);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteListAsync(int id)
        {
            var list = await _context.ToDoLists.FindAsync(id);
            if (list != null)
            {
                _context.ToDoLists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
