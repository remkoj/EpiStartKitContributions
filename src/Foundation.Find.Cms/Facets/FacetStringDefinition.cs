using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Cms.Facets
{
    public class FacetStringDefinition : FacetDefinition
    {
        public FacetStringDefinition() => RenderType = GetType().Name;

        public ITypeSearch<T> Filter<T>(ITypeSearch<T> query, List<string> selectedValues) => selectedValues.Any() ? query.AddStringFilter(selectedValues, FieldName) : query;

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query, Filter filter) => Cms.SearchExtensions.TermsFacetFor(query, FieldName, typeof(string), filter);

        public override void PopulateFacet(FacetGroupOption facetGroupOption, Facet facet, string selectedFacets)
        {
            if (!(facet is TermsFacet termsFacet))
            {
                return;
            }

            facetGroupOption.Facets = termsFacet.Terms.Select(x => new FacetOption
            {
                Count = x.Count,
                Key = $"{facet.Name}:{x.Term}",
                Name = x.Term,
                Selected = selectedFacets != null && selectedFacets.Contains($"{facet.Name}:{x.Term}")
            }).ToList();
        }
    }
}