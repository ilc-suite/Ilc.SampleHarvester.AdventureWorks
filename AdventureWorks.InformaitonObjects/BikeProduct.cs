using Ilc.BusinessObjects;
using Ilc.BusinessObjects.Common;
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
    public class BikeProduct : Product
    {
        public string Articlenumber { get; set; }

        public bool MakeFlag { get; set; }

        public bool FinishedGoodsFlag { get; set; }

        public string Color { get; set; }

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        public string Size { get; set; } 
        
        public string SizeMeasure { get; set; }

        public string WeightMeasure { get; set; }

        public decimal? Weight { get; set; }

        public int DaysToManufacture { get; set; }

        public string ProductLine { get; set; }

        public string Class { get; set; }

        public string Style { get; set; }

        public int SubCategoryId { get; set; }

        public int ProductModelId { get; set; }

        public DateTime? SellEndDate { get; set; }

        public int CustomerOrderQty { get; set; }
    }
}
