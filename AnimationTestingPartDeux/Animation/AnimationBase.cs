namespace AnimationTestingPartDeux.Animation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Xml.Serialization;

    /// <summary>
    /// Animation base class for "code based" animations
    /// </summary>
    [Serializable]
    public abstract class AnimationBase : IAnimation
    {
        /// <summary>
        /// Current time for saving/restoring the timeline state
        /// </summary>
        private TimeSpan currentTime;

        /// <summary>
        /// The master storyboard for the animation.
        /// </summary>
        [NonSerialized]
        private Storyboard masterStoryboard;

        /// <summary>
        /// The bound object for animating
        /// </summary>
        [NonSerialized]
        private DependencyObject boundObject;

        /// <summary>
        /// Flag for whether the animation has been created
        /// </summary>
        [NonSerialized]
        private bool animationCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationBase"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        protected AnimationBase(AnimationConfiguration configuration)
        {
            this.CurrentAnimationState = AnimationState.Stopped;
            this.AnimationConfiguration = configuration;
        }

        /// <summary>
        /// Sets the bound object that the value will animate
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Private getter")]
        [XmlIgnore]
        public DependencyObject BoundObject
        {
            get
            {
                return this.boundObject;
            }

            set
            {
                this.DetachAnimation();
                this.boundObject = value;
                this.AttachAnimation();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the animation is currently bound
        /// </summary>
        [XmlIgnore]
        public bool IsBound
        {
            get
            {
                return this.BoundObject != null;
            }
        }

        /// <summary>
        /// Gets or sets the current animation state
        /// </summary>
        public AnimationState CurrentAnimationState { get; protected set; }

        /// <summary>
        /// Gets or sets the animation configuration
        /// </summary>
        public AnimationConfiguration AnimationConfiguration { get; protected set; }

        /// <summary>
        /// Gets the master storyboard for the animation
        /// </summary>
        protected Storyboard MasterStoryboard
        {
            get
            {
                return this.masterStoryboard;
            }

            private set
            {
                this.masterStoryboard = value;
            }
        }

        /// <summary>
        /// Plays the animation - resumes if paused, starts from the start if stopped.
        /// </summary>
        public virtual void PlayAnimation()
        {
            if (!this.IsBound)
            {
                return;
            }

            if (this.CurrentAnimationState == AnimationState.Paused)
            {
                this.MasterStoryboard.Resume();
            }
            else
            {
                this.BeginAnimation();
            }

            this.CurrentAnimationState = AnimationState.Playing;
        }

        /// <summary>
        /// Pause the animation - call PlayAnimation to resume.
        /// </summary>
        public virtual void PauseAnimation()
        {
            if (!this.IsBound)
            {
                return;
            }

            this.MasterStoryboard.Pause();

            this.CurrentAnimationState = AnimationState.Paused;
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public virtual void StopAnimation()
        {
            if (!this.IsBound)
            {
                return;
            }

            this.MasterStoryboard.Stop();

            this.CurrentAnimationState = AnimationState.Stopped;
        }

        /// <summary>
        /// "Detach" the animation from the bound object before it changes
        /// </summary>
        protected virtual void DetachAnimation()
        {
            if (this.BoundObject == null)
            {
                return;
            }

            this.InvokeOnBoundObjectDispatcherThread(
                () =>
                    {
                        this.StopAnimation();
                        Storyboard.SetTarget(this.MasterStoryboard, null);
                    });
        }

        /// <summary>
        /// "Attach" the animation to the bound object when it changes
        /// </summary>
        protected virtual void AttachAnimation()
        {
            if (!this.IsBound)
            {
                return;
            }

            this.InvokeOnBoundObjectDispatcherThread(
                () =>
                    {
                        if (!this.animationCreated)
                        {
                            this.CreateStoryboard();
                            this.CreateAnimation();

                            this.animationCreated = true;
                        }

                        Storyboard.SetTarget(this.MasterStoryboard, this.BoundObject);
                        this.MasterStoryboard.Freeze();
                        this.SyncCurrentState();
                    });
        }

        /// <summary>
        /// Create the animation and add it to the master timeline
        /// </summary>
        protected abstract void CreateAnimation();

        /// <summary>
        /// Saves the state of the animation
        /// </summary>
        protected virtual void SaveState()
        {
            this.currentTime = this.MasterStoryboard.GetCurrentTime();
        }

        /// <summary>
        /// Invokes an action on the given dispatcher thread.
        /// </summary>
        /// <param name="action">Action to execute</param>
        private void InvokeOnBoundObjectDispatcherThread(Action action)
        {
            if (!this.IsBound)
            {
                throw new InvalidOperationException("Unable to invoke on the bound object dispatcher thread when unbound.");
            }

            this.BoundObject.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Create the master storyboard
        /// </summary>
        private void CreateStoryboard()
        {
            this.MasterStoryboard = new Storyboard
                {
                    Duration = this.AnimationConfiguration.Duration,
                    RepeatBehavior = this.AnimationConfiguration.RepeatBehavior,
                    AccelerationRatio = this.AnimationConfiguration.AccelerationRatio,
                    DecelerationRatio = this.AnimationConfiguration.DecelerationRatio,
                    AutoReverse = this.AnimationConfiguration.AutoReverse
                };
        }

        /// <summary>
        /// Starts the animation from the beginning.
        /// </summary>
        private void BeginAnimation()
        {
            if (!this.IsBound)
            {
                return;
            }

            this.MasterStoryboard.Begin();

            if (this.currentTime != default(TimeSpan))
            {
                this.masterStoryboard.Seek(this.currentTime);
                this.currentTime = default(TimeSpan);
            }
        }

        /// <summary>
        /// Syncronises the storyboard with the current state
        /// </summary>
        private void SyncCurrentState()
        {
            switch (this.CurrentAnimationState)
            {
                case AnimationState.Playing:
                    this.PlayAnimation();
                    break;
                case AnimationState.Paused:
                    this.BeginAnimation();
                    this.PauseAnimation();
                    break;
                case AnimationState.Stopped:
                    break;
            }
        }

        /// <summary>
        /// Saves the timeline state on serialization
        /// </summary>
        /// <param name="context">Streaming context</param>
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            this.SaveState();
        }
    }
}