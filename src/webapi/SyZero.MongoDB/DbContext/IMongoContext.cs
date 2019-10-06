using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SyZero.MongoDB
{
   public  interface IMongoContext
   {
       IMongoCollection<T> Set<T>();
   }

   
}
