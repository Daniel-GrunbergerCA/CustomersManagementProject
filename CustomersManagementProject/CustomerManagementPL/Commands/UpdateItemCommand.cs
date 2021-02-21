using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomerManagementPL.Commands
{
    public class UpdateItemCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event Action UpdateEvent;

        public UpdateItemCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //CurrentVM.Students.Add(parameter as Student);
            if (UpdateEvent != null)
                UpdateEvent();
        }
    }
}
