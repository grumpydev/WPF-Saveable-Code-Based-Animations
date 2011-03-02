namespace AnimationTestingPartDeux.Animation
{
    using System;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Configuration for an animation
    /// </summary>
    [Serializable]
    public class AnimationConfiguration
    {
        /// <summary>
        /// Gets or sets the duration of the storyboard animation
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the animation repeat behaviour
        /// </summary>
        public AnimationRepeatBehavior RepeatBehavior { get; set; }

        /// <summary>
        /// Gets or sets the animation acceleration ratio
        /// </summary>
        public double AccelerationRatio { get; set; }

        /// <summary>
        /// Gets or sets the animation deceleration ratio
        /// </summary>
        public double DecelerationRatio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation should auto-reverse
        /// </summary>
        public bool AutoReverse { get; set; }
    }
}