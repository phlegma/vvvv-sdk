using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phidgets;

using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
    public class PhidgetMainNode<T> : IPluginEvaluate where T : Phidget, new()
    {

        #pragma warning disable 0649
        [Input("Serial", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FSerial;

        [Input("Connect Webservice", DefaultValue = 0, IsSingle = true)]
        IDiffSpread<bool> FEnableWebservice;

        [Input("IP", DefaultString = "127.0.0.1", IsSingle = true)]
        IDiffSpread<string> FIP;

        [Input("Port", DefaultValue = 5001, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = 65535)]
        IDiffSpread<int> FPort;

        [Input("Password", IsSingle = true)]
        IDiffSpread<string> FPassword;

        [Input("Enable", IsSingle = true, DefaultBoolean = true)]
        IDiffSpread<bool> FEnableIn;

        [Output("Connected")]
        ISpread<bool> FConnected;
        #pragma warning restore

        public PhidgetsMain<T> FPhidgetMain;
        public bool FInit = true;
        public bool FEnable = true;

        public void Evaluate(int SpreadMax)
        {
            FEnable = FEnableIn[0];

            if (FSerial.IsChanged || FEnableWebservice.IsChanged)
            {
                if (FPhidgetMain != null)
                {
                    FPhidgetMain.Close();
                    FPhidgetMain = null;
                }

                if (!FEnableWebservice[0])
                {
                    FPhidgetMain = new PhidgetsMain<T>(FSerial[0]);
                }
                else
                {
                    FPhidgetMain = new PhidgetsMain<T>(FSerial[0], FIP[0], FPort[0], FPassword[0]);
                }
                FInit = false;
            }

            FConnected[0] = FPhidgetMain.Connected;
        }
    }
}
