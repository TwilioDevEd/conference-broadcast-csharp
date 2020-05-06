<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# Rapid Response Kit: Building Conferencing and Broadcasting with Twilio. Level: Intermediate. Powered by Twilio - ASP.NET MVC

![](https://github.com/TwilioDevEd/conference-broadcast-csharp/workflows/NetFx/badge.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/j65aeh8inbqlav0s?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/conference-broadcast-csharp)

> We are currently in the process of updating this sample template. If you are encountering any issues with the sample, please open an issue at [github.com/twilio-labs/code-exchange/issues](https://github.com/twilio-labs/code-exchange/issues) and we'll try to help you.

An example application implementing an disaster response kit that allows an organizer to instantly communicate with volunteers.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/conference-broadcast/csharp/mvc)!

### Local development

1. To run this application with Twilio, you will need to make it publicly accessible. [We recommend using ngrok for this.](https://www.twilio.com/docs/usage/tutorials/how-use-ngrok-windows-and-visual-studio-test-webhooks) Install [ngrok](https://ngrok.com/download) now, or install the [ngrok Extensions for Visual Studio](https://marketplace.visualstudio.com/items?itemName=DavidProthero.NgrokExtensions).

2. First clone this repository and `cd` into its directory:
   ```
   git clone git@github.com:TwilioDevEd/conference-broadcast-csharp.git

   cd conference-broadcast-csharp
   ```

3. Copy the file `ConferenceBroadcast.Web/Local.config.example` to `ConferenceBroadcast.Web/Local.config`.

4. Update those values in the `Local.config` file to match your Twilio account. You can get your Account SID and auth token from your [dashboard](https://www.twilio.com/console). For the two phone numbers, you can use different phone numbers, or the same phone number. (The first number is for broadcasts, the second is for the conference line.) You can buy numbers [right
here](https://www.twilio.com/user/account/phone-numbers/search).

5. Build the solution in Visual Studio.

6. Run the application. You'll see it start up at `http://localhost:1229`, but we aren't quite ready yet.

7. Start ngrok now, to make the application publicly accessible. Either run this command:
    ```
    ngrok http 1229 -host-header="localhost:1229"
    ```

    Or, if you installed the [ngrok Extensions for Visual Studio](https://marketplace.visualstudio.com/items?itemName=DavidProthero.NgrokExtensions), choose "Start ngrok Tunnel" from the "Tools" menu in Visual Studio.

8. Open [the number management page](https://www.twilio.com/user/account/phone-numbers/incoming)
and open a number's configuration by clicking on it.

    Remember that the number where you change the voice webhooks must be the same one you set on
the `RapidResponsePhoneNumber` setting.

    For this application, you must set the voice webhook of your number to something like this:

    ```
    http://<your-ngrok-subdomain>.ngrok.io/Conference/Join
    ```

    And in this case set the `POST` method on the configuration for this webhook.

9. Open the application in your browser *using the ngrok url*: `http://<your-ngrok-subdomain>.ngrok.io/`

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* The CodeExchange repository can be found [here](https://github.com/twilio-labs/code-exchange/).
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
