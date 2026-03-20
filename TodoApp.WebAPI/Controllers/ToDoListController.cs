using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _toDoListService; 
        public ToDoListController(IToDoListService toDoItemService )
        {
            this._toDoListService = toDoItemService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateToDoList([FromBody] ToDoList toDoList)
        {
            await _toDoListService.AddListAsync(toDoList);
            return CreatedAtAction(nameof(GetToDoList), new { id = toDoList.ToDoListId }, toDoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await _toDoListService.GetListByIdAsync(id);
            return toDoList == null ? NotFound() : Ok(toDoList);
        }
    }
}
