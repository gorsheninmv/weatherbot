#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Testing.NUnit
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "bin"
    ++ "tests"
    |> Shell.cleanDirs
)

Target.create "BuildApp" (fun _ ->
    !! "src/WeatherBot.App/*.*proj"
    |> Seq.iter (DotNet.build (fun opts ->
      { opts with OutputPath = Some "bin" }))
)

Target.create "BuildTests" (fun _ ->
    !! "src/WeatherBot.Tests/*.*proj"
    |> Seq.iter (DotNet.build (fun opts ->
      { opts with OutputPath = Some "tests" }))
)

let setTestsOpts (opts: DotNet.TestOptions) = {
  opts with
    Output = Some "tests";
    ResultsDirectory = Some "tests/results";
    Logger = Some "html;LogFileName=result.html";
    NoRestore = true;
    NoBuild = true;
  }

let setUnitTestsFilter (opts: DotNet.TestOptions) =
  { opts with Filter = Some "FullyQualifiedName~.Tests.Unit."  }

Target.create "RunTests" (fun p ->
    let unitOnly = p.Context.Arguments |> Seq.contains "--unit-only"
    let optsBuilder =
      match unitOnly with
      | true -> setTestsOpts >> setUnitTestsFilter
      | false -> setTestsOpts
    !! "src/WeatherBot.Tests/*.*proj"
    |> Seq.iter (DotNet.test optsBuilder)
)

Target.create "All" ignore

("Clean" ==> "BuildApp") <=> ("Clean" ==> "BuildTests" ==> "RunTests") ==> "All"
Target.runOrDefaultWithArguments "All"
