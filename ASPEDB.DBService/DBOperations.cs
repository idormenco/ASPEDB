using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using ASPEDB.DTO.Query;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public IList<EncryptedDBPoint> Search(EncryptedQuery query)
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

        public DBOperationResponse Insert(EncryptedDBPoint encryptedPoint)
        {
            try
            {
                MongoClient mc = new MongoClient();
                var db = mc.GetDatabase("ASPEDB");
                var collection = db.GetCollection<EncryptedDBPoint>("Points");
                var ret = collection.InsertOneAsync(encryptedPoint);
                ret.Wait();
            }
            catch (Exception ex)
            { return new DBOperationResponse(false, ex.InnerException.ToString()); }
            return new DBOperationResponse(true, "Insert OK");
        }

        //public async Task<int> CountAsync()
        //{

        //    MongoClient mc = new MongoClient();

        //    var db = mc.GetDatabase("ASPEDB");
        //    var collection = db.GetCollection<EncryptedDBPoint>("Points");
        //    var filter = new BsonDocument();
        //    var count = 0;
        //    using (var cursor = await collection.FindAsync(filter))
        //    {
        //        while (await cursor.MoveNextAsync())
        //        {
        //            var batch = cursor.Current;
        //            foreach (var document in batch)
        //            {
        //                // process document
        //                count++;
        //            }
        //        }
        //    }
        //    return count;
        //}

        //public async Task<int> DoSomethingAsync(Encryp)
        //{

        //    MongoClient mc = new MongoClient();

        //    var db = mc.GetDatabase("ASPEDB");
        //    var collection = db.GetCollection<EncryptedDBPoint>("Points");
        //    var filter = new BsonDocument();
        //    var count = 0;
        //    using (var cursor = await collection.FindAsync(filter))
        //    {
        //        while (await cursor.MoveNextAsync())
        //        {
        //            var batch = cursor.Current;
        //            foreach (var document in batch)
        //            {
        //                // process document
        //                count++;
        //            }
        //        }
        //    }
        //    return count;
        //}
    }
}
