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
    [PluginInfo(Name = "IO",
                Category = "Devices",
                Version = "Phidget",
                Help = "Controls the Phidget Interface Kits",
                Tags = "controller, interfacekit",
                Author = "Phlegma",
                AutoEvaluate = true
)]
    #endregion
    public class InterfaceBoard : PhidgetMainNode<InterfaceKit>, IPluginEvaluate
    {
        #region fields & pins

#pragma warning disable 0649


        //Input 
        [Input("Digital", DefaultValue = 0)]
        IDiffSpread<bool> FDigitalIn;

        [Input("Radiometric", IsSingle = true)]
        IDiffSpread<bool> FRadiometricIn;

        [Input("Sensitivity",AsInt=true, DefaultValue = 0, MinValue = 0, MaxValue = 1000)]
        IDiffSpread<int> FSensitivity;

        [Input("DataRate (ms)", DefaultValue = 16, Visibility = PinVisibility.OnlyInspector)]
        IDiffSpread<int> FDataRateIn;

        //Output
        [Output("Attached")]
        ISpread<bool> FAttached;

        [Output("Sensor")]
        ISpread<double> FSensorOut;

        [Output("Digital")]
        ISpread<bool> FDigitalOut;

        [Output("DataRateMin(ms)", Visibility = PinVisibility.OnlyInspector)]
        ISpread<int> FDataRateMinOut;

        [Output("DataRateMax(ms)", Visibility = PinVisibility.OnlyInspector)]
        ISpread<int> FDataRateMaxOut;


        //Logger
        [Import()]
        ILogger FLogger;

#pragma warning restore
        //private Fields
        private bool FFirstTime = true;

        #endregion fields & pins

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            try
            {
                base.Evaluate(SpreadMax);

                if (FPhidgetMain.Attached)
                {
                    int SensorCount = FPhidgetMain.FPhidget.sensors.Count;
                    int InputCount = FPhidgetMain.FPhidget.inputs.Count;
                    int OutputCount = FPhidgetMain.FPhidget.outputs.Count;

                    if (!(FPhidgetMain.FPhidget.ID == Phidget.PhidgetID.LINEAR_TOUCH || FPhidgetMain.FPhidget.ID == Phidget.PhidgetID.ROTARY_TOUCH))
                    {
                        if (FRadiometricIn.IsChanged || FFirstTime)
                            FPhidgetMain.FPhidget.ratiometric = FRadiometricIn[0];
                    }

                    if (FDataRateIn.IsChanged || FSensitivity.IsChanged || FFirstTime)
                    {
                        for (int i = 0; i < SensorCount; i++)
                        {
                            FPhidgetMain.FPhidget.sensors[i].DataRate = FDataRateIn[i];
                            FPhidgetMain.FPhidget.sensors[i].Sensitivity = FSensitivity[i];
                        }
                    }

                    if (FDigitalIn.IsChanged || FFirstTime)
                    {
                        for (int i = 0; i < OutputCount; i++)
                            FPhidgetMain.FPhidget.outputs[i] = FDigitalIn[i];
                    }


                    FDigitalOut.SliceCount = InputCount;
                    for (int i = 0; i < InputCount; i++)
                    {
                        FDigitalOut[i] = FPhidgetMain.FPhidget.inputs[i];
                    }

                    FSensorOut.SliceCount = FDataRateMaxOut.SliceCount = FDataRateMinOut.SliceCount = SensorCount;
                    for (int i = 0; i < SensorCount; i++)
                    {

                        if (!(FPhidgetMain.FPhidget.ID == Phidget.PhidgetID.LINEAR_TOUCH || FPhidgetMain.FPhidget.ID == Phidget.PhidgetID.ROTARY_TOUCH))
                        {
                            FSensorOut[i] = Convert.ToDouble(FPhidgetMain.FPhidget.sensors[i].RawValue) / 4095;
                            FDataRateMinOut[i] = FPhidgetMain.FPhidget.sensors[i].DataRateMin;
                            FDataRateMaxOut[i] = FPhidgetMain.FPhidget.sensors[i].DataRateMax;
                        }
                        else
                        {
                            FSensorOut[i] = Convert.ToDouble(FPhidgetMain.FPhidget.sensors[i].Value) / 1000;
                        }
                    }

                    FFirstTime = false;
                }
                else
                {
                    FSensorOut.SliceCount = 0;
                    FDigitalOut.SliceCount = 0;
                    FFirstTime = true;
                }

                FAttached[0] = FPhidgetMain.Attached;

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
