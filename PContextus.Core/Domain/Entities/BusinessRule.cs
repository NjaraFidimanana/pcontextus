using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class BusinessRule : EntityBase<string>
    {

        public int ContextId { get; set; }


        public string BuninessType { get; set; }


        public int NumberOfContents { get; set; }


        public string Designation { get; set; }


        public string Default { get; set; }

    
    }
}
