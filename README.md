<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# Rapid Response Kit: Building Conferencing and Broadcasting with Twilio. Level: Intermediate. Powered by Twilio - ASP.NET MVC

[![Build status](https://ci.appveyor.com/api/projects/status/j65aeh8inbqlav0s?svg=true)](https://ci.appveyor.com/project/TwilioDevEd/conference-broadcast-csharp)

An example application implementing an disaster response kit that allows an organizer to instantly communicate with volunteers.

[Read the full tutorial here](https://www.twilio.com/docs/tutorials/walkthrough/conference-broadcast/csharp/mvc)!

### Local development

1. First clone this repository and `cd` into its directory:
   ```
   git clone git@github.com:TwilioDevEd/conference-broadcast-csharp.git

   cd conference-broadcast-csharp
   ```

2. Create a new file ConferenceBroadcast.Web/Local.config and update the content with:

   ```
   <appSettings>
     <add key="webpages:Version" value="3.0.0.0"/>
     <add key="webpages:Enabled" value="false"/>
     <add key="ClientValidationEnabled" value="true"/>
     <add key="UnobtrusiveJavaScriptEnabled" value="true"/>
     <add key="TwilioAccountSid" value="Your Twilio Account SID" />
     <add key="TwilioAuthToken" value="Your Twilio Auth Token" />
     <add key="TwilioPhoneNumber" value="Your Twilio Phone Number" />
     <add key="RapidResponsePhoneNumber" value="Your Rapid Response Phone Number" />
   </appSettings>
   ```

3. Build the solution.

4. Run the application.

5. Check it out at [http://localhost:1229](http://localhost:1229)

That's it

### Configure Twilio to call your webhooks
You will also need to configure Twilio to call your application when calls are received.

You will need to provision at least one Twilio number with voice capabilities
so the application's users can take surveys. You can buy a number [right
here](https://www.twilio.com/user/account/phone-numbers/search). Once you have
a number you need to configure your number to work with your application. Open
[the number management page](https://www.twilio.com/user/account/phone-numbers/incoming)
and open a number's configuration by clicking on it.

Remember that the number where you change the voice webhooks must be the same one you set on
the `RapidResponsePhoneNumber` setting.

![Configure Voice](http://howtodocs.s3.amazonaws.com/twilio-number-config-all-med.gif)

For this application, you must set the voice webhook of your number to
something like this:

```
http://<your-ngrok-subdomain>.ngrok.io/Conference/Join
```

And in this case set the `POST` method on the configuration for this webhook.

#### Using ngrok

Endpoints like `/Conference/Join` needs to be publicly accessible. [We recommend using ngrok to solve this problem](https://www.twilio.com/blog/2015/09/6-awesome-reasons-to-use-ngrok-when-testing-webhooks.html).

```
ngrok http 1229 -host-header="localhost:1229"
```

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
