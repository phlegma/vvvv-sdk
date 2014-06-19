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
    [PluginInfo(Name = "InteractionBox",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap device",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class InteractionBoxNode : IPluginEvaluate
    {
        #region fields & pins
#pragma warning disable 0649
        [Input("Frame")]
        IDiffSpread<Frame> FFrameIn;

        [Output("Center")]
        ISpread<Vector3D> FCenterOut;

        [Output("Width")]
        ISpread<double> FWidthOut;

        [Output("Height")]
        ISpread<double> FHeightOut;

        [Output("Depth")]
        ISpread<double> FDepthOut;



#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FFrameIn.IsChanged)
            {
                if (FFrameIn[0] != null)
                {
                    var InteractionBox = FFrameIn[0].InteractionBox;
                    if (InteractionBox.IsValid)
                    {
                        FCenterOut[0] = InteractionBox.Center.ToVector3DPos();
                        FWidthOut[0] = InteractionBox.Width * 0.001;
                        FHeightOut[0] = InteractionBox.Height * 0.001;
                        FDepthOut[0] = InteractionBox.Depth * 0.001;
                    }
                }
            }
        }
    }
}
