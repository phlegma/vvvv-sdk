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
    public class BridgeNode:PhidgetMainNode<Bridge>,IPluginEvaluate
    { 
        #region fields & pins

#pragma warning disable 0649
        //Input 
        [Input("Gain")]
        IDiffSpread<BridgeInput.Gains> FGainIn;

        [Input("Activate", DefaultBoolean = true)]
        IDiffSpread<bool> FActivateIn;

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
        private bool FInit = true;
        private bool FFirstFrame = true;

        #endregion fields & pins
        

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {

            base.Evaluate(SpreadMax);

            try
            {
                if (FPhidgetMain.Attached && FInit == false && FEnable)
                {

                    SpreadMax = FPhidgetMain.FPhidget.bridges.Count;
                    FGainOut.SliceCount = FSensorOut.SliceCount = FMinOut.SliceCount = FMaxOut.SliceCount = SpreadMax;

                    for (int i = 0; i < SpreadMax; i++)
                    {

                        if (FGainIn.IsChanged || FFirstFrame)
                        {
                            FPhidgetMain.FPhidget.bridges[i].Gain = FGainIn[i];
                            FGainOut[i] = FPhidgetMain.FPhidget.bridges[i].Gain.ToString();
                        }
                        if (FActivateIn[i])
                        {
                            if (!FPhidgetMain.FPhidget.bridges[i].Enabled)
                            {
                                FPhidgetMain.FPhidget.bridges[i].Enabled = true;
                                FMaxOut[i] = FPhidgetMain.FPhidget.bridges[i].BridgeMax;
                                FMinOut[i] = FPhidgetMain.FPhidget.bridges[i].BridgeMin;
                            }
                            FSensorOut[i] = FPhidgetMain.FPhidget.bridges[i].BridgeValue;
                        }
                        else
                            FPhidgetMain.FPhidget.bridges[i].Enabled = false;
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

                FAttached[0] = FPhidgetMain.FPhidget.Attached;
            }
            catch (PhidgetException ex)
            {
                FLogger.Log(ex);
            }
        }
    }
}
