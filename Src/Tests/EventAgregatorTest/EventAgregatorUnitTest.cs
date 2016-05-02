using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.SDK.Composite.Event;
using System.Diagnostics;
using System.Threading;
using Core.SDK.Composite.Common;

namespace EventAgregatorTest
{
    [TestClass]
    public class EventAgregatorUnitTest
    {
        Core.SDK.Log.ILogMgr _LogMgr;
        Core.SDK.Log.ILogger _Logger;
        public EventAgregatorUnitTest()
        {            
            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("EventAgregatorUnitTest");
            _Logger.Info("Start EventAgregatorUnitTest.");           
        }

        public class TestCompositionEvent1 : CompositeEvent<string>
        { }

        public class TestCompositionEvent2 : CompositeEvent<string>
        { }

        public class TestCompositionEvent3 : CompositeEvent<UserMessage>
        { }

        public class UserMessage
        {
            public string Text { get; set; }

            public UserMessage() { }

            public UserMessage(string text)
            {
                this.Text = text;
            }
        }

        string _message;
        void EventHandlerMethod(string str)
        {
            _message = str;
        }

        void EventHandlerMethod2(string str)
        {
            _message = _message + str;
        }

        [TestMethod]
        public void TestSubscribeUnsubscribe()
        {
            string text = "1";
            IEventMgr eventMgr = new EventMgr(_LogMgr);
            IdentKey subsId = eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(str => { text = str; });
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("2");
            Debug.Assert(string.Equals(text, "2"), "Subscribe1 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("");
            Debug.Assert(string.IsNullOrEmpty(text), "Subscribe2 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish(null);
            Debug.Assert(string.IsNullOrEmpty(text), "Subscribe3 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("3");
            Debug.Assert(string.Equals(text, "3"), "Subscribe4 fail");
            
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(subsId);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("4");
            Debug.Assert(string.Equals(text, "3"), "Subscribe after unsubscribe fail");
        }

        [TestMethod]
        public void TestSubscribeUnsubscribe2()
        {
            _message = "1";
            IEventMgr eventMgr = new EventMgr(_LogMgr);
            IdentKey subsId = eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod);
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("2");
            Debug.Assert(string.Equals(_message, "2"), "Subscribe1 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("");
            Debug.Assert(string.IsNullOrEmpty(_message), "Subscribe2 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish(null);
            Debug.Assert(string.IsNullOrEmpty(_message), "Subscribe3 fail");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("3");
            Debug.Assert(string.Equals(_message, "3"), "Subscribe4 fail");

            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("4");
            Debug.Assert(string.Equals(_message, "3"), "Subscribe after unsubscribe fail");
        }

        [TestMethod]
        public void TestSubscribeUnsubscribe3()
        {
            _message = "1";
            IEventMgr eventMgr = new EventMgr(_LogMgr);
            IdentKey subsId = eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod);
            eventMgr.GetEvent<TestCompositionEvent2>().Publish("2");
            Debug.Assert(string.Equals(_message, "1"), "Subscribe1 fail");

            eventMgr.GetEvent<TestCompositionEvent2>().Subscribe(EventHandlerMethod);
            eventMgr.GetEvent<TestCompositionEvent2>().Publish("2");
            Debug.Assert(string.Equals(_message, "2"), "Subscribe2 fail");           
        }

        [TestMethod]
        public void TestSubscribeUnsubscribe4()
        {
            _message = "";
            IEventMgr eventMgr = new EventMgr(_LogMgr);
            // Три разным подписки на одно событие, EventHandlerMethod2 - конкатенация
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("22");
            Debug.Assert(string.Equals(_message, "222222"), "Subscribe1 fail");

            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("1");
            Debug.Assert(string.Equals(_message, "222222"), "Subscribe2 fail");
        }


        [TestMethod]
        public void TestSubscribeUnsubscribe5()
        {
            _message = "";
            IEventMgr eventMgr = new EventMgr(_LogMgr);
            // Три разным подписки на одно событие, EventHandlerMethod2 - конкатенация
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2,  ThreadOption.BackgroundThread);
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2, ThreadOption.BackgroundThread);
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2, ThreadOption.BackgroundThread);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("22");            

            Thread.Sleep(2000);

            Debug.Assert(string.Equals(_message, "222222"), "Subscribe1 fail");

            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);           

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("1");
            Debug.Assert(string.Equals(_message, "222222"), "Subscribe2 fail");
        }


        [TestMethod]
        public void TestSubscribeUnsubscribe6()
        {
            _message = "1";
            IEventMgr eventMgr = new EventMgr(_LogMgr);

            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2, ThreadOption.BackgroundThread);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("2");
            Debug.Assert(string.Equals(_message, "1"), "Subscribe1 fail");

            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod, ThreadOption.BackgroundThread);            
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2, ThreadOption.BackgroundThread);
            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod2);

            eventMgr.GetEvent<TestCompositionEvent1>().Publish("22");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("33");
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("44");

            Thread.Sleep(2000);

            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);
            eventMgr.GetEvent<TestCompositionEvent1>().Unsubscribe(EventHandlerMethod2);

            eventMgr.GetEvent<TestCompositionEvent1>().Subscribe(EventHandlerMethod);  
            eventMgr.GetEvent<TestCompositionEvent1>().Publish("55");
            Debug.Assert(string.Equals(_message, "55"), "Subscribe1 fail");                      
        }
    }
}
