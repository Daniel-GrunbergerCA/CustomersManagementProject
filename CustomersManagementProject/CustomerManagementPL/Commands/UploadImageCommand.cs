using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomerManagementPL.ViewModels;
using CustomersManagementDP;


namespace CustomerManagementPL.Commands
{
    public class UploadImageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event Action<Item> UploadEvent;

        public UploadImageCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (UploadEvent != null)
                UploadEvent(parameter as Item);
        }
    }
}
