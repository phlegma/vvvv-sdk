#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.Algorithm;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using Leap;

#endregion usings

namespace VVVV.Nodes.Devices
{
    #region PluginInfo
    [PluginInfo(Name = "Hand",
    Category = "Leap",
    Help = "Returns the hand data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class HandNode : IPluginEvaluate
    {
        #region fields & pins
        #pragma warning disable 0649
        [Input("Hand")]
        IDiffSpread<HandList> FHandIn;

        [Output("Finger")]
        ISpread<FingerList> FFingerListOut;

        [Output("Hand Position")]
        ISpread<Vector3D> FHandPosOut;

        [Output("Hand Direction")]
        ISpread<Vector3D> FHandDirOut;

        [Output("Hand Normal")]
        ISpread<Vector3D> FHandNormOut;

        [Output("Hand Width")]
        ISpread<float> FHandWidthOut;

        [Output("Pintch")]
        ISpread<float> FPintchOut;

        [Output("Confidence")]
        ISpread<float> FCondfidenceOut;

        [Output("Hand Ball Center", Visibility = PinVisibility.OnlyInspector)]
        ISpread<Vector3D> FHandBallCentOut;

        [Output("Hand Ball Radius", Visibility = PinVisibility.OnlyInspector)]
        ISpread<double> FHandBallRadOut;

        [Output("Hand Velocity", Visibility = PinVisibility.OnlyInspector)]
        ISpread<Vector3D> FHandVelOut;

        [Output("Hand Name", Visibility = PinVisibility.OnlyInspector)]
        ISpread<string> FHandNameOut;

        [Output("Duration", Visibility = PinVisibility.OnlyInspector)]
        ISpread<float> FDurationOut;

        [Output("Hand ID")]
        ISpread<int> FHandIDOut;


        #pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FHandIn.IsChanged)
            {
                if (FHandIn[0] != null)
                {
                    SpreadMax = FHandIn[0].Count;

                    FHandPosOut.SliceCount = SpreadMax;
                    FHandDirOut.SliceCount = SpreadMax;
                    FHandVelOut.SliceCount = SpreadMax;
                    FHandNormOut.SliceCount = SpreadMax;
                    FHandBallCentOut.SliceCount = SpreadMax;
                    FHandBallRadOut.SliceCount = SpreadMax;
                    FHandIDOut.SliceCount = SpreadMax;
                    FCondfidenceOut.SliceCount = SpreadMax;
                    FHandNameOut.SliceCount = SpreadMax;
                    FHandWidthOut.SliceCount = SpreadMax;
                    FPintchOut.SliceCount = SpreadMax;
                    FDurationOut.SliceCount = SpreadMax;
                    FFingerListOut.SliceCount = SpreadMax;



                    for (int i = 0; i < SpreadMax; i++)
                    {
                        var hand = FHandIn[0][i];

                        if (hand != null && hand.IsValid)
                        {
                            FHandPosOut[i] = hand.PalmPosition.ToVector3DPos();
                            FHandDirOut[i] = hand.Direction.ToVector3DDir();
                            FHandNormOut[i] = hand.PalmNormal.ToVector3DDir();
                            FHandBallCentOut[i] = hand.SphereCenter.ToVector3DPos();
                            FHandBallRadOut[i] = hand.SphereRadius * 0.001;
                            FHandVelOut[i] = hand.PalmVelocity.ToVector3DPos();
                            FHandIDOut[i] = hand.Id;
                            FCondfidenceOut[i] = hand.Confidence;
                            FHandNameOut[i] = hand.IsLeft ? "Left hand" : "Right hand";
                            FHandWidthOut[i] = hand.PalmWidth;
                            FPintchOut[i] = hand.PinchStrength;
                            FDurationOut[i] = hand.TimeVisible;

                            FFingerListOut[i] = hand.Fingers;
                        }
                    }
                }
            }
        }
    }
}
		