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
    Help = "Returns the bone data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class BoneNode : IPluginEvaluate
    {
        #region fields & pins
#pragma warning disable 0649
        [Input("Bone")]
        IDiffSpread<Bone> FBoneListIn;

        [Output("Name")]
        ISpread<string> FNameOut;

        [Output("Size")]
        ISpread<Vector2D> FSizeOut;

        [Output("Position")]
        ISpread<Vector3D> FPostionOut;

        [Output("Direction")]
        ISpread<Vector3D> FDirectionOut;

        [Output("Previous Joint")]
        ISpread<Vector3D> FPrevJointOut;

        [Output("Next Joint")]
        ISpread<Vector3D> FNextJointOut;



#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FBoneListIn.IsChanged)
            {
                FNameOut.SliceCount = 0;
                FSizeOut.SliceCount = 0;
                FPostionOut.SliceCount = 0;
                FDirectionOut.SliceCount = 0;
                FPrevJointOut.SliceCount = 0;
                FNextJointOut.SliceCount = 0;

                if (FBoneListIn != null)
                {

                    for (int i = 0; i < FBoneListIn.SliceCount; i++)
                    {
                        Bone Bone = FBoneListIn[i];
                        FNameOut.Add(Bone.Type.ToString());
                        FSizeOut.Add(new Vector2D(Bone.Width * 0.001, Bone.Length * 0.001));
                        FPostionOut.Add(Bone.Center.ToVector3DPos());
                        FDirectionOut.Add(Bone.Direction.ToVector3DDir());
                        FPrevJointOut.Add(Bone.PrevJoint.ToVector3DPos());
                        FNextJointOut.Add(Bone.NextJoint.ToVector3DPos());
                    }
                }
            }
        }
    }
}
