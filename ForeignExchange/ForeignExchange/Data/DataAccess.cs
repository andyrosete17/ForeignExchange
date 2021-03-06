﻿
namespace ForeignExchange.Data
{
    using Interfaces;
    using Models;
    using SQLite.Net;
    using SQLiteNetExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;

    public class DataAccess : IDisposable // To forbid open connection
    {
        SQLiteConnection connection;
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection
                                (
                                    config.Platform,
                                    System.IO.Path.Combine(config.DirectoryDB, "ForeignExchange.db3")
                                );
            connection.CreateTable<Rate>();
        }

        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }
        public void Update<T>(T model)
        {
            connection.Update(model);
        }
        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }
        public T First<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault();
            }
            else
            {
                return connection.Table<T>().FirstOrDefault();
            }
        }
        public List<T> GetList<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().ToList();
            }
            else
            {
                return connection.Table<T>().ToList();
            }
        }
        public T Find<T>(int pk, bool WithChildren) where T: class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
            else
            {
                return connection.Table<T>().FirstOrDefault(mbox => mbox.GetHashCode() == pk);
            }
        }
        /// <summary>
        /// This method close open connection
        /// </summary>
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
