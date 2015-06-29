using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using ASPEDB.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

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


        public IList<EncryptedDBPoint> Search(EncryptedDBQuery query)
        {
            try
            {
                Task<IList<EncryptedDBPoint>> queryDBAsynkTask = QueryDBAsynkTask(query);
                queryDBAsynkTask.Wait();
                return queryDBAsynkTask.Result;
            }
            catch (Exception ex)
            {
                int a =0; 
            }
            return null;
        }

        public bool Update(EncryptedDBQuery query, EncryptedDBPoint newPoint)
        {
            throw new NotImplementedException();
        }

        public bool Delete(EncryptedDBQuery query)
        {
            throw new NotImplementedException();
        }

        public DBOperationResponse Insert(EncryptedDBPoint encryptedPoint)
        {
            try
            {
                MongoClient mc = new MongoClient();
                var db = mc.GetDatabase("ASPEDB");
                var collection = db.GetCollection<EncryptedMDBPoint>("Points");
                var ret = collection.InsertOneAsync(new EncryptedMDBPoint(encryptedPoint));
                ret.Wait();
            }
            catch (Exception ex)
            { return new DBOperationResponse(false, ex.InnerException.ToString()); }
            return new DBOperationResponse(true, "Insert OK");
        }

        public async Task<int> CountAllAsync()
        {

            MongoClient mc = new MongoClient();
            var db = mc.GetDatabase("ASPEDB");
            var collection = db.GetCollection<EncryptedMDBPoint>("Points");
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        count++;
                    }
                }
            }
            return count;
        }

        public async Task<IList<EncryptedDBPoint>> QueryDBAsynkTask(EncryptedDBQuery edbq)
        {
            MongoClient mc = new MongoClient();
            List<EncryptedDBPoint> dbPoints = new List<EncryptedDBPoint>();
            decimal epsilon = (decimal)Math.Pow(10, -4);
            var db = mc.GetDatabase("ASPEDB");
            var collection = db.GetCollection<EncryptedMDBPoint>("Points");
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    dbPoints.AddRange(batch.Where(document => edbq.DBQueryCoversDBPoint(document, epsilon)));
                }
            }
            return dbPoints;
        }
    }
}
