namespace WeatherBot.Tests.Integration

open NUnit
open NUnit.Framework
open FsUnit
open WeatherBot.App.CompositionRoot

module Test =

  [<Test>]
  let ``check if intgration tests works should pass`` () =
    add 3 2 |> should equal 5
