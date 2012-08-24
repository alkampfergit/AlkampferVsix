using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.AlkampferVsix.OutputFormatting
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;
    using System.Windows.Media;

    public class OutputClassifier: IClassifier
    {
        IClassificationTypeRegistryService _classificationTypeRegistry;

        internal OutputClassifier(IClassificationTypeRegistryService registry)
        {
            this._classificationTypeRegistry = registry;
        }

        #region Public Events

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        #endregion 

        #region Public Methods

        /// <summary>
        /// Classify the given spans
        /// </summary>
        /// <param name="span">The span of interest in this projection buffer.</param>
        /// <returns>The list of <see cref="ClassificationSpan"/> as contributed by the source buffers.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            ITextSnapshot snapshot = span.Snapshot;

            List<ClassificationSpan> spans = new List<ClassificationSpan>();

            if (snapshot.Length == 0)
                return spans;

            var text = span.GetText();

            if (text.StartsWith("System.Windows.Data Error", StringComparison.OrdinalIgnoreCase))
            {
                IClassificationType type = _classificationTypeRegistry.GetClassificationType("output.wpfbindingalert");
                spans.Add(new ClassificationSpan(span, type));
            }
            else if (text.IndexOf("error ", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                //error followed by a space is the typical error of the build.
                IClassificationType type = _classificationTypeRegistry.GetClassificationType("output.alert");
                spans.Add(new ClassificationSpan(span, type));
            }
            else if (text.IndexOf("INFO:", StringComparison.OrdinalIgnoreCase) >= 0) 
            {
                //error followed by a space is the typical error of the build.
                IClassificationType type = _classificationTypeRegistry.GetClassificationType("output.info");
                spans.Add(new ClassificationSpan(span, type));
            }

            return spans;
        }


        #endregion // Public Methods

        #region Classification Type Definitions

        [Export]
        [Name("output.alert")]
        internal static ClassificationTypeDefinition outputAlertDefinition = null;

        [Export]
        [Name("output.info")]
        internal static ClassificationTypeDefinition outputInfoDefinition = null;

        [Export]
        [Name("output.wpfbindingalert")]
        internal static ClassificationTypeDefinition outputWpfBindingAlertDefinition = null;

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "output.alert")]
        [Name("output.alert")]
        internal sealed class OutputAlertFormat : ClassificationFormatDefinition
        {
            public OutputAlertFormat()
            {
                this.ForegroundColor = Colors.Red;
                this.IsBold = true;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "output.info")]
        [Name("output.info")]
        internal sealed class OutputInfoFormat : ClassificationFormatDefinition
        {
            public OutputInfoFormat()
            {
                this.ForegroundColor =  Colors.Blue;
                this.IsBold = true;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "output.wpfbindingalert")]
        [Name("output.wpfbindingalert")]
        internal sealed class OutputWpfBindingAlertFormat : ClassificationFormatDefinition
        {
            public OutputWpfBindingAlertFormat()
            {
                this.ForegroundColor = Colors.DarkRed;
                this.IsItalic = true;
                this.IsBold = true;
                this.BackgroundColor = Colors.LightGray;
            }
        }

        #endregion
    }
}
