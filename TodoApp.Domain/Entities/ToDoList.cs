using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Domain.Entities
{
    public class ToDoList
    {
        public int ToDoListId { get; set; }
        public int UserId { get; set; }         // Foreign Key to User
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ToDoItem> Items { get; set; }// a list can have multiple todoitems
    }
}
