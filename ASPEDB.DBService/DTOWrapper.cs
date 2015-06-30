using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.DTO.DB;
using MongoDB.Bson;

namespace ASPEDB.DBService
{
    class EncryptedMDBPoint
    {
        public EncryptedMDBPoint(EncryptedDBPoint encryptedPoint)
        {
            DBRecord = encryptedPoint;
        }

        public ObjectId _id { get; set; }
        public EncryptedDBPoint DBRecord { get; set; } 
    }
}
