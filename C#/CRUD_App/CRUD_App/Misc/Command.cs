using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Dapper.SqlMapper;

namespace CRUD_App.Misc
{



    internal class Command : ICommand
    {
        public Command(Action action)
        {
            dynFuncNoParam = action;
        }
        public Command(Func<Task<bool>> func)
        {
            dynFuncNoParam = func; 
        }
        public Command(Func<Task> func)
        {
            dynFuncNoParam = func;
        }

        private readonly dynamic dynFuncNoParam;       

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            dynFuncNoParam?.Invoke();
        }
    }
}
