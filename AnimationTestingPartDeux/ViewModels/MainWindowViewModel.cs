namespace AnimationTesting.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using AnimationTestingPartDeux.Animation;

    using IAnimatable = AnimationTestingPartDeux.Animation.IAnimatable;

    public class MainWindowViewModel : INotifyPropertyChanged, IAnimatable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IAnimation Animation { get; private set; }

        private int currentAnimation = 0;
        public int CurrentAnimation
        {
            get
            {
                return this.currentAnimation;
            }

            set
            {
                if (this.currentAnimation == value)
                {
                    return;
                }

                this.currentAnimation = value;

                this.OnPropertyChanged("CurrentAnimation");

                this.SetAnimation(value);
            }
        }

        private double x;
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                if (this.x == value)
                {
                    return;
                }

                this.x = value;

                this.OnPropertyChanged("X");
            }
        }

        private double y;
        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                if (this.y == value)
                {
                    return;
                }

                this.y = value;

                this.OnPropertyChanged("Y");
            }
        }

        private double angle;

        private byte[] animationState;

        private readonly AnimationConfiguration animationConfiguration = new AnimationConfiguration
            {
                Duration = new TimeSpan(0, 0, 0, 2),
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.3,
                AutoReverse = true,
                RepeatBehavior = AnimationRepeatBehavior.Forever
            };

        public double Angle
        {
            get
            {
                return this.angle;
            }

            set
            {
                if (this.angle == value)
                {
                    return;
                }

                this.angle = value;

                this.OnPropertyChanged("Angle");
            }
        }

        public ICommand SaveStateCommand { get; private set; }

        public ICommand RestoreStateCommand { get; private set; }

        public ICommand AnimationCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.X = this.Y = 200;

            this.SaveStateCommand = new BasicCommand(this.SaveAnimationState);
            this.RestoreStateCommand = new BasicCommand(this.RestoreAnimationState);
            this.AnimationCommand = new BasicCommand(this.HandleStartStopPause);

            this.SetAnimation(this.CurrentAnimation);
        }

        private void SaveAnimationState(object parameter)
        {
            // Use a background thread to make sure we can handle being 
            // serialized on a different thread
            ThreadPool.QueueUserWorkItem(
                (s) =>
                {
                    using (var objectStream = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(objectStream, this.Animation);

                        this.animationState = objectStream.GetBuffer();
                    }

                    this.Animation.StopAnimation();
                });
        }

        private void RestoreAnimationState(object parameter)
        {
            if (this.animationState == null)
            {
                return;
            }

            // Normally we wouldn't need to do this because the view would be 
            // recreated and the behaviour would kick in.
            var oldBoundObject = this.Animation.BoundObject;

            // Use a background thread to make sure we can handle being 
            // restored on a different thread
            ThreadPool.QueueUserWorkItem(
                (s) =>
                {
                    using (var objectStream = new MemoryStream(this.animationState))
                    {
                        var formatter = new BinaryFormatter();
                        this.Animation = (IAnimation)formatter.Deserialize(objectStream);
                    }

                    this.Animation.BoundObject = oldBoundObject;
                });
        }

        private void SetAnimation(int value)
        {
            DependencyObject tempObject = null;
            this.animationState = null;

            if (this.Animation != null)
            {
                this.Animation.StopAnimation();
                tempObject = this.Animation.BoundObject;
            }

            switch (value)
            {
                case 0:
                    this.Animation = new PointToPointAnimation(50, 50, 200, 200, this.animationConfiguration);
                    break;
                case 1:
                    this.Animation = new PathFollowerAnimation("M 10,100 C 10,300 300,-200 300,100", this.animationConfiguration);
                    break;
            }

            if (this.Animation != null)
            {
                this.Animation.BoundObject = tempObject;
            }
        }

        private void HandleStartStopPause(object parameter)
        {
            var commandParameter = parameter as string;

            if (String.IsNullOrEmpty(commandParameter))
            {
                return;
            }

            switch (commandParameter.ToLower())
            {
                case "play":
                    this.Animation.PlayAnimation();
                    break;
                case "pause":
                    this.Animation.PauseAnimation();
                    break;
                case "stop":
                    this.Animation.StopAnimation();
                    break;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
