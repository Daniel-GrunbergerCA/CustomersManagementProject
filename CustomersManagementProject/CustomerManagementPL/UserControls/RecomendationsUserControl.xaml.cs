using CustomerManagementPL.ViewModels;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerManagementPL.UserControls
{
    /// <summary>
    /// Interaction logic for RecomendationsUserControl.xaml
    /// </summary>
    public partial class RecomendationsUserControl : UserControl
    {
        public RecomendationViewModel currentVM = new RecomendationViewModel();
        public RecomendationsUserControl()
        {
           
            InitializeComponent();
            DataContext = currentVM;
         //   this.TodayLabel.Content = currentVM.today;
        }

        /*private void CreatePdfButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "PDF files|*.pdf", ValidateNames = true, Multiselect = false };
        
                if (ofd.ShowDialog() ==true)
                {
                    try
                    {
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(ofd.FileName);
                        StringBuilder sb = new StringBuilder();
                        for (int  i=1; i<=reader.NumberOfPages;i++)
                        {
                            sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                        PdfOutput.Text += sb.ToString();
                        reader.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                
            }
        }*/
    }
    }

