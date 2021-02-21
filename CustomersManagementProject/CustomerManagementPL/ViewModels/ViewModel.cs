using CustomerManagementPL.Models;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;



        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);




        /// <summary>
        /// The smallest size of the window's width
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 667;
        /// <summary>
        /// The smallest size of the window's height
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            // If it is maximized or docked, no border
            get => Borderless ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);



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
