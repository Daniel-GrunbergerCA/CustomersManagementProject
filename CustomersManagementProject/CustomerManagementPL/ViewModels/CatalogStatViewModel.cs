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
            SelectedIndexYear = 8;

        }

        private int selectedYear;
        public int SelectedIndexYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                RefreshChart();
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
                RefreshChart();
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
                RefreshChart();
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
                RefreshChart();
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

        public void aggregateProductDay()
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
        public void aggregateProductWeek()
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
        public void aggregateProductMonth()
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
            switch(statistics)
            {
                case Enums.STAT.Products:
                    RefreshProductChart();
                    break;
                case Enums.STAT.Stores:
                    RefreshStoreChart();
                    break;
                case Enums.STAT.Categories:
                    RefreshCategoryChart();
                    break;
                case Enums.STAT.Costs:
                    RefreshCostChart();
                    break;
            }
        }


        public void RefreshProductChart()
        {
            if (selectedMonth == -1)
            { // per month
                aggregateProductMonth();
                Labels = AggregationMonth;
            }
            else if(selectedWeek == -1)
            { // per week
                aggregateProductWeek();
                Labels = AggregationWeek;
            }
            else
            { // per day
                aggregateProductDay();
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
            AxisYTitle = "Quantity";
            YFormatter = value => value.ToString("N0");
            int maxValue = 0;
            foreach(var item in itemKeys)
            {
                int num = unsigned_max(itemKeys[item.Key]);
                if (maxValue < num)
                    maxValue = num;
            }

            setStepSeperator(maxValue, itemKeys.Count);

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
        
        int unsigned_max(int[] arr)
        {
            int maxNum = 0;
            foreach(var num in arr)
            {
                if (maxNum < num)
                    maxNum = num;
            }
            return maxNum;
        }

        double unsigned_max(double[] arr)
        {
            double maxNum = 0;
            foreach (var num in arr)
            {
                if (maxNum < num)
                    maxNum = num;
            }
            return maxNum;
        }

        public SeriesCollection SeriesLineCollection { get; set; }
        public SeriesCollection SeriesBarCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public int StepSeperator { get; set; }


        private const int DEFAULT_SEPERATE_VALUE = 10;





        Dictionary<string, int[]> storeNames = new Dictionary<string, int[]> { };

        public void aggregateStoreDay()
        {
            storeNames.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear)) 
            {
                if (storeNames.ContainsKey(item.Store_name))
                {
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    storeNames.Add(item.Store_name, new int[AggregationDay.Count()]);
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) % AggregationDay.Count()] = item.Quantity;
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
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    storeNames.Add(item.Store_name, new int[AggregationWeek.Count()]);
                    storeNames[item.Store_name][(item.Date_of_purchase.Day) / AggregationDay.Count()] = item.Quantity;
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
                    storeNames[item.Store_name][item.Date_of_purchase.Month - 1] += item.Quantity;
                }
                else
                {
                    storeNames.Add(item.Store_name, new int[AggregationMonth.Count()]);
                    storeNames[item.Store_name][item.Date_of_purchase.Month - 1] = item.Quantity;
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
                    Title = store.Key, Values = new ChartValues<int>(storeNames[store.Key])
                });
                SeriesBarCollection.Add(new ColumnSeries
                {
                    Title = store.Key, Values = new ChartValues<int>(storeNames[store.Key].ToList())
                });
            }
            AxisYTitle = "Quantity";
            YFormatter = value => value.ToString("N0");
            int maxValue = 0;
            foreach (var store in storeNames)
            {
                int num = unsigned_max(storeNames[store.Key]);
                if (maxValue < num)
                    maxValue = num;
            }

            setStepSeperator(maxValue, storeNames.Count);

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









        Dictionary<Enums.TYPE, int[]> categories = new Dictionary<Enums.TYPE, int[]> { };
        public void aggregateCategoryDay()
        {
            categories.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (categories.ContainsKey(item.Categorie))
                {
                    categories[item.Categorie][(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    categories.Add(item.Categorie, new int[AggregationDay.Count()]);
                    categories[item.Categorie][(item.Date_of_purchase.Day) % AggregationDay.Count()] = item.Quantity;
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
                    categories[item.Categorie][(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Quantity;
                }
                else
                {
                    categories.Add(item.Categorie, new int[AggregationWeek.Count()]);
                    categories[item.Categorie][(item.Date_of_purchase.Day) / AggregationDay.Count()] = item.Quantity;
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
                    categories[item.Categorie][item.Date_of_purchase.Month - 1] += item.Quantity;
                }   
                else
                {   
                    categories.Add(item.Categorie, new int[AggregationMonth.Count()]);
                    categories[item.Categorie][item.Date_of_purchase.Month - 1] = item.Quantity;
                }
            }
        }





        public void RefreshCategoryChart()
        {
            if (selectedMonth == -1)
            { // per month
                aggregateCategoryMonth();
                Labels = AggregationMonth;
            }
            else if (selectedWeek == -1)
            { // per week
                aggregateCategoryWeek();
                Labels = AggregationWeek;
            }
            else
            { // per day
                aggregateCategoryDay();
                Labels = AggregationDay;
            }
            SeriesLineCollection = new SeriesCollection();
            SeriesBarCollection = new SeriesCollection();
            // Remake the Two Charts

            foreach (var category in categories)
            {
                SeriesLineCollection.Add(new LineSeries
                {
                    Title = category.Key.ToString(),
                    Values = new ChartValues<int>(categories[category.Key])
                });
                SeriesBarCollection.Add(new ColumnSeries
                {
                    Title = category.Key.ToString(),
                    Values = new ChartValues<int>(categories[category.Key])
                });
            }
            AxisYTitle = "Quantity";
            YFormatter = value => value.ToString("N0");
            int maxValue = 0;
            foreach (var category in categories)
            {
                int num = unsigned_max(categories[category.Key]);
                if (maxValue < num)
                    maxValue = num;
            }

            setStepSeperator(maxValue, categories.Count);
            
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



        Dictionary<int, double> costs = new Dictionary<int, double>();
        public void aggregateCostDay()
        {
            costs.Clear();
            foreach (Item item in itemsModel.GetWeekItems(1 + selectedWeek, 1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if (!costs.ContainsKey((item.Date_of_purchase.Day) % AggregationDay.Count()))
                {
                    costs.Add((item.Date_of_purchase.Day) % AggregationDay.Count(), 0);
                }
                costs[(item.Date_of_purchase.Day) % AggregationDay.Count()] += item.Price;
            }
        }
        public void aggregateCostWeek()
        {
            costs.Clear();
            foreach (Item item in itemsModel.GetMonthItems(1 + selectedMonth, DateTime.Now.Year - selectedYear))
            {
                if(!costs.ContainsKey((item.Date_of_purchase.Day) / AggregationDay.Count()))
                {
                    costs.Add((item.Date_of_purchase.Day) / AggregationDay.Count(), 0);
                }
                costs[(item.Date_of_purchase.Day) / AggregationDay.Count()] += item.Price;
            }
        }
        public void aggregateCostMonth()
        {
            costs.Clear();
            foreach (Item item in itemsModel.GetYearItems(DateTime.Now.Year - selectedYear))
            {
                if (!costs.ContainsKey(item.Date_of_purchase.Month - 1))
                {
                    costs.Add(item.Date_of_purchase.Month - 1, 0);
                }
                costs[item.Date_of_purchase.Month - 1] += item.Price;
            }
        }

        public void RefreshCostChart()
        {
            if (selectedMonth == -1)
            { // per month
                aggregateCostMonth();
                Labels = AggregationMonth;
            }
            else if (selectedWeek == -1)
            { // per week
                aggregateCostWeek();
                Labels = AggregationWeek;
            }
            else
            { // per day
                aggregateCostDay();
                Labels = AggregationDay;
            }
            SeriesLineCollection = new SeriesCollection();
            SeriesBarCollection = new SeriesCollection();
            // Remake the Two Charts
            List<int> costsInt = new List<int>();
            foreach(var cost in costs)
            {
                costsInt.Add((int)cost.Value);
            }

            SeriesLineCollection.Add(new LineSeries
            {
                Title = "Your Costs",
                Values = new ChartValues<int>(costsInt)
            }) ;
            SeriesBarCollection.Add(new ColumnSeries
            {
                Title = "Your Costs",
                Values = new ChartValues<int>(costsInt)
            });
            AxisYTitle = "Cost";
            YFormatter = value => value.ToString("C");
            double maxValue = 0;
            for(int i = 0; i< costs.Count; ++i)
            {
                double num = costsInt[i];
                if (maxValue < num)
                    maxValue = num;
            }

            setStepSeperator((int)maxValue, costs.Count);

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


        void setStepSeperator(int maxValue, int Count)
        {
            Count = Count == 0 ? 1 : Count;
            StepSeperator = maxValue / Count;
            StepSeperator = StepSeperator == 0 ? DEFAULT_SEPERATE_VALUE : StepSeperator;
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("StepSeperator"));
            }
        }
    }

    
}


