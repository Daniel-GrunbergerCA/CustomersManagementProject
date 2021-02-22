using CustomerManagementPL.Models;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults; //Contains the already defined types
using LiveCharts.Wpf;
using CustomerManagementPL.Commands;
using System.ComponentModel;

namespace CustomerManagementPL.ViewModels
{
    public class CatalogStatViewModel : INotifyPropertyChanged, IViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get;
            set;
        }

        public ObservableCollection<ItemViewModel> ItemsVM { get; set; }

        private ItemsModel itemsModel;
        private Enums.STAT statistics;

        public CatalogStatViewModel(Enums.STAT categoryStat)
        {
            this.itemsModel = new ItemsModel();
            this.statistics = categoryStat;
            Title = "Long-Term " + ((Enums.STAT)categoryStat).ToString();

            AggregationDay = new List<string>() { "Sunday", "Monday", "Tuestday", "Wendsday", "Thirstday", "Friday", "Saturday" };
            AggregationWeek = new List<string>() { "1st Week", "2st Week", "3st Week", "4st Week" };
            AggregationMonth = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            AggregationYear = new List<string>();
            for (int i = DateTime.Now.Year; i >= 1970; --i)
                AggregationYear.Add(i.ToString());

            foreach(var item in itemsModel.getAllProductsTupleNameKey())
            {
                if(!itemNames.ContainsKey(item.Item1))
                {
                    itemNames.Add(item.Item1,item.Item2);
                }
            }

            selectedMonth = -1;
            selectedWeek = -1;
            monthSelected = false;
            SelectedIndexYear = 0;
        }

        private int selectedYear;
        public int SelectedIndexYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                if (statistics == Enums.STAT.Products)
                    RefreshChart();
                else
                    RefreshStoreChart();
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("YearComboBox"));
            }
        }

        private int selectedMonth;
        public int SelectedIndexMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                if (-1 == value)
                {
                    SelectedIndexWeek = value;
                    MonthIsSelected = false;
                }
                else
                {
                    MonthIsSelected = true;
                }
                if (statistics == Enums.STAT.Products)
                    RefreshChart();
                else
                    RefreshStoreChart();
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexMonth"));
            }
        }

        private int selectedWeek;
        public int SelectedIndexWeek
        {
            get { return selectedWeek; }
            set
            {
                selectedWeek= value;
                if (statistics == Enums.STAT.Products)
                    RefreshChart();
                else
                    RefreshStoreChart();
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexWeek"));
            }
        }

        private bool monthSelected = true;
        public bool MonthIsSelected
        {
            get { return monthSelected; }
            set
            {
                monthSelected = value;
                if (statistics == Enums.STAT.Products)
                    RefreshChart();
                else
                    RefreshStoreChart();
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("MonthIsSelected"));
            }
        }

        public string AxisYTitle { get; set; }

        public void OnCatalogChange(Enums.STAT categoryStat)
        {
            this.statistics = categoryStat;
            Title = "Long-Term " + ((Enums.STAT)categoryStat).ToString();
        }
        


        public List<string> AggregationDay { get; set; }
        public List<string> AggregationWeek { get; set; }
        public List<string> AggregationMonth { get; set; }
        public List<string> AggregationYear { get; set; }

        Dictionary<string, int[]> itemKeys = new Dictionary<string, int[]> { };
        Dictionary<string, string> itemNames = new Dictionary<string, string>{ };

        public void aggregateDay()
        {
            itemKeys.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear)) //selectedWeek + 1, selectedMonth + 1, selectedYear + 1
            {
                if (itemKeys.ContainsKey(item.SerialKey))
                {
                    itemKeys[item.SerialKey][(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    itemKeys.Add(item.SerialKey, new int[AggregationDay.Count()]);
                    itemKeys[item.SerialKey][(item.Date_of_purchase.Day) % AggregationDay.Count()] = item.Quantity;
                }
            }
        }
        public void aggregateWeek()
        {
            itemKeys.Clear();
            foreach (Item item in itemsModel.GetMonthItems(1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (itemKeys.ContainsKey(item.SerialKey))
                {
                    itemKeys[item.SerialKey][(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    itemKeys.Add(item.SerialKey, new int[AggregationWeek.Count()]);
                    itemKeys[item.SerialKey][(item.Date_of_purchase.Day) / AggregationDay.Count()] = item.Quantity;
                }
            }
        }
        public void aggregateMonth()
        {
            itemKeys.Clear();
            foreach (Item item in itemsModel.GetYearItems(DateTime.Now.Year-selectedYear))
            {
                if (itemKeys.ContainsKey(item.SerialKey))
                {
                    itemKeys[item.SerialKey][item.Date_of_purchase.Month -1] += item.Quantity;
                }
                else
                {
                    itemKeys.Add(item.SerialKey, new int[AggregationMonth.Count()]);
                    itemKeys[item.SerialKey][item.Date_of_purchase.Month - 1] = item.Quantity;
                }
            }
        }






        public void RefreshChart()
        {
            if (selectedMonth == -1)
            { // per month
                aggregateMonth();
                Labels = AggregationMonth;
            }
            else if(selectedWeek == -1)
            { // per week
                aggregateWeek();
                Labels = AggregationWeek;
            }
            else
            { // per day
                aggregateDay();
                Labels = AggregationDay;
            }
            SeriesLineCollection = new SeriesCollection();
            SeriesBarCollection = new SeriesCollection();
            // Remake the Two Charts

            foreach (var item in itemKeys)
            {
                SeriesLineCollection.Add(new LineSeries
                {
                    Title = itemNames[item.Key],
                    Values = new ChartValues<int>(itemKeys[item.Key])
                });
                SeriesBarCollection.Add(new ColumnSeries
                {
                    Title = itemNames[item.Key],
                    Values = new ChartValues<int>(itemKeys[item.Key])
                });
            }
            if(statistics == Enums.STAT.Products)
            {
                AxisYTitle = "Quantity";
                YFormatter = value => value.ToString("N0");
            } 
            else
            {
                AxisYTitle = "Cost";
                YFormatter = value => value.ToString("C");
            }

            // Notify The binded labels in the view
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SeriesLineCollection"));
                PropertyChanged(this, new PropertyChangedEventArgs("SeriesBarCollection"));
                PropertyChanged(this, new PropertyChangedEventArgs("YFormatter"));
                PropertyChanged(this, new PropertyChangedEventArgs("Labels"));
                PropertyChanged(this, new PropertyChangedEventArgs("AxisYTitle"));
            }
        }
        
        public SeriesCollection SeriesLineCollection { get; set; }
        public SeriesCollection SeriesBarCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }









        public SeriesCollection SeriessssCollection { get; set; }
        public SeriesCollection SeriesbbbCollection { get; set; }


        Dictionary<string, double[]> storeNames = new Dictionary<string, double[]> { };

        public void aggregateStoreDay()
        {
            storeNames.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear)) 
            {
                if (storeNames.ContainsKey(item.Store_name))
                {
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Price;
                }
                else
                {
                    storeNames.Add(item.Store_name, new double[AggregationDay.Count()]);
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) % AggregationDay.Count()] = item.Price;
                }
            }
        }
        public void aggregateStoreWeek()
        {
            storeNames.Clear();
            foreach (Item item in itemsModel.GetMonthItems(1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (storeNames.ContainsKey(item.Store_name))
                {
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Price;
                }
                else
                {
                    storeNames.Add(item.Store_name, new double[AggregationWeek.Count()]);
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) / AggregationDay.Count()] = item.Price;
                }
            }
        }
        public void aggregateStoreMonth()
        {
            storeNames.Clear();
            foreach (Item item in itemsModel.GetYearItems(DateTime.Now.Year - selectedYear))
            {
                if (storeNames.ContainsKey(item.Store_name))
                {
                    storeNames[item.Store_name][item.Date_of_purchase.Month - 1] += item.Price;
                }
                else
                {
                    storeNames.Add(item.Store_name, new double[AggregationMonth.Count()]);
                    storeNames[item.Store_name][item.Date_of_purchase.Month - 1] = item.Price;
                }
            }
        }



        public void RefreshStoreChart()
        {
            if (selectedMonth == -1)
            { // per month
                aggregateStoreMonth();
                Labels = AggregationMonth;
            }
            else if (selectedWeek == -1)
            { // per week
                aggregateStoreWeek();
                Labels = AggregationWeek;
            }
            else
            { // per day
                aggregateStoreDay();
                Labels = AggregationDay;
            }
            SeriesLineCollection = new SeriesCollection();
            SeriesBarCollection = new SeriesCollection();
            // Remake the Two Charts

            foreach (var store in storeNames)
            {
                SeriesLineCollection.Add(new LineSeries
                {
                    Title = store.Key, Values = new ChartValues<double>(storeNames[store.Key])
                });
                SeriesBarCollection.Add(new ColumnSeries
                {
                    Title = store.Key, Values = new ChartValues<double>(storeNames[store.Key].ToList())
                });
            }
            AxisYTitle = "Cost";
            YFormatter = value => value.ToString("C");

            // Notify The binded labels in the view
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SeriesLineCollection"));
                PropertyChanged(this, new PropertyChangedEventArgs("SeriesBarCollection"));
                PropertyChanged(this, new PropertyChangedEventArgs("YFormatter"));
                PropertyChanged(this, new PropertyChangedEventArgs("Labels"));
                PropertyChanged(this, new PropertyChangedEventArgs("AxisYTitle"));
            }
        }









        Dictionary<Enums.TYPE, double[]> categories = new Dictionary<Enums.TYPE, double[]> { };
        public void aggregateCategoryDay()
        {
            categories.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (categories.ContainsKey(item.Categorie))
                {
                    categories[item.Categorie][(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Price;
                }
                else
                {
                    categories.Add(item.Categorie, new double[AggregationDay.Count()]);
                    categories[item.Categorie][(item.Date_of_purchase.Day) % AggregationDay.Count()] = item.Price;
                }
            }
        }
        public void aggregateCategoryWeek()
        {
            categories.Clear();
            foreach (Item item in itemsModel.GetMonthItems(1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (categories.ContainsKey(item.Categorie))
                {
                    categories[item.Categorie][(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Price;
                }
                else
                {
                    categories.Add(item.Categorie, new double[AggregationWeek.Count()]);
                    categories[item.Categorie][(item.Date_of_purchase.Day) / AggregationDay.Count()] = item.Price;
                }
            }
        }
        public void aggregateCategoryMonth()
        {
            categories.Clear();
            foreach (Item item in itemsModel.GetYearItems(DateTime.Now.Year - selectedYear))
            {
                if (categories.ContainsKey(item.Categorie))
                {   
                    categories[item.Categorie][item.Date_of_purchase.Month - 1] += item.Price;
                }   
                else
                {   
                    categories.Add(item.Categorie, new double[AggregationMonth.Count()]);
                    categories[item.Categorie][item.Date_of_purchase.Month - 1] = item.Price;
                }
            }
        }



    }

    
}


