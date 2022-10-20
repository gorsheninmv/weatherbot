namespace WeatherBot.Tests.Integration

open NUnit
open NUnit.Framework
open FsUnit

module Test =

  [<Test>]
  let ``check if intgration tests works should pass`` () =
    3 + 2 |> should equal 5
