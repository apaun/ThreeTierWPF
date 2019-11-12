using ViewmodelBase;

namespace Assets.Viewmodel
{
    public class AssetsViewModel : ViewModelBase
    {
        public SavingsAccountViewmodel SavingsAccountVm { get; set; }

        public AssetsViewModel()
        {
            SavingsAccountVm = new SavingsAccountViewmodel();
        }
    }
}
