namespace WeatherBot.Tests.Unit

open NUnit
open NUnit.Framework
open FsUnit

module Test =

  [<Test>]
  let ``check if unit tests works should pass`` () =
    2 + 2 |> should equal 4
