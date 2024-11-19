using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Classes
{
    public class ToDoValidator : AbstractValidator<ToDoItem>
    {
        public ToDoValidator() 
        {
            RuleFor(ToDoItem => ToDoItem.Title).NotNull().NotEmpty().WithMessage("Title cannot be empty.");
            RuleFor(ToDoItem => ToDoItem.Description).NotNull().NotEmpty().WithMessage("Description cannot be empty.");
        }
    }
}
