using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_App.DatabaseItems;
using CRUD_App.DatabaseItems.Brokers;
using CRUD_App.DatabaseItems.Controllers;

namespace CRUD_App
{
    public class MainViewModel
    {
        public MainViewModel() { }  

        public MainViewModel(DatabaseService service) 
        {
            DB = new(service);
        }

        public DatabaseController DB { get; }
               
    }
}
