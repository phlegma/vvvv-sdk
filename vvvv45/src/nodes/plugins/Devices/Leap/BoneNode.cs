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
    [PluginInfo(Name = "Bone",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class BoneNode : IPluginEvaluate
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

                for (int i = 0; i < SpreadMax; i++)
                {
                    var Fingers = hands[i].Fingers;
                    foreach (Finger finger in Fingers)
                    {
                        Bone bone;
                        foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
                        {
                            bone = finger.Bone(boneType);
                            // ... Use bone
                        }
                    }
                }
            }
        }
    }
}
