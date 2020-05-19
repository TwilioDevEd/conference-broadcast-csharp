
<a  href="https://www.twilio.com">
<img  src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg"  alt="Twilio"  width="250"  />
</a>

# Rapid Response Kit: Building Conferencing and Broadcasting with Twilio. Level: Intermediate. Powered by Twilio - ASP.NET MVC

![](https://github.com/TwilioDevEd/conference-broadcast-csharp/workflows/NetFx/badge.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/j65aeh8inbqlav0s?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/conference-broadcast-csharp)

> We are currently in the process of updating this sample template. If you are encountering any issues with the sample, please open an issue at [github.com/twilio-labs/code-exchange/issues](https://github.com/twilio-labs/code-exchange/issues) and we'll try to help you.

## About

An example application implementing an disaster response kit that allows an organizer to instantly communicate with volunteers.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/conference-broadcast/csharp/mvc)!

Implementations in other languages:

| PHP | Java | Python | Ruby | Node |
| :--- | :--- | :----- | :-- | :--- |
| [Done](https://github.com/TwilioDevEd/conference-broadcast-laravel)  | [Done](https://github.com/TwilioDevEd/conference-broadcast-spark)  | TBD  | [Done](https://github.com/TwilioDevEd/conference-broadcast-rails) | TDB |

<!--
### How it works

**TODO: Describe how it works**
-->

## Set up

### Requirements

- [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework/net472)
- A Twilio account - [sign up](https://www.twilio.com/try-twilio)
- [ngrok](https://ngrok.com)

### Twilio Account Settings

This application should give you a ready-made starting point for writing your
own application. Before we begin, we need to collect
all the config values we need to run the application:

| Config&nbsp;Value | Description                                                                                                                                                  |
| :---------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Account&nbsp;Sid  | Your primary Twilio account identifier - find this [in the Console](https://www.twilio.com/console).                                                         |
| Auth&nbsp;Token   | Used to authenticate - [just like the above, you'll find this here](https://www.twilio.com/console).                                                         |
| Phone&nbsp;number | A Twilio phone number in [E.164 format](https://en.wikipedia.org/wiki/E.164) - you can [get one here](https://www.twilio.com/console/phone-numbers/incoming) |

### Local development

After the above requirements have been met:

1. Clone this repository and `cd` into it

```bash
git clone git@github.com:TwilioDevEd/conference-broadcast-csharp.git
cd conference-broadcast-csharp
```

2. Set your configuration variables

```bash
copy ConferenceBroadcast.Web/Local.config.example ConferenceBroadcast.Web/Local.config
```

See [Twilio Account Settings](#twilio-account-settings) to locate the necessary environment variables. For the two phone numbers, you can use different phone numbers, or the same phone number. (The first number is for broadcasts, the second is for the conference line.) You can buy numbers [right here](https://www.twilio.com/user/account/phone-numbers/search).

3. Build the solution

4. Run the application

5. To run this application with Twilio, you will need to make it publicly accessible. [We recommend using ngrok for this.](https://www.twilio.com/docs/usage/tutorials/how-use-ngrok-windows-and-visual-studio-test-webhooks).

```bash
ngrok http 1229 -host-header="localhost:1229"
```

Or, use [ngrok Extensions](https://marketplace.visualstudio.com/items?itemName=DavidProthero.NgrokExtensions) for Visual Studio. Select "Tools... Start ngrok Tunnel".

6. Open [the number management page](https://www.twilio.com/user/account/phone-numbers/incoming)
and open a number's configuration by clicking on it.

Remember that the number where you change the voice webhooks must be the same one you set on the `RapidResponsePhoneNumber` setting.

For this application, you must set the voice webhook of your number to something like this:

```
http://<your-ngrok-subdomain>.ngrok.io/Conference/Join
```

And in this case set the `POST` method on the configuration for this webhook.

7. Open the application in your browser *using the ngrok url*: `http://<your-ngrok-subdomain>.ngrok.io/`

That's it!

## Resources

- The CodeExchange repository can be found [here](https://github.com/twilio-labs/code-exchange/).

## Contributing

This template is open source and welcomes contributions. All contributions are subject to our [Code of Conduct](https://github.com/twilio-labs/.github/blob/master/CODE_OF_CONDUCT.md).

[Visit the project on GitHub](https://github.com/twilio-labs/sample-template-dotnet)

## License

[MIT](http://www.opensource.org/licenses/mit-license.html)

## Disclaimer

No warranty expressed or implied. Software is as is.

[twilio]: https://www.twilio.com
