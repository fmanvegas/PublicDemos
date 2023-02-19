using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Common;
using CRUD_App.DatabaseItems.Interfaces;
using CRUD_App.Extensions;
using CRUD_App.Misc;
using SuperheroDatabaseCreator;

namespace CRUD_App.DatabaseItems.Controllers
{
    public enum AppState { Normal, Editing, Adding }

    public class DatabaseController : PropChanged
    {
        private readonly IDatabaseOperations _dbService;
        private bool _dBFound;
        private string _message;
        private ObservableCollection<SuperHero> _superHeroes;
        private SuperHero _selectedSuperHero;
        private SuperHero _tempSuperHero;
        private string _filerText = string.Empty;
        private AppState _state = AppState.Normal;

        public DatabaseController(IDatabaseOperations dbService)
        {
            _dbService = dbService;
            DBCreator = new();
            _superHeroes = new();
            _message = string.Empty;

            DBFound = _dbService.Exists();

            ClearFilterCommand = new Command(() => FilterText = string.Empty);
            CreateDatabaseCommand = new Command(CreateDatabase);
            PullAllDatabaseCommand = new Command(PullAllAction);
            DeleteSuperHeroCommand = new DeleteSuperHeroCommand(DeleteSuperHeroAsync);

            CancelEditCommand = new Command(() => State = AppState.Normal);

            EditSuperHeroCommand = new DeleteSuperHeroCommand(EditSuperHeroAsync);            
            AddSuperHeroCommand = new Command(AddSuperHeroAsync);

            PurgeDatabaseCommand = new Command(PullAllAction);

            if (DBFound)
                PullAllAction();
        }

      

        public string FilterText
        {
            get { return _filerText; }
            set { _filerText = value; OnPropertyChanged(); FilterSuperHeroes(value); }
        }
        private void FilterSuperHeroes(string value)
        {
            var col = CollectionViewSource.GetDefaultView(_superHeroes);
            if (col is not CollectionView view)
                return;

            view.Filter += (x) =>
            {
                if (string.IsNullOrEmpty(value) || x is not SuperHero sh || string.IsNullOrEmpty(sh.Name))
                    return true;

                return sh.Name.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                       (!string.IsNullOrEmpty(sh.Identity) && sh.Identity.Contains(value, StringComparison.OrdinalIgnoreCase));
            };
        }

        public AppState State { get => _state; set { _state = value; OnPropertyChanged(); } }
        public bool DBFound { get => _dBFound; set { _dBFound = value; OnPropertyChanged(); } }
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public ObservableCollection<SuperHero> SuperHeroes { get => _superHeroes; set { _superHeroes = value; OnPropertyChanged(); } }

        public SuperHero SelectedSuperHero { get => _selectedSuperHero; set { _selectedSuperHero = value; OnPropertyChanged(); } }
        public SuperHero TempSuperHero { get => _tempSuperHero; set { _tempSuperHero = value; OnPropertyChanged(); } }
        public SuperheroDatabaseCreator.Creator DBCreator { get; set; }





        public ICommand ClearFilterCommand { get; }
        public ICommand CreateDatabaseCommand { get; }
        public ICommand PullAllDatabaseCommand { get; }
        public ICommand DeleteSuperHeroCommand { get; }
        public ICommand EditSuperHeroCommand { get; }
        public ICommand PurgeDatabaseCommand { get; }
        public ICommand AddSuperHeroCommand { get; }
        public ICommand CancelEditCommand { get; }


        private async Task CreateDatabase()
        {
            DBFound = await DBCreator.Create();

            if (!_dBFound)
                return;

            if (DBFound)
                PullAllAction();
        }
        private async void PullAllAction()
        {
            var result = await _dbService.GetAllAsync<SuperHero>();
            if (result.Any())
                SuperHeroes = new(result);
        }

        public async Task<T>? GetAsync<T>(int id)
        {
            var result = await _dbService.GetAsync<T>(id);
            return result;
        }
               
        public async void DeleteSuperHeroAsync(SuperHero hero)
        {
            var count = await _dbService.DeleteAsync(hero);
            if (count > 0)
            {
                SuperHeroes.Remove(hero);
                Message = $"{hero.Name} Deleted";
            }
        }
        public async void EditSuperHeroAsync(SuperHero hero)        
        {            
            if (_state != AppState.Editing)
            {
                State = AppState.Editing;
                TempSuperHero = hero.Clone();
                return;
            }

            await _dbService.UpdateAsync(TempSuperHero);
            State = AppState.Normal;

            Message = $"{TempSuperHero} Updated";

            PullAllAction();
            SelectedSuperHero = TempSuperHero;
        }
        public async void AddSuperHeroAsync()
        {
            if (_state != AppState.Adding)
            {
                State = AppState.Adding;
                TempSuperHero = new();
                return;
            }

            await _dbService.Insert(TempSuperHero);
            
            SuperHeroes.Add(TempSuperHero);
            SelectedSuperHero = TempSuperHero;

            Message = $"{TempSuperHero} Added";

            State = AppState.Normal;
        }
    }
}
