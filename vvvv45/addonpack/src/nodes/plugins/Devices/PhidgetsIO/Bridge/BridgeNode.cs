#region usings
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
using Phidgets;

#endregion usings

namespace VVVV.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Bridge",
                Category = "Devices",
                Version = "Phidget",
                Help = "Controls the Bridge",
                Tags = "controller, interfacekit",
                Author = "Phlegma",
                AutoEvaluate = true
)]
    #endregion
    class BridgeNode:PhidgetMainNode<Bridge>,IPluginEvaluate
    {
        #region fields & pins

#pragma warning disable 0649
        //Input 
        [Input("Gain")]
        IDiffSpread<BridgeInput.Gains> FGainIn;

        [Input("Enable", DefaultBoolean = true)]
        IDiffSpread<bool> FEnableIn;

        //Output
        [Output("Attached")]
        ISpread<bool> FAttached;

        [Output("Sensor")]
        ISpread<double> FSensorOut;

        [Output("Gain")]
        ISpread<string> FGainOut;

        [Output("Min", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FMinOut;

        [Output("Max", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FMaxOut;

        //Logger
        [Import()]
        ILogger FLogger;



#pragma warning restore
        //private Fields
        Bridge FBridge;
        private bool FInit = true;
        private bool FFirstFrame = true;

        #endregion fields & pins

        public void OnImportsSatisfied()
        {
            //start with an empty stream output
            
            FBridge = FPhidget.FPhidget;
        }

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {

            base.Evaluate(SpreadMax);

            try
            {
                if (FBridge.Attached && FInit == false)
                {

                    SpreadMax = FBridge.bridges.Count;
                    FGainOut.SliceCount = FSensorOut.SliceCount = FMinOut.SliceCount = FMaxOut.SliceCount = SpreadMax;

                    for (int i = 0; i < SpreadMax; i++)
                    {

                        if (FGainIn.IsChanged || FFirstFrame)
                        {
                            FBridge.bridges[i].Gain = FGainIn[i];
                            FGainOut[i] = FBridge.bridges[i].Gain.ToString();
                        }
                        if (FEnableIn[i])
                        {
                            if (!FBridge.bridges[i].Enabled)
                            {
                                FBridge.bridges[i].Enabled = true;
                                FMaxOut[i] = FBridge.bridges[i].BridgeMax;
                                FMinOut[i] = FBridge.bridges[i].BridgeMin;
                            }
                            FSensorOut[i] = FBridge.bridges[i].BridgeValue;
                        }
                        else
                            FBridge.bridges[i].Enabled = false;
                    }

                    FFirstFrame = false;
                }
                else
                {
                    FSensorOut.SliceCount = 0;
                    FGainOut.SliceCount = 0;
                    FMinOut.SliceCount = 0;
                    FMaxOut.SliceCount = 0;
                    FFirstFrame = true;
                }

                FAttached[0] = FBridge.Attached;
            }
            catch (PhidgetException ex)
            {
                FLogger.Log(ex);
            }
        }
    }
}
