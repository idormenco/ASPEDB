using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.DTO.DB;
using MongoDB.Bson;

namespace ASPEDB.DBService
{
    class EncryptedMDBPoint:EncryptedDBPoint
    {
        public ObjectId _id { get; set; }
        public EncryptedMDBPoint(EncryptedDBValue type, EncryptedDBValue name, EncryptedDBValue value) : base(type, name, value)
        {
        }

        public EncryptedMDBPoint(EncryptedDBPoint edbp)
            : base(edbp)
        {
        }
    }
}
