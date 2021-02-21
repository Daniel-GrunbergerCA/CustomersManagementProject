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
            SelectedIndexYear = 0;
            SelectedIndexMonth = -1;
            SelectedIndexWeek = -1;

            BasicLineChart();
            BasicBarChart();
        }

        private int selectedYear;
        public int SelectedIndexYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
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
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("MonthIsSelected"));
            }
        }


        public void OnCatalogChange(Enums.STAT categoryStat)
        {
            this.statistics = categoryStat;
            Title = "Long-Term " + ((Enums.STAT)categoryStat).ToString();
            // filter for category stat
        }
        
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }


        public List<string> AggregationDay { get; set; }
        public List<string> AggregationWeek { get; set; }
        public List<string> AggregationMonth { get; set; }
        public List<string> AggregationYear { get; set; }



        public void aggregateWeek()
        {
            Dictionary<string, double[]> itemKeys = new Dictionary<string, double[]> { };

            foreach (Item item in itemsModel.GetWeekItems(Week, Month, Year))
            {
                if (itemKeys.ContainsKey(item.SerialKey))
                {
                    itemKeys[item.SerialKey][item.Date_of_purchase.Day] += item.Quantity;
                }
                else
                {
                    itemKeys.Add(item.SerialKey, new double[AggregationDay.Count()]);
                }
            }

        }


        public void BasicLineChart()
        {



            //foreach(var date in itemsModel.GetDateGroups())
            //{
            //    foreach
            //}


            //List<double> 
            //foreach(var itemGroup in itemsModel.groupByDate())
            //{
            //    foreach(var date in itemGroup)
            //    {
                    
            //    }
            //}




            SeriesLineCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,80 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                }
            };
            LineLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            LineFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesLineCollection.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0 //straight lines, 1 really smooth lines
            });

            //modifying any series values will also animate and update the chart
            SeriesLineCollection[2].Values.Add(5d);
        }


        public void BasicBarChart()
        {

            SeriesBarCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesBarCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesBarCollection[1].Values.Add(48d);

            BarLabels = new[] { "Maria", "Susan", "Charles", "Frida" };
            BarFormatter = value => value.ToString("N");
        }



        public SeriesCollection SeriesLineCollection { get; set; }
        public string[] LineLabels { get; set; }
        public Func<double, string> LineFormatter { get; set; }

        public SeriesCollection SeriesBarCollection { get; set; }
        public string[] BarLabels { get; set; }
        public Func<double, string> BarFormatter { get; set; }

    }

    
}


