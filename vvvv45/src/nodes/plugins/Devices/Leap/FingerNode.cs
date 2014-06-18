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
    [PluginInfo(Name = "Finger",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class FingerNode : IPluginEvaluate
    {
        #region fields & pins
#pragma warning disable 0649
        [Input("Hands")]
        IDiffSpread<HandList> FHandsListIn;

        [Output("Finger Position")]
        ISpread<Vector3D> FFingerPosOut;

        [Output("Finger Direction")]
        ISpread<Vector3D> FFingerDirOut;

        [Output("Finger Velocity", Visibility = PinVisibility.OnlyInspector)]
        ISpread<Vector3D> FFingerVelOut;

        [Output("Is Tool")]
        ISpread<bool> FFingerIsToolOut;

        [Output("Finger Size", Visibility = PinVisibility.OnlyInspector)]
        ISpread<Vector2D> FFingerSizeOut;

        [Output("Finger ID")]
        ISpread<int> FFingerIDOut;

        [Output("Hand Slice", Visibility = PinVisibility.OnlyInspector)]
        ISpread<int> FHandSliceOut;


#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FHandsListIn.IsChanged)
            {
                var hands = FHandsListIn[0];

                SpreadMax = hands.Count;

                FFingerPosOut.SliceCount = 0;
                FFingerDirOut.SliceCount = 0;
                FFingerVelOut.SliceCount = 0;
                FFingerIsToolOut.SliceCount = 0;
                FFingerSizeOut.SliceCount = 0;
                FFingerIDOut.SliceCount = 0;
                FHandSliceOut.SliceCount = 0;

                for (int i = 0; i < SpreadMax; i++)
                {

                    var pointables = hands[i].Pointables;
                    
                    for (int j = 0; j < pointables.Count; j++)
                    {
                        var pointable = pointables[j];
                        FFingerPosOut.Add(pointable.TipPosition.ToVector3DPos());
                        FFingerDirOut.Add(pointable.Direction.ToVector3DDir());
                        FFingerVelOut.Add(pointable.TipVelocity.ToVector3DPos());
                        FFingerIsToolOut.Add(pointable.IsTool);
                        FFingerSizeOut.Add(new Vector2D(pointable.Width * 0.001, pointable.Length * 0.001));
                        FFingerIDOut.Add(pointable.Id);
                        FHandSliceOut.Add(i);
                    }
                }
            }
        }
    }
}
