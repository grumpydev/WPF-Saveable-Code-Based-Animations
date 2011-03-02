namespace AnimationTestingPartDeux.Animation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Repeat behavior for an animation
    /// </summary>
    [Serializable]
    public sealed class AnimationRepeatBehavior
    {
        /// <summary>
        /// What mode the behavior uses
        /// </summary>
        private RepeatMode repeatMode;

        /// <summary>
        /// The duration if in Timed mode
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// The number of iterations if in Numbered mode
        /// </summary>
        private double iterations;

        /// <summary>
        /// Repeat mode
        /// </summary>
        private enum RepeatMode
        {
            /// <summary>
            /// Run for a number of iterations
            /// </summary>
            Numbered,

            /// <summary>
            /// Run for a time
            /// </summary>
            Timed,

            /// <summary>
            /// Run forever
            /// </summary>
            Forever
        }

        /// <summary>
        /// Gets a constantly repeating repeat behavior
        /// </summary>
        public static AnimationRepeatBehavior Forever
        {
            get
            {
                return new AnimationRepeatBehavior { repeatMode = RepeatMode.Forever };
            }
        }

        /// <summary>
        /// Gets a constantly repeating repeat behavior
        /// </summary>
        public static AnimationRepeatBehavior Once
        {
            get
            {
                return new AnimationRepeatBehavior { repeatMode = RepeatMode.Numbered, iterations = 1 };
            }
        }

        /// <summary>
        /// Get a timed repeat behavior
        /// </summary>
        /// <param name="time">Time to run for</param>
        /// <returns>Repeat behavior</returns>
        public static AnimationRepeatBehavior Timed(TimeSpan time)
        {
            return new AnimationRepeatBehavior { repeatMode = RepeatMode.Timed, duration = time };
        }

        /// <summary>
        /// Get a repeat behaviour for a number of iterations
        /// </summary>
        /// <param name="iterations">Iterations to run for</param>
        /// <returns>Repeat behavior</returns>
        public static AnimationRepeatBehavior Iterations(double iterations)
        {
            return new AnimationRepeatBehavior { repeatMode = RepeatMode.Numbered, iterations = iterations };
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator.")]
        public static implicit operator RepeatBehavior(AnimationRepeatBehavior behavior)
        {
            switch (behavior.repeatMode)
            {
                case RepeatMode.Forever:
                    return RepeatBehavior.Forever;
                case RepeatMode.Timed:
                    return new RepeatBehavior(behavior.duration);
                case RepeatMode.Numbered:
                    return new RepeatBehavior(behavior.iterations);
            }

            return RepeatBehavior.Forever;
        }
    }
}