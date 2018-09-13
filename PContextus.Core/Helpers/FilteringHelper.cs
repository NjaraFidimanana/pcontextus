using PContextus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PContextus.Core.Helpers
{
    public static  class FilteringHelper
    {

       /* public Func<IEnumerable<ArticleContent>, IOrderedEnumerable<ArticleContent>> MostRecent(IEnumerable<ArticleContent> contents) {

            Func<IEnumerable<ArticleContent>, IOrderedEnumerable<ArticleContent>> filter = contents.OrderBy(x => x.Title);
        }*/


        public static Func<T, object> GetSortExpression<T>(string sortExpressionStr)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var sortExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(param, sortExpressionStr), typeof(object)), param);
            return sortExpression.Compile();
        }


        public static IOrderedEnumerable<T> GetOrderFor<T>(this IEnumerable<T> list , string defaultSort)
        {         
            switch (defaultSort)
            {
                case "MostRecent":
                    return list.OrderByDescending(GetSortExpression<T>("CreatedAt")).ThenBy(GetSortExpression<T>("Title"));

                case "MostViewed":
                    return list.OrderByDescending(GetSortExpression<T>("Views")).ThenBy(GetSortExpression<T>("Title"));

                case "Relevant":
                    return list.OrderByDescending(GetSortExpression<T>("RelevantScoring")).ThenBy(GetSortExpression<T>("Title"));

                case "MostRated":
                    return list.OrderByDescending(GetSortExpression<T>("Rated")).ThenBy(GetSortExpression<T>("Title"));

                case "TopReview":
                    return list.OrderByDescending(GetSortExpression<T>("ReviewCount")).ThenBy(GetSortExpression<T>("Title"));

                default:
                    return list.OrderByDescending(GetSortExpression<T>("Title"));
            }
        }
    }
}
