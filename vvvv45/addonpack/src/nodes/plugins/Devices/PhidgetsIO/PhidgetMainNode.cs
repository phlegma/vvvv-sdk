using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phidgets;

using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
    public class PhidgetMainNode<T> : IPluginEvaluate where T :Phidget,new()
    {
        [Input("Serial", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FSerial;

        [Input("Enable Webservice", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FEnableWebservice;

        [Input("IP", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FIP;

        [Input("Port", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FPort;

        [Input("Password", IsSingle = true)]
        IDiffSpread<string> FPassword;


        public PhidgetsMain<T> FPhidget;
        
        public bool FInit = true;


        public void Evaluate(int SpreadMax)
        {
            if (FSerial.IsChanged)
            {
                if (FPhidget != null)
                {
                    FPhidget.Close();
                    FPhidget = null;
                }

                FPhidget = new PhidgetsMain<T>(FSerial[0]);
                FInit = false;
            }
        }
    }
}
