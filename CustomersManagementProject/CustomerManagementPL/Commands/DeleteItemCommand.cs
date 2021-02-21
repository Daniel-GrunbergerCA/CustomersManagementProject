using CustomerManagementPL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomersManagementDP;

namespace CustomerManagementPL.Commands
{
    public class DeleteItemCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event Action<ItemViewModel> DeleteItemViewModelEvent;
        public event Action<Item> DeleteItemEvent;

        public DeleteItemCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (DeleteItemViewModelEvent != null)
                DeleteItemViewModelEvent(parameter as ItemViewModel);
            else
                //if (DeleteItemEvent != null)
                DeleteItemEvent(parameter as Item);
        }
    }
}
