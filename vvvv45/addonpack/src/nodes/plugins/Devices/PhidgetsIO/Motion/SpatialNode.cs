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
    [PluginInfo(Name = "Spatial",
                Category = "Devices",
                Version = "Phidget",
                Help = "Manages the 1043_0 - PhidgetSpatial",
                Tags = "controller",
                Author = "Phlegma",
                AutoEvaluate = true
)]
    #endregion PluginInfo

    public class SpatialNode : PhidgetMainNode<Spatial>, IPluginEvaluate
    {
        #region fields & pins

#pragma warning disable 0649
        //Input 
        [Input("Datarate", DefaultValue = 16, MinValue = 8, MaxValue = Int16.MaxValue, StepSize=8)]
        IDiffSpread<int> FDataRate;

        [Output("Datarate Minimum", Visibility=PinVisibility.OnlyInspector)]
        ISpread<int> FDataRateMin;

        [Output("Datarate Maximum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<int> FDataRateMax;

        //Output
        [Output("Attached")]
        ISpread<bool> FAttached;

        [Output("Acceleration")]
        ISpread<double> FAcceleration;

        [Output("Acceleration Minimum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FAccelerationMin;

        [Output("Acceleration Maximum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FAccelerationMax;

        [Output("Compass")]
        ISpread<double> FCompass;

        [Output("Compass Minimum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FCompassMin;

        [Output("Compass Maximum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FCompassMax;

        [Output("Gyros")]
        ISpread<double> FGyros;

        [Output("Gyros Minimum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FGyrosMin;

        [Output("Gyros Maximum", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FGyrosMax;


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
                if (FPhidgetMain.Attached && FInit == false && FEnable)
                {
                    int SliceCountAcceleration = FPhidgetMain.FPhidget.accelerometerAxes.Count;
                    int SliceCountCompass = FPhidgetMain.FPhidget.compassAxes.Count;
                    int SliceCountGyro = FPhidgetMain.FPhidget.gyroAxes.Count;

                    FAcceleration.SliceCount = FAccelerationMax.SliceCount = FAccelerationMin.SliceCount = SliceCountAcceleration;
                    for (int i = 0; i < SliceCountAcceleration; i++)
                    {
                        FAcceleration[i] = FPhidgetMain.FPhidget.accelerometerAxes[i].Acceleration;
                        FAccelerationMin[i] = FPhidgetMain.FPhidget.accelerometerAxes[i].AccelerationMin;
                        FAccelerationMax[i] = FPhidgetMain.FPhidget.accelerometerAxes[i].AccelerationMax;
                    }

                    FGyros.SliceCount = FGyrosMax.SliceCount = FGyrosMin.SliceCount = SliceCountCompass;
                    for (int i = 0; i < SliceCountGyro; i++)
                    {
                        FAcceleration[i] = FPhidgetMain.FPhidget.compassAxes[i].MagneticField;
                        FAccelerationMin[i] = FPhidgetMain.FPhidget.compassAxes[i].MagneticFieldMin;
                        FAccelerationMax[i] = FPhidgetMain.FPhidget.compassAxes[i].MagneticFieldMax;
                    }

                    for (int i = 0; i < SliceCountAcceleration; i++)
                    {
                        FAcceleration[i] = FPhidgetMain.FPhidget.gyroAxes[i].AngularRate;
                        FAccelerationMin[i] = FPhidgetMain.FPhidget.gyroAxes[i].AngularRateMin;
                        FAccelerationMax[i] = FPhidgetMain.FPhidget.gyroAxes[i].AngularRateMax;
                    }

                    if (FDataRate.IsChanged)
                    {
                        for (int i = 0; i < SliceCountAcceleration; i++)
                        {
                            //can only pass values that are a multiple of 8
                            int val = FDataRate[i];
                            if (val >= 8)
                            {
                                val = (int)(val / 8) * 8;
                            }
                            else
                            {
                                val = 8;
                            }

                            FPhidgetMain.FPhidget.DataRate = val;
                            FDataRateMax[0] = FPhidgetMain.FPhidget.DataRateMax;
                            FDataRateMin[0] = FPhidgetMain.FPhidget.DataRateMin;
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
