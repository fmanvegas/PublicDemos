using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using static Dapper.SqlMapper;

namespace CRUD_App.Misc
{
    internal class DeleteSuperHeroCommand : ICommand
    {
        Action<SuperHero> deleteAction;

        public DeleteSuperHeroCommand(Action<SuperHero> deleteSuperHeroAsync)
        {
            deleteAction = deleteSuperHeroAsync;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is SuperHero hero)
                deleteAction?.Invoke(hero);
        }
    }



    internal class Command : ICommand
    {
        public Command(Action action)
        {
            dynFuncNoParam = action;
        }              
        public Command(Func<Task> func)
        {
            dynFuncNoParam = func;
        }
       

        private readonly dynamic dynFuncNoParam;
        private readonly dynamic dynFuncParam;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            dynFuncNoParam?.Invoke();
            dynFuncParam?.Invoke(parameter);
        }
    }
}
