using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class Segmentation :  EntityBase<string>
    {
        public string SegmentedCode;

        public DateTime ExpiryDate;

        public int Expiration;

        public string SegmentedValue;


        public  void UpdateExpiryDay() {
             var date = DateTime.Now;
             ExpiryDate = date.AddDays(Expiration);
        }
    }
}
