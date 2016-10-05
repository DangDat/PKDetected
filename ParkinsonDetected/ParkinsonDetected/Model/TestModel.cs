using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkinsonDetected.Model
{
    public partial class TestModel : PageModelBase
    {
        public TestModel Self
        {
            get { return this; }
        }

        private bool testBusy;
        public bool TestBusy
        {
            get { return testBusy; }
            set { Set("testBusy", ref testBusy, value, true); }
        }
    }
}
