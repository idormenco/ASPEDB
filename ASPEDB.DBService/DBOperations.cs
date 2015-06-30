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
            return "Hello from service";
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
                throw ex;
            }
            return null;
        }

        public DBOperationResponse Update(EncryptedDBQuery query, EncryptedDBValue newValue)
        {
            try
            {
                Task<DBOperationResponse> updateDBAsynkTask = UpdateDBAsynkTask(query, newValue);
                updateDBAsynkTask.Wait();
                return updateDBAsynkTask.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public DBOperationResponse Delete(EncryptedDBQuery query)
        {
            try
            {
                Task<DBOperationResponse> deleteDBAsynkTask = DeleteDBAsynkTask(query);
                deleteDBAsynkTask.Wait();
                return deleteDBAsynkTask.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
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
            {
                return new DBOperationResponse(false, ex.InnerException.ToString());
            }
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
            decimal epsilon = (decimal) Math.Pow(10, -4);
            var db = mc.GetDatabase("ASPEDB");
            var collection = db.GetCollection<EncryptedMDBPoint>("Points");
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    dbPoints.AddRange(
                        batch.Where(document => edbq.DBQueryCoversDBPoint(document.DBRecord, epsilon))
                            .Select(x => x.DBRecord));
                }
            }
            return dbPoints;
        }

        public async Task<DBOperationResponse> UpdateDBAsynkTask(EncryptedDBQuery edbq, EncryptedDBValue newValue)
        {
            try
            {
                MongoClient mc = new MongoClient();
                List<EncryptedMDBPoint> dbPoints = new List<EncryptedMDBPoint>();
                decimal epsilon = (decimal) Math.Pow(10, -4);
                var db = mc.GetDatabase("ASPEDB");
                var collection = db.GetCollection<EncryptedMDBPoint>("Points");
                var filter = new BsonDocument();
                using (var cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        dbPoints.AddRange(batch.Where(document => edbq.DBQueryCoversDBPoint(document.DBRecord, epsilon)));
                    }
                }
                if (dbPoints.Count == 0)
                {
                    return new DBOperationResponse(true, "No points covered by query,no update");
                }
                else
                {
                    foreach (var toUp in dbPoints)
                    {
                        toUp.DBRecord.Value = newValue;
                        await collection.ReplaceOneAsync(x => x._id == toUp._id, toUp);
                    }
                    return new DBOperationResponse(true, string.Format("{0} points updated", dbPoints.Count));
                }
            }
            catch (Exception ex)
            {
                return new DBOperationResponse(false, ex.InnerException.ToString());
            }
        }

        public async Task<DBOperationResponse> DeleteDBAsynkTask(EncryptedDBQuery edbq)
        {
            try
            {
                MongoClient mc = new MongoClient();
                List<EncryptedMDBPoint> dbPoints = new List<EncryptedMDBPoint>();
                decimal epsilon = (decimal)Math.Pow(10, -4);
                var db = mc.GetDatabase("ASPEDB");
                var collection = db.GetCollection<EncryptedMDBPoint>("Points");
                var filter = new BsonDocument();
                using (var cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        dbPoints.AddRange(batch.Where(document => edbq.DBQueryCoversDBPoint(document.DBRecord, epsilon)));
                    }
                }
                if (dbPoints.Count == 0)
                {
                    return new DBOperationResponse(true, "No points covered by query,no delete");
                }
                else
                {
                    foreach (var toUp in dbPoints)
                    {
                        await collection.DeleteOneAsync(x => x._id == toUp._id);
                    }
                    return new DBOperationResponse(true, string.Format("{0} points deleted", dbPoints.Count));
                }
            }
            catch (Exception ex)
            {
                return new DBOperationResponse(false, ex.InnerException.ToString());
            }
        }
    }
}
