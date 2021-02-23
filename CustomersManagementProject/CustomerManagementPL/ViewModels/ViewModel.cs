using CustomerManagementPL.Models;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagementPL.ViewModels
{

    public enum WindowDockPosition
    {
        /// <summary>
        /// Not docked
        /// </summary>
        Undocked,
        /// <summary>
        /// Docked to the left of the screen
        /// </summary>
        Left,
        /// <summary>
        /// Docked to the right of the screen
        /// </summary>
        Right,
    }
    public class ViewModel : IViewModel
    {

        public ViewModel(Window window)
        {
            itemsModel = new ItemsModel();
        }

        public void Init()
        {
            itemsModel.Init();
        }




        private ItemsModel itemsModel;




    }
}
