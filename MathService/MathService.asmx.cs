using System;
using System.Web.Services;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using System.IO;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web;

namespace MathService
{
    /// <summary>
    /// Summary description for MathService
    /// </summary>
    [WebService(Namespace = "http://192.168.0.22/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class MathService : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //This method is designed to check push notification, however without real device it's not possible
        public string HelloWorld(string device)
        {
            //create the puchbroker object
            var push = new PushBroker();
            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;         
                   
            try
            {
                var appleCert = File.ReadAllBytes(Server.MapPath("certificate directory"));
                       
                push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "certificate password"));
                       
                push.QueueNotification(new AppleNotification()
                                                    .ForDeviceToken(device)//the recipient device id
                                                    .WithAlert("Hello")//the message
                                                    .WithBadge(1)
                                                    .WithSound("sound.caf")
                                                    );
            }
            catch (Exception ex)
            {
                 throw ex;
            }
             
            push.StopAllServices(waitForQueuesToFinish: true);
            return "Hello, World!";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        //Method that is called from "http_webserver" application
        public void Add(int a, int b)
        {
            //create object which converts some object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //create object which stores result
            var json = new {
                res = a+b,
            };
            //convert object to json and response to the client
            HttpContext.Current.Response.Write(serializer.Serialize(json));
        }     


        //All methods below are created for push notification service, currently they are useless
        static void DeviceSubscriptionChanged(object sender,
        string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Do something here
        }

        //this even raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //Do something here
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //Do something here
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //Do something here
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //Do something here
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Do something here
        }

    }
}
