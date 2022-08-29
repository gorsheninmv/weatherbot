namespace WeatherBot.Tests.Unit

open NUnit
open NUnit.Framework
open FsUnit
open WeatherBot.App.CompositionRoot

module Test =

  [<Test>]
  let ``check if unit tests works should pass`` () =
    add 2 2 |> should equal 4
