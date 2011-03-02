namespace AnimationTestingPartDeux.Animation
{
    using System.Windows;

    /// <summary>
    /// Represents a "code based" animation
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Sets the bound object that the value will animate
        /// </summary>
        DependencyObject BoundObject { get;  set; }

        /// <summary>
        /// Gets a value indicating whether the animation is currently bound
        /// </summary>
        bool IsBound { get; }

        /// <summary>
        /// Gets a value indicating the current animation state
        /// </summary>
        AnimationState CurrentAnimationState { get; }

        /// <summary>
        /// Plays the animation - resumes if paused, starts from the start if stopped.
        /// </summary>
        void PlayAnimation();

        /// <summary>
        /// Pause the animation - call PlayAnimation to resume.
        /// </summary>
        void PauseAnimation();

        /// <summary>
        /// Stops the animation
        /// </summary>
        void StopAnimation();
    }
}