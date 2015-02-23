using Ilc.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.InformaitonObjects
{
    [DataContract, Export(typeof(InformationObject))]
    public class ProductPhoto : InformationObject
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LargePhotoFileName { get; set; }

        [DataMember]
        public string LargePhotoB64 { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }

        public override string GetArticle()
        {
            return LargePhotoFileName;
        }

        public override string GetHeader()
        {
            return Id.ToString();
        }

        public override string GetId()
        {
            return Id.ToString();
        }

        public override double GetScore()
        {
            return 0;
        }

        public override DateTime? GetTimelineDate()
        {
            return ModifiedDate;
        }
    }
}
