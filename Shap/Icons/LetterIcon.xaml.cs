namespace Shap.Icons
{
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Windows;


    /// <summary>
    /// Interaction logic for LetterIcon.xaml
    /// </summary>
    public partial class LetterIcon : UserControl
    {
        /// <summary>
        /// Used to set the day number
        /// </summary>
        public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.Register(
              "Character",
              typeof(string),
              typeof(LetterIcon),
              new PropertyMetadata("0"));

        /// <summary>
        /// Used to highlight this icon
        /// </summary>
        public static readonly DependencyProperty IconSelectedBrushProperty =
            DependencyProperty.Register(
              "IconSelectedBrush",
              typeof(Brush),
              typeof(LetterIcon),
              new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Used to set the background of this icon.
        /// </summary>
        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(
              "BackgroundBrush",
              typeof(Brush),
              typeof(LetterIcon),
              new PropertyMetadata(Brushes.Blue));

        /// <summary>
        /// Initialises a new instance of the <see cref="LetterIcon"/> class.
        /// </summary>
        public LetterIcon()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Gets or sets the character value
        /// </summary>
        public string Character
        {
            get
            {
                return (string)GetValue(CharacterProperty);
            }

            set
            {
                SetValue(CharacterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the brush used to indicate if this <see cref="LetterIcon"/> is currently selected.
        /// </summary>
        public Brush IconSelectedBrush
        {
            get
            {
                return (Brush)GetValue(IconSelectedBrushProperty);
            }

            set
            {
                SetValue(IconSelectedBrushProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the colour used for the background of this <see cref="LetterIcon"/>.
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                return (Brush)GetValue(BackgroundBrushProperty);
            }

            set
            {
                SetValue(BackgroundBrushProperty, value);
            }
        }
    }


}
