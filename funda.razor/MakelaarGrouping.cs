using System.Linq;
using funda.Services;
using System.Collections.Generic;

namespace funda.razor
{
    public class MakelaarGrouping {
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
        public List<Object1> Objects { get; set; }
    }

    public static class Object1CollectionExtensions
    {
        public static IEnumerable<MakelaarGrouping> GroupByMakelaarId(this IEnumerable<Object1> objects)
        {
            return objects
                .GroupBy(o => o.MakelaarId)
                .Select(o => new MakelaarGrouping
                {
                    MakelaarId = o.Key,
                    MakelaarNaam = o.First().MakelaarNaam,
                    Objects = o.ToList()
                })
                .OrderByDescending(o => o.Objects.Count);
        }
    }
}
