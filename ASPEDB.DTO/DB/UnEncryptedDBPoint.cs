using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class UnEncryptedDBPoint
    {
        [DataMember]
        public UnEncryptedDBValue Type { get; set; }
        [DataMember]
        public UnEncryptedDBValue Name { get; set; }
        [DataMember]
        public UnEncryptedDBValue Value { get; set; }

        public UnEncryptedDBPoint(UnEncryptedDBValue type, UnEncryptedDBValue name, UnEncryptedDBValue value)
        {
            this.Type = new UnEncryptedDBValue(type);
            this.Name = new UnEncryptedDBValue(name);
            this.Value = new UnEncryptedDBValue(value);
        }

        public UnEncryptedDBPoint(UnEncryptedDBPoint uedbp)
            : this(uedbp.Type, uedbp.Name, uedbp.Value)
        {
        }
    }
}
