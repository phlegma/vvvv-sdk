#region usings
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
using Phidgets;

#endregion usings

namespace VVVV.Nodes
{
    #region PluginInfo
    [PluginInfo(Name = "Accelerometer",
                Category = "Devices",
                Version = "Phidget",
                Help = "Manages the 1059 - PhidgetAccelerometer 3-Axis",
                Tags = "controller",
                Author = "Phlegma",
                AutoEvaluate = true
)]
    #endregion PluginInfo

    public class AccelerometerNode : PhidgetMainNode<Accelerometer>, IPluginEvaluate
    {
        #region fields & pins

#pragma warning disable 0649
        //Input 
        [Input("Sensitivy", DefaultValue = 0, MinValue = 0, MaxValue = 1)]
        IDiffSpread<double> FSensitivy;

        //Output
        [Output("Attached")]
        ISpread<bool> FAttached;

        [Output("Acceleration")]
        ISpread<double> FAcceleration;

        [Output("Acceleration Minimum")]
        ISpread<double> FAccelerationMin;

        [Output("Acceleration Maximum")]
        ISpread<double> FAccelerationMax;


        //Logger
        [Import()]
        ILogger FLogger;

#pragma warning restore

        //private Fields
        #endregion fields & piins


        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {

            base.Evaluate(SpreadMax);
            try
            {
                if (FPhidgetMain.Attached && FInit == false)
                {
                    int SliceCount = FPhidgetMain.FPhidget.axes.Count;
                    FAcceleration.SliceCount = SliceCount;
                    FAccelerationMax.SliceCount = SliceCount;
                    FAccelerationMin.SliceCount = SliceCount;
                    for (int i = 0; i < SliceCount; i++)
                    {
                        FAcceleration[i] = FPhidgetMain.FPhidget.axes[i].Acceleration;
                        FAccelerationMin[i] = FPhidgetMain.FPhidget.axes[i].AccelerationMin;
                        FAccelerationMax[i] = FPhidgetMain.FPhidget.axes[i].AccelerationMax;
                    }

                    if (FSensitivy.IsChanged)
                    {
                        for (int i = 0; i < SliceCount; i++)
                        {
                            FPhidgetMain.FPhidget.axes[i].Sensitivity = FSensitivy[i];
                        }
                    }

                }

                FAttached[0] = FPhidgetMain.FPhidget.Attached;

                List<PhidgetException> Exceptions = FPhidgetMain.Errors;
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
