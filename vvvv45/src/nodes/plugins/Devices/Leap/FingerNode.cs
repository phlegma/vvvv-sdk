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
    Help = "Returns the finger data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class FingerNode : IPluginEvaluate
    {
        #region fields & pins
#pragma warning disable 0649
        [Input("Finger")]
        IDiffSpread<FingerList> FFingerListIn;

        [Output("Bone")]
        ISpread<Bone> FBoneOut;

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

        [Output("Extended", Visibility = PinVisibility.OnlyInspector)]
        ISpread<bool> FExtendedOut;

        [Output("Name", Visibility = PinVisibility.OnlyInspector)]
        ISpread<string> FNameOut;


        List<Bone> Bones = new List<Bone>();
#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FFingerListIn.IsChanged)
            {

                FFingerPosOut.SliceCount = 0;
                FFingerDirOut.SliceCount = 0;
                FFingerVelOut.SliceCount = 0;
                FFingerIsToolOut.SliceCount = 0;
                FFingerSizeOut.SliceCount = 0;
                FFingerIDOut.SliceCount = 0;
                FHandSliceOut.SliceCount = 0;
                FExtendedOut.SliceCount = 0;
                FNameOut.SliceCount = 0;
                Bones.Clear();

                if (FFingerListIn != null)
                {
                    for (int i = 0; i < FFingerListIn.SliceCount; i++)
                    {
                        if (FFingerListIn[i] != null)
                        {
                            for (int j = 0; j < FFingerListIn[i].Count; j++)
                            {
                                var Finger = FFingerListIn[i][j];

                                if (Finger.IsValid)
                                {
                                    FFingerPosOut.Add(Finger.TipPosition.ToVector3DPos());
                                    FFingerDirOut.Add(Finger.Direction.ToVector3DDir());
                                    FFingerVelOut.Add(Finger.TipVelocity.ToVector3DPos());
                                    FFingerIsToolOut.Add(Finger.IsTool);
                                    FFingerSizeOut.Add(new Vector2D(Finger.Width * 0.001, Finger.Length * 0.001));
                                    FFingerIDOut.Add(Finger.Id);
                                    FHandSliceOut.Add(i);
                                    FExtendedOut.Add(Finger.IsExtended);
                                    FNameOut.Add(Finger.Type().ToString());

                                    foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
                                    {
                                        Bone Bone = Finger.Bone(boneType);
                                        if (Bone.IsValid)
                                            Bones.Add(Bone);
                                    }
                                }
                            }

                        }
                    }
                }

                FBoneOut.AssignFrom(Bones);
            }
        }
    }
}
