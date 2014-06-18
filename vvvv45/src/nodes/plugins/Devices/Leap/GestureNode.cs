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

    public abstract class LeapSplitGestureNodeBase : IPluginEvaluate
    {
        #region fields & pins
#pragma warning disable 0649

        //screen tab
        [Input("Gesture List")]
        IDiffSpread<GestureList> FGestureListIn;

        [Output("ID")]
        ISpread<int> FIDOut;

        [Output("Duration")]
        ISpread<float> FDurationOut;

        [Output("State")]
        ISpread<Gesture.GestureState> FStateOut;

#pragma warning restore

        #endregion fields & pins

        public void Evaluate(int SpreadMax)
        {
            if (FGestureListIn.IsChanged)
            {
                ClearPins();
                var list = FGestureListIn[0];
                if (list == null) return;

                for (int i = 0; i < list.Count; i++)
                {
                    var gesture = list[i];
                    if (CheckGesture(gesture))
                    {
                        FIDOut.Add(gesture.Id);
                        FDurationOut.Add(gesture.DurationSeconds);
                        FStateOut.Add(gesture.State);
                        SplitGesture(gesture);
                    }
                }
            }
        }

        protected virtual void ClearPins()
        {
            FIDOut.SliceCount = 0;
            FDurationOut.SliceCount = 0;
            FStateOut.SliceCount = 0;
        }

        protected abstract bool CheckGesture(Gesture gesture);
        protected abstract void SplitGesture(Gesture gesture);
    }

    #region PluginInfo
    [PluginInfo(Name = "CircleGesture",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap circle gesture",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class LeapSplitCircleGestureNode : LeapSplitGestureNodeBase
    {
        [Output("Center")]
        public ISpread<Vector3D> FCenterOut;

        [Output("Normal")]
        public ISpread<Vector3D> FNormalOut;

        [Output("Radius")]
        public ISpread<float> FRadiusOut;

        [Output("Progress")]
        public ISpread<float> FProgressOut;

        protected override void SplitGesture(Gesture gesture)
        {
            var g = new CircleGesture(gesture);
            FCenterOut.Add(g.Center.ToVector3DPos());
            FNormalOut.Add(g.Normal.ToVector3DDir());
            FRadiusOut.Add(g.Radius * 0.001f);
            FProgressOut.Add(g.Progress);
        }

        protected override void ClearPins()
        {
            base.ClearPins();

            FCenterOut.SliceCount = 0;
            FNormalOut.SliceCount = 0;
            FRadiusOut.SliceCount = 0;
            FProgressOut.SliceCount = 0;
        }

        protected override bool CheckGesture(Gesture gesture)
        {
            return gesture.Type == Gesture.GestureType.TYPECIRCLE;
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "SwipeGesture",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap swipe gesture",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class LeapSplitSwipeGestureNode : LeapSplitGestureNodeBase
    {
        [Output("Direction")]
        public ISpread<Vector3D> FDirectinOut;

        [Output("Position")]
        public ISpread<Vector3D> FPositionOut;

        [Output("Speed")]
        public ISpread<float> FSpeedOut;

        [Output("Start Position")]
        public ISpread<Vector3D> FStartPositionOut;

        protected override void SplitGesture(Gesture gesture)
        {
            var g = new SwipeGesture(gesture);
            FDirectinOut.Add(g.Direction.ToVector3DDir());
            FPositionOut.Add(g.Position.ToVector3DPos());
            FSpeedOut.Add(g.Speed * 0.001f);
            FStartPositionOut.Add(g.StartPosition.ToVector3DPos());
        }

        protected override void ClearPins()
        {
            base.ClearPins();

            FDirectinOut.SliceCount = 0;
            FPositionOut.SliceCount = 0;
            FSpeedOut.SliceCount = 0;
            FStartPositionOut.SliceCount = 0;
        }

        protected override bool CheckGesture(Gesture gesture)
        {
            return gesture.Type == Gesture.GestureType.TYPESWIPE;
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "KeyTabGesture",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap key tab gesture",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class LeapSplitKeyTabGestureNode : LeapSplitGestureNodeBase
    {
        [Output("Direction")]
        public ISpread<Vector3D> FDirectinOut;

        [Output("Position")]
        public ISpread<Vector3D> FPositionOut;

        [Output("Progress")]
        public ISpread<float> FProgressOut;

        protected override void SplitGesture(Gesture gesture)
        {
            var g = new KeyTapGesture(gesture);
            FDirectinOut.Add(g.Direction.ToVector3DDir());
            FPositionOut.Add(g.Position.ToVector3DPos());
            FProgressOut.Add(g.Progress);
        }

        protected override void ClearPins()
        {
            base.ClearPins();

            FDirectinOut.SliceCount = 0;
            FPositionOut.SliceCount = 0;
            FProgressOut.SliceCount = 0;
        }

        protected override bool CheckGesture(Gesture gesture)
        {
            return gesture.Type == Gesture.GestureType.TYPEKEYTAP;
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "ScreenTabGesture",
    Category = "Leap",
    Help = "Returns the tracking data of the Leap screen tab gesture",
    Tags = "tracking, hand, finger",
    AutoEvaluate = true)]
    #endregion PluginInfo
    public class LeapSplitScreenTabGestureNode : LeapSplitGestureNodeBase
    {
        [Output("Direction")]
        public ISpread<Vector3D> FDirectinOut;

        [Output("Position")]
        public ISpread<Vector3D> FPositionOut;

        [Output("Progress")]
        public ISpread<float> FProgressOut;

        protected override void SplitGesture(Gesture gesture)
        {
            var g = new KeyTapGesture(gesture);
            FDirectinOut.Add(g.Direction.ToVector3DDir());
            FPositionOut.Add(g.Position.ToVector3DPos());
            FProgressOut.Add(g.Progress);
        }

        protected override void ClearPins()
        {
            base.ClearPins();

            FDirectinOut.SliceCount = 0;
            FPositionOut.SliceCount = 0;
            FProgressOut.SliceCount = 0;
        }

        protected override bool CheckGesture(Gesture gesture)
        {
            return gesture.Type == Gesture.GestureType.TYPESCREENTAP;
        }
    }
}
