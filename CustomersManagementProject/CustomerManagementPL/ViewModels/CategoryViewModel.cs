using System;
using System.Collections.Generic;


namespace CustomerManagementPL.ViewModels
{
    public class CategoryViewModel
    {
        public string Title { get; set; }
        public List<string> Categories { get; set; }
        public CategoryViewModel(string title, List<string> categories)
        {
            Title = title;
            Categories = categories;
        }

    }
}
