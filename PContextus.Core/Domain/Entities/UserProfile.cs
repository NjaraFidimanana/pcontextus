using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Domain.Entities
{
    public class UserProfile :  EntityBase<string>
    {
        public string Urn;

        public string UserName;

        public List<Segmentation> Segments;

        public string TriedBrand;

        public string FavoriteSection;


    }
}
