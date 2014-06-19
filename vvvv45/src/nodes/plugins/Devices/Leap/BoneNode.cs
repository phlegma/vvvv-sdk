﻿#region usings
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
        [Input("Bone")]
        IDiffSpread<Bone> FBoneListIn;

        [Output("Name")]
        ISpread<string> FNameOut;



#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FBoneListIn.IsChanged)
            {
                FNameOut.SliceCount = SpreadMax;

                for (int i = 0; i < FBoneListIn.SliceCount; i++)
                {
                    FNameOut[i] = FBoneListIn[i].Type.ToString();
                }
            }
        }
    }
}
