using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace SparqlAccess
{
    public class SparqlTest
    {
        public void Test() {

            var queryString =  @"# Alle Politiker mit österr. Staatsbürgerschaft
SELECT ?item ?itemLabel WHERE {
  ?item wdt:P106 wd:Q82955;
        wdt:P27 wd:Q40.
  SERVICE wikibase:label { bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],de"". }
}";

            SparqlRemoteEndpoint endpoint =
                new SparqlRemoteEndpoint(
                    new Uri("https://query.wikidata.org/sparql"),
                    "https://query.wikidata.org/sparql");

            var results = endpoint.QueryWithResultSet(queryString);
            foreach (SparqlResult sparqlResult in results) {
                Console.WriteLine(sparqlResult);
            }



        }
    }
}
