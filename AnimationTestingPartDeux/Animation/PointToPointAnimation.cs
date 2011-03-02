namespace AnimationTestingPartDeux.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    /// <summary>
    /// A basic point to point animation.
    /// </summary>
    [Serializable]
    public class PointToPointAnimation : AnimationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointToPointAnimation"/> class.
        /// </summary>
        /// <param name="startX">the starting X coordinate</param>
        /// <param name="startY">the starting Y coordinate</param>
        /// <param name="endX">the ending X coordinate</param>
        /// <param name="endY">the ending Y coordinate</param>
        /// <param name="configuration">the animation configuration</param>
        public PointToPointAnimation(double startX, double startY, double endX, double endY, AnimationConfiguration configuration)
            : base(configuration)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.EndX = endX;
            this.EndY = endY;
        }

        /// <summary>
        /// Gets or sets the starting X coordinate
        /// </summary>
        public double StartX { get; set; }

        /// <summary>
        /// Gets or sets the starting Y coordinate
        /// </summary>
        public double StartY { get; set; }

        /// <summary>
        /// Gets or sets the ending X coordinate
        /// </summary>
        public double EndX { get; set; }

        /// <summary>
        /// Gets or sets the ending Y coordinate
        /// </summary>
        public double EndY { get; set; }

        /// <summary>
        /// Create the animation and add it to the master timeline
        /// </summary>
        protected override void CreateAnimation()
        {
            var animationDuration = new Duration(this.AnimationConfiguration.Duration);

            this.MasterStoryboard.Duration = animationDuration;

            var animationX = new DoubleAnimation(this.StartX, this.EndX, animationDuration);
            var animationY = new DoubleAnimation(this.StartY, this.EndY, animationDuration);

            Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));

            animationX.Freeze();
            animationY.Freeze();

            this.MasterStoryboard.Children.Add(animationX);
            this.MasterStoryboard.Children.Add(animationY);
        }
    }
}