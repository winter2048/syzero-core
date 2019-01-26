using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SyZero.Infrastructure.Mongo
{
   public  interface IMongoContext
   {
       IMongoCollection<T> Set<T>();
   }

   
}
