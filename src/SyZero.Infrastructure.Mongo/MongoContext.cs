using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SyZero.Infrastructure.Mongo
{
   public  class MongoContext:IMongoContext
    {
       public  IMongoDatabase _db;
      
        public MongoContext(IOptions<MongoOptions> options)

        {
            var permissionSystem =
                MongoCredential.CreateCredential(options.Value.DataBase, options.Value.UserName,
                    options.Value.Password);
            var services = new List<MongoServerAddress>();
            foreach (var item in options.Value.Services)
            {
                services.Add(new MongoServerAddress(item.Host, item.Port));
            }
            var settings = new MongoClientSettings
            {
                Credentials = new[] { permissionSystem },
                Servers = services
            };


            var _mongoClient = new MongoClient(settings);
            _db = _mongoClient.GetDatabase(options.Value.DataBase);
        }

        public IMongoCollection<T> Set<T>()
        {
           return  _db.GetCollection<T>(typeof(T).Name);
        }





        //   public IMongoCollection<T> Entities => _db.GetCollection<T>(typeof(T).ToString());

        //  public IMongoCollection<PermissionSystemLogs> PermissionSystemLogs => _db.GetCollection<PermissionSystemLogs>("PermissionSystemLogs");


    }
}
