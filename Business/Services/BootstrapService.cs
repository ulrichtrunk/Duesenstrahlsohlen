using Business.Drawers;
using Business.Grids;
using Business.Plotters;
using Data;
using Data.Depots;
using Microsoft.AspNet.Identity;
using NPoco;
using Shared;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Services
{
    public class BootstrapService
    {
        private BootstrapDepot depot;
        private UserDepot userDepot;
        private IDatabase database;
        private IPasswordHasher passwordHasher;

        public BootstrapService(IPasswordHasher passwordHasher) : this(new NPocoDataBaseFactory().GetDatabase(), passwordHasher)
        {

        }

        public BootstrapService(IDatabase database, IPasswordHasher passwordHasher)
        {
            this.database = database;
            this.depot = new BootstrapDepot(database);
            this.userDepot = new UserDepot(database);
            this.passwordHasher = passwordHasher;
        }

        public void Run(string baseDirectory)
        {
            if (database.DatabaseType == DatabaseType.SQLite)
            {
                string databasePath = baseDirectory + "\\db.db";

                if (!File.Exists(databasePath))
                {
                    //File.Delete();
                    string command = File.ReadAllText(baseDirectory + "Data\\Scripts\\SQLite.sql");
                    depot.Run(command);

                    User user = new User();
                    user.Name = "admin@fhnw.ch";
                    user.Password = passwordHasher.HashPassword("Duesenstrahl2017!");
                    user.IsLocal = true;

                    userDepot.Save(user);
                }
            }
        }
    }
}