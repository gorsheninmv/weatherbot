namespace WeatherBot.App.Weather.Infrastructure

  module Telegram =

    open WeatherBot.App.Common.Monads
    open FsConfig
    open Funogram.Api
    open Funogram.Telegram
    open Funogram.Telegram.Bot

    type MessageHandler = (int64 * string) -> string

    let private getToken () =
      let result = EnvConfig.Get<string> "WEATHER_BOT_TOKEN"
      match result with
      | Ok token -> token
      | Error error -> failwith "Telegram token not found!"

    let private handleRequest (messageHanlder: MessageHandler) (ctx: UpdateContext) =
      let info = maybe {
        let! message = ctx.Update.Message
        let! text = message.Text
        let messageId = message.MessageId
        let chat = message.Chat
        return (text, messageId, chat)
      }
      match info with
      | Some (text, messageId, chat) ->
        let message = messageHanlder (chat.Id, text)
        Api.sendMessageReply chat.Id message messageId
        |> api ctx.Config
        |> Async.Ignore
        |> Async.Start
      | _ -> ()

    let private startBotInternal config handler =
      async {
          let! _ = Api.deleteWebhookBase () |> api config
          return! startBot config handler None
        } |> Async.RunSynchronously
      0

    let private setToken token (config: Funogram.Types.BotConfig) =
      { config with Token = token }

    let startBot updateConfig messageHanlder =
      let token = getToken ()
      let buildConfig = updateConfig >> setToken token
      let config = buildConfig Config.defaultConfig
      let requestHandler = handleRequest messageHanlder
      startBotInternal config requestHandler
