using WorkMyFlight;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfOnFlights;
using WpfOnFlights.Model;

namespace WpfOnFlights
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private WpfInfo wpfinfo = new WpfInfo();
        public DelegateCommand AddToDBCommand { get; set; }
        public DelegateCommand ReplaceDBCommand { get; set; }
        private int customerToAdd;
        private int airlinesToAdd;
        private int flightsToAdd;
        private int ticketsToAdd;
        private int countriesToAdd;
        public int CustomerToAdd
        {
            get
            {
                return customerToAdd;
            }
            set
            {
                customerToAdd = value;
                OnPropertyChanged("CustomerToAdd");
            }
        }
        public int AirlinesToAdd
        {
            get
            {
                return airlinesToAdd;
            }
            set
            {
                airlinesToAdd = value;
                OnPropertyChanged("AirlinesToAdd");
            }
        }
        public int FlightsToAdd
        {
            get
            {
                return flightsToAdd;
            }
            set
            {
                flightsToAdd = value;
                OnPropertyChanged("FlightsToAdd");
            }
        }
        public int TicketsToAdd
        {
            get
            {
                return ticketsToAdd;
            }
            set
            {
                ticketsToAdd = value;
                OnPropertyChanged("TicketsToAdd");
            }
        }
        public int CountriesToAdd
        {
            get
            {
                return countriesToAdd;
            }
            set
            {
                countriesToAdd = value;
                OnPropertyChanged("CountriesToAdd");
            }
        }
        private bool notBusy = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlyingCenterSystem FlyingCenterSystem { get; set; }
        public MainWindowViewModel()
        {
            notBusy = true;
            AddToDBCommand = new DelegateCommand(ExecuteAddToDB, canExecuteData);
            ReplaceDBCommand = new DelegateCommand(ExecuteReplaceDB, canExecuteData);
            Task.Run(() =>
            {
                while (true)
                {
                     // go check the enable/disable
                    ReplaceDBCommand.RaiseCanExecuteChanged();
                    Thread.Sleep(500);
                }

            });
            Task.Run(() =>
            {
                while (true)
                {
                     // go check the enable/disable
                    ReplaceDBCommand.RaiseCanExecuteChanged();
                    Thread.Sleep(500);
                }

            });
        }
        //private void SafeInvoke(Action work)
        //{
        //    if (Dispatcher.CheckAccess())
        //    {
        //        work.Invoke();
        //        return;
        //    }
        //    this.Dispatcher.BeginInvoke(work);

        //}
        private bool canExecuteData()
        {
            return notBusy;
            
        }
        
        private void ExecuteAddToDB()
        {
            wpfinfo.CreateRandomCustomers(customerToAdd);
            wpfinfo.CreateRandomCountries(countriesToAdd);
            wpfinfo.CreateRandomAirlineCompanies(airlinesToAdd);
            MessageBox.Show($"ExecuteAddToDataBase:  cus: {customerToAdd} coun: {countriesToAdd} air:  {airlinesToAdd}");
           
        }

        private void ExecuteReplaceDB()
        {
            wpfinfo.ClearDB();
            wpfinfo.CreateRandomCustomers(customerToAdd);
            wpfinfo.CreateRandomCountries(countriesToAdd);
            wpfinfo.CreateRandomAirlineCompanies(airlinesToAdd);
            MessageBox.Show("ExecuteReplaceDataBase");
        }
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
  
}
