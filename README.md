<img src="https://user-images.githubusercontent.com/45312141/143664602-5a900f9a-6811-4e84-8aae-8220f98b2855.png" width="575">

---

**PixiBot** is a [Discord](https://discord.com) Bot whose able to parse .pixi files and encode them to a high res png image.

All you have to do is send a .pixi file in any channel and PixiBot will automatically respond with the upscaled png version.

# About PixiBot

PixiBot is a .NET 6 console application written in C#

It uses [Discord.Net](https://github.com/discord-net/Discord.Net) to connect to the Discord API, [PixiParser](https://github.com/PixiEditor/PixiParser) to parse .pixi files and [SkiaSharp](https://github.com/mono/SkiaSharp) for rendering the files to a upscaled version png version

<img src="https://user-images.githubusercontent.com/45312141/143664509-1919f9fb-8fc7-41e8-8192-c8547f427128.png" width="300">

# Invite

Currently, we host PixiBot only for our [own server](https://discord.gg/qSRMYmq).

You can however host the bot yourself in any environment that can run .NET 5 Console Applications.

# Self-Hosting

We provide [binaries](https://github.com/PixiEditor/PixiBot/releases/latest) for Windows and Linux

The bot runs perfectly fine on a Raspberry Pi 4

## Getting started

NOTE: For the bot to stay awake all the time you will need to run the whole time.

### :heavy_plus_sign: Create a bot account

You will need a Discord bot account if you want to connect to Discord. If you already have one you can skip this step.

1. Just head over to the [Discord Developer Portal](https://discord.com/developers/applications) and log in if you aren't already

2. Click on the "New Application" button and give your new application a name, you can just use your Bot's name

3. Now you can create a Bot by going in the Bot tab and hitting the "Add Bot" button, this will create the Bot account. This action is irreversible!

### :cd: Download and install the bot client

Just download the correct binary from the [releases](https://github.com/PixiEditor/PixiBot/releases/latest) for the machine you will use or you can build it yourself.

After the download is complete you will need to extract the ZIP to be able to use the client.

### :gear: Configure the Bot

You will need to tell the bot how it can log into Discord

1. Go back to the bot's profile in the Developer Portal and click copy where it says Token. 

    NOTE: Only share this token with someone you trust! You have full control over the bot with this token

2. Create a file called `appsettings.json` and open it in your favorite text editor

3. Put in the following JSON data and replace the <bot-token> with your actual bot token (Do not remove any quotation marks!)

```json
{
    "Bot": {
        "BotToken": "<bot-token>"
    },
    "Discord": {
        "GatewayIntents": "GuildMessages, DirectMessages, Guilds"
    }
}
```

The GatewayIntents tell Discord what the Bot want's to be notified about, if you don't want your Bot to respond to direct messages you can just remove the DirectMessage from the intents

### :arrow_forward: Run the bot

This step depends on your OS

#### Windows

Just run the `PixiEditor.exe`

#### Linux

Open a terminal in your Bot's directory

Run the command `.\PixiEditor` (you can also append & to the end to run the bot in the background)