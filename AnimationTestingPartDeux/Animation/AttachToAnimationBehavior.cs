namespace AnimationTestingPartDeux.Animation
{
    using System.Windows;
    using System.Windows.Interactivity;

    public class AttachToAnimationBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Optional source object for attaching to the adapter in place of the AssocitatedObject
        /// </summary>
        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(DependencyObject), typeof(AttachToAnimationBehavior), new UIPropertyMetadata(null));

        /// <summary>
        /// Optional source object for attaching to the adapter in place of the AssocitatedObject
        /// </summary>
        public DependencyObject SourceObject
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (DependencyObject)GetValue(SourceObjectProperty);
            }

            set
            {
                SetValue(SourceObjectProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            var animatableViewModel = this.AssociatedObject.DataContext as IAnimatable;
            if (animatableViewModel == null)
            {
                return;
            }

            animatableViewModel.Animation.BoundObject = this.SourceObject ?? this.AssociatedObject;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var animatableViewModel = this.AssociatedObject.DataContext as IAnimatable;
            if (animatableViewModel == null)
            {
                return;
            }

            animatableViewModel.Animation.BoundObject = null;
        }
    }
}
