#r @"../ConsoleApp1/packages/HtmlAgilityPack.1.6.0/lib/Net40/HtmlAgilityPack.dll"
#r @"../ConsoleApp1/packages/Newtonsoft.Json.10.0.3/lib/net40/Newtonsoft.Json.dll"
#r @"../ConsoleApp1/packages/VDS.Common.1.9.0/lib/net40-client/VDS.Common.dll"
#r @"../ConsoleApp1/packages/dotNetRDF.2.0.0/lib/net40/dotNetRDF.dll"

#load "DataProviderLib.fs"

open System
open VDS.RDF.Query
open VDS.RDF

let [<Literal>] query =
    @"#Alle Politiker mit österr. Staatsbürgerschaft
        SELECT ?item ?itemLabel WHERE {
            ?item wdt:P106 wd:Q82955;
            wdt:P27 wd:Q40.
            SERVICE wikibase:label { bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],de"". }
        }"

let [<Literal>] results2017Query =
    @"SELECT ?kandidat ?kandidatLabel ?stimme WHERE {
        SERVICE wikibase:label { bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],en"". }
          wd:Q19311231 p:P726 [ ps:P726 ?kandidat; pq:P1111 ?stimme ].
        }"

let [<Literal>] results2013Query =
    @"SELECT ?kandidat ?kandidatLabel ?stimme WHERE {
        SERVICE wikibase:label { bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],en"". }
          wd:Q1386143 p:P726 [ ps:P726 ?kandidat; pq:P1111 ?stimme ].
        }"

let endpoint = SparqlRemoteEndpoint (Uri("https://query.wikidata.org/sparql"))

let result = endpoint.QueryWithResultSet results2017Query

let cleanRecordLabel r =
    r |> string |> (fun s -> s.Split('@') |> Seq.head)

let r1 = result |> Seq.head
let bla = r1.["stimme"] :?> ILiteralNode

let [<Literal>] res2017Query =
        @"SELECT ?kandidat ?kandidatLabel ?stimme WHERE {{
            SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],{0}"". }}
              wd:Q19311231 p:P726 [ ps:P726 ?kandidat; pq:P1111 ?stimme ].
            }}"

let [<Literal>] res2013Query =
    @"SELECT ?kandidat ?kandidatLabel ?stimme WHERE {{
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],{0}"". }}
          wd:Q1386143 p:P726 [ ps:P726 ?kandidat; pq:P1111 ?stimme ].
        }}"

open BotDataProvider.ProviderDetails

insertLabelLang res2013Query "de"