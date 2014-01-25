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
    public class BridgeNode : IPluginEvaluate
    {
        #region fields & pins

        #pragma warning disable 0649
        //Input 
        [Input("Gain")]
        IDiffSpread<BridgeInput.Gains> FGainIn;

        [Input("Serial", DefaultValue = 0, IsSingle = true, AsInt = true, MinValue = 0, MaxValue = int.MaxValue)]
        IDiffSpread<int> FSerial;

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
        WrapperBridge FBridge;
        private bool FInit = true;

        #endregion fields & pins

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            try
            {
                if (FSerial.IsChanged)
                {
                    if (FBridge != null)
                    {
                        FBridge.Close();
                        FBridge = null;
                    }
                    FBridge = new WrapperBridge(FSerial[0]);
                    FInit = false;
                }

                if (FBridge.Attached && FInit == false)
                {

                    SpreadMax = FBridge.Count;
                    FGainOut.SliceCount = FSensorOut.SliceCount = FMinOut.SliceCount = FMaxOut.SliceCount = FBridge.Count;

                    for (int i = 0; i < SpreadMax; i++)
                    {
                        if (FBridge.Changed)
                            FSensorOut[i] = FBridge.GetSensorValue(i);

                        if (FGainIn.IsChanged)
                        {
                            FBridge.SetBridgeGain(i, FGainIn[i]);
                            FGainOut[i] = FBridge.GetGain(i);
                        }

                        if (FEnableIn[i])
                        {
                            if (!FBridge.GetBridgeEnable(i))
                            {
                                FBridge.SetBridgeEnable(i, true);
                                FMaxOut[i] = FBridge.GetBridgeMax(i);
                                FMinOut[i] = FBridge.GetBridgeMin(i);
                            }
                        }
                        else
                            FBridge.SetBridgeEnable(i, false);
                    }
                }
                else
                {
                    FSensorOut.SliceCount = 0;
                    FGainOut.SliceCount = 0;
                    FMinOut.SliceCount = 0;
                    FMaxOut.SliceCount = 0;

                }

                FAttached[0] = FBridge.Attached;

                List<PhidgetException> Exceptions = FBridge.Errors;
                if (Exceptions != null)
                {
                    foreach (Exception e in Exceptions)
                        FLogger.Log(e);
                }
            }
            catch (PhidgetException ex)
            {
                FLogger.Log(ex);
            }
        }
    }
}
