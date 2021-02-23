using CustomerManagementPL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomersManagementDP;
using System.Windows.Input;

namespace CustomerManagementPL.Commands
{
    public class CreatePDFCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event Action GeneratePdfEvent;

        public CreatePDFCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //if (GeneratePdfEvent != null)
                GeneratePdfEvent();
        }
    }
}
