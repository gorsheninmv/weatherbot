open System
open Funogram.Api
open Funogram.Telegram
open Funogram.Telegram.Bot
open WeatherBot.App.Weather.Infrastructure.Telegram

let messageHanlder: MessageHandler = fun (mId, text) ->
  $"{mId}: {text}"

[<EntryPoint>]
let main _ =
  startBot id messageHanlder
