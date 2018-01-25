namespace BotDataProvider

open System
open VDS.RDF.Query
open VDS.RDF

type ElectorialResult =
    { PartyLabel : string
      BallotCount : int }

module internal ProviderDetails =
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

    let cleanRecordLabel r =
        r |> string |> (fun s -> s.Split('@') |> Seq.head)

    let toElectorialResult (r : SparqlResult) =
        let label = r.["kandidatLabel"] |> cleanRecordLabel
        let ballotCount =
            (r.["stimme"] :?> ILiteralNode).Value
            |> (fun v ->
                    match Int32.TryParse v with
                    | true, i -> i
                    | false, _ -> failwith "Parsing error" )

        { PartyLabel = label; BallotCount = ballotCount }

module Provider =
    open ProviderDetails

    let Get2013Results () =
        endpoint.QueryWithResultSet results2013Query
        |> Seq.map toElectorialResult
        |> ResizeArray<ElectorialResult>


    let Get2017Results () =
        endpoint.QueryWithResultSet results2017Query
        |> Seq.map toElectorialResult
        |> ResizeArray<ElectorialResult>