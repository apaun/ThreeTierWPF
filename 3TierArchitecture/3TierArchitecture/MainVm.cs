using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using _3TierArchitecture.Annotations;

namespace _3TierArchitecture
{
	class MainVm : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged Implementation
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region Private Fields
		private int _employeeId;
		private string _employeeFirstName;
		private string _employeeLastName;
		private ObservableCollection<Employee> _employeeList;
		private Employee _employeeSelectedItem;
		#endregion

		#region Public Properties
		public int EmployeeId
		{
			get { return _employeeId; }
			set
			{
				_employeeId = value;
				OnPropertyChanged();
			}
		}

		public string EmployeeFirstName
		{
			get { return _employeeFirstName; }
			set { _employeeFirstName = value; OnPropertyChanged(); }
		}

		public string EmployeeLastName
		{
			get { return _employeeLastName; }
			set { _employeeLastName = value; OnPropertyChanged(); }
		}

		public ObservableCollection<Employee> EmployeeList
		{
			get { return _employeeList; }
			set { _employeeList = value; }
		}

		public Employee EmployeeSelectedItem
		{
			get { return _employeeSelectedItem; }
			set
			{
				_employeeSelectedItem = value;
				OnPropertyChanged();
				EmployeeId = value.EmployeeId;
				EmployeeFirstName = value.EmployeeFirstName;
				EmployeeLastName = value.EmployeeLastName;
			}
		}
		#endregion

		#region Command

		public ICommand AddCommand { get; private set; }

		public ICommand RetreiveCommand { get; private set; }

		public ICommand UpdateCommand { get; private set; }

		public ICommand DeleteCommand { get; private set; }

		#endregion

		#region Constructor
		public MainVm()
		{
			AddCommand = new RelayCommand(DoAdd);
			RetreiveCommand = new RelayCommand(DoRetreive);
			UpdateCommand = new RelayCommand(DoUpdate);
			DeleteCommand = new RelayCommand(DoDelete);

			EmployeeList = new ObservableCollection<Employee>();

			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
		}
		#endregion

		#region Command Method
		private void DoDelete()
		{
			if (EmployeeSelectedItem == null)
			{
				MessageBox.Show("Please select a item to delete");
				return;
			}

			Dal.DeleteEmployee(EmployeeSelectedItem);
			DoRetreive();
		}

		private void DoRetreive()
		{
			EmployeeList.Clear();
			foreach (var employee in Dal.GetEmployees())
			{
				EmployeeList.Add(employee);
			}
		}

		private void DoUpdate()
		{
			var emp = new Employee(EmployeeId, EmployeeFirstName, EmployeeLastName);
			Dal.Update(emp);
			DoRetreive();
		}

		private void DoAdd()
		{
			var emp = new Employee(EmployeeId, EmployeeFirstName, EmployeeLastName);
			Dal.AddEmployee(emp);
			DoRetreive();
		}
		#endregion

		#region Private Method
		private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;
			MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		#endregion

	}
}
