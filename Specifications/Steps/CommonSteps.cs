using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation.Sms;
using TechTalk.SpecFlow;
using Implementation;
using TypeMock.ArrangeActAssert;

namespace Specifications.Steps
{
    [Binding]
    public class CommonSteps
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            Isolate.WhenCalled(() => MessageDispatcher.DispatchMessage(null)).WillReturn(false);
            Application.Instance.ReceivedMessages.Clear();
            Application.Instance.SentMessages.Clear();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Isolate.CleanUp();
        }
    }
}
