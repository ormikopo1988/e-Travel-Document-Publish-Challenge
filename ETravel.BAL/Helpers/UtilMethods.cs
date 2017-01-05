using System;
using System.Collections.Generic;
using System.Linq;

namespace ETravel.BAL.Helpers
{
    public static class UtilMethods
    {
        public static long GetCurrentUserId(IUnitOfWork uow, string username)
        {
            try
            {
                return uow
                        .UserRepository
                        .SearchFor(e => e.Username == username)
                        .Select(e => e.Id)
                        .SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("User lookup for current logged in User Id failed", ex);
            }
        }

        public static IEnumerable<TSource> 
            DistinctBy<TSource, TKey>
            (IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public enum StatusCodes
        {
            NOT_FOUND = 0,
            NOT_AUTHORIZED = 1,
            OK = 2
        };
    }
}
