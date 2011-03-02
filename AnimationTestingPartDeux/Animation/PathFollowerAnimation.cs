namespace AnimationTestingPartDeux.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    [Serializable]
    public class PathFollowerAnimation : AnimationBase
    {
        /// <summary>
        /// Gets or sets the path geometry
        /// </summary>
        public string PathGeometry { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFollowerAnimation"/> class.
        /// </summary>
        /// <param name="pathGeometry">The path Geometry.</param>
        /// <param name="configuration">The configuration to use.</param>
        public PathFollowerAnimation(string pathGeometry, AnimationConfiguration configuration)
            : base(configuration)
        {
            if (string.IsNullOrWhiteSpace(pathGeometry))
            {
                throw new ArgumentException(@"pathGeometry must not be null or empty.", "pathGeometry");
            }

            this.PathGeometry = pathGeometry;
        }

        /// <summary>
        /// Create the animation and add it to the master timeline
        /// </summary>
        protected override void CreateAnimation()
        {
            var geometry = new PathGeometry();
            geometry.AddGeometry(Geometry.Parse("M 10,100 C 10,300 300,-200 300,100"));

            var animationX = new DoubleAnimationUsingPath
                {
                    PathGeometry = geometry,
                    Duration = new Duration(this.AnimationConfiguration.Duration),
                    Source = PathAnimationSource.X
                };

            var animationY = new DoubleAnimationUsingPath
            {
                PathGeometry = geometry,
                Duration = new Duration(this.AnimationConfiguration.Duration),
                Source = PathAnimationSource.Y
            };

            Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));

            animationX.Freeze();
            animationY.Freeze();

            this.MasterStoryboard.Children.Add(animationX);
            this.MasterStoryboard.Children.Add(animationY);
        }
    }
}