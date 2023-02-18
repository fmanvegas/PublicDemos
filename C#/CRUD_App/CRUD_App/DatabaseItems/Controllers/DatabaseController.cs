using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Common;
using CRUD_App.DatabaseItems.Interfaces;
using CRUD_App.Misc;

namespace CRUD_App.DatabaseItems.Controllers
{
    public class DatabaseController : PropChanged
    {
        private readonly IDatabaseOperations _dbService;
        private bool dBFound;
        private string _message;
        private ObservableCollection<SuperHero> _superHeroes;
        private SuperHero _selectedSuperHero;


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
            EditSuperHeroCommand = new Command(PullAllAction);
            PurgeDatabaseCommand = new Command(PullAllAction);

            if (DBFound)
                PullAllAction();
        }

        private string _filerText = string.Empty;

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


        public bool DBFound { get => dBFound; set { dBFound = value; OnPropertyChanged(); } }
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public ObservableCollection<SuperHero> SuperHeroes { get => _superHeroes; set { _superHeroes = value; OnPropertyChanged(); } }


        public SuperHero SelectedSuperHero
        {
            get => _selectedSuperHero;
            set { _selectedSuperHero = value; OnPropertyChanged(); }
        }


        public SuperheroDatabaseCreator.Creator DBCreator { get; set; }

        public ICommand ClearFilterCommand { get; }
        public ICommand CreateDatabaseCommand { get; }
        public ICommand PullAllDatabaseCommand { get; }
        public ICommand DeleteSuperHeroCommand { get; }
        public ICommand EditSuperHeroCommand { get; }
        public ICommand PurgeDatabaseCommand { get; }


        private async Task CreateDatabase()
        {
            DBFound = await DBCreator.Create();

            if (!dBFound)
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

        public async Task<T> GetAsync<T>(int id)
        {
            var result = await _dbService.GetAsync<T>(id);
            return result;
        }


        public async Task DeleteIDAsync(int id)
        {
            var count = await _dbService.DeleteAsync(id);
            Message = $"{count} SuperHeroes Deleted";
            PullAllAction();
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

        public async Task UpdateAsync<T>(int id, T obj) => await _dbService.UpdateAsync(id, obj);
    }
}
