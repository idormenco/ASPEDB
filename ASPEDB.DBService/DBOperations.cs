using ASPEDB.DTO;
using ASPEDB.DTO.Query;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPEDB.DBService
{
    public class DBOperations : IDBOperations
    {
        public string Hello()
        {
            try
            {
                MongoClient mc = new MongoClient();
                var db = mc.GetDatabase("ASPEDB");
                EncryptedPoint ep = new EncryptedPoint(65);
                var collection = db.GetCollection<EncryptedPoint>("Points");
                for (int i = 0; i < 100; i++)
                {
                    var ret = collection.InsertOneAsync(ep);
                    ret.Wait();
                }
            }
            catch (Exception ex)
            { return ex.InnerException.ToString(); }

            return "SS";
        }


        public IList<DTO.EncryptedPoint> Search(EncryptedQuery query)
        {
            throw new NotImplementedException();
        }

        public bool Update(EncryptedQuery query, EncryptedPoint newPoint)
        {
            throw new NotImplementedException();
        }

        public bool Delete(EncryptedQuery query)
        {
            throw new NotImplementedException();
        }

        public bool Insert(EncryptedPoint point)
        {
            throw new NotImplementedException();
        }
    }
}
