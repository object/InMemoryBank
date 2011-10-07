using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Implementation;

namespace Specifications.Steps
{
    [Binding]
    public class CommonSteps
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            Application.Instance.ReceivedMessages.Clear();
            Application.Instance.SentMessages.Clear();
        }
    }
}
