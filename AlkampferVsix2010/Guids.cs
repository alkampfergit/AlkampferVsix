// Guids.cs
// MUST match guids.h
using System;

namespace Alkampfer.AlkampferVsix2010
{
    static class GuidList
    {
        public const string guidAlkampferVsix2010PkgString = "bb8e08f1-6ca8-41fa-a9f9-39cbac3485f6";
        public const string guidAlkampferVsix2010CmdSetString = "c78a89df-6115-4fd7-9186-7b7c04d8d1c6";

        public static readonly Guid guidAlkampferVsix2010CmdSet = new Guid(guidAlkampferVsix2010CmdSetString);
    };
}