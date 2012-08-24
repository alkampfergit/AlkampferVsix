using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.AlkampferVsix.OutputFormatting
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IClassifierProvider))]
    [ContentType("output")]
    public class OutputClassifierProvider : IClassifierProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        static OutputClassifier outputClassifier;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return outputClassifier ?? (outputClassifier = new OutputClassifier(ClassificationRegistry));
        }
    }
}
