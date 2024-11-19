﻿using System;

namespace ToDoListApp.Classes
{
    public class ToDoItem
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public bool IsCompleted { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public DateTime? CompletedAt { get; set; } 
    }
}
