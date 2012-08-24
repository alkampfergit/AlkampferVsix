// Guids.cs
// MUST match guids.h
using System;

namespace Alkampfer.AlkampferVsix2012
{
    static class GuidList
    {
        public const string guidAlkampferVsix2012PkgString = "868cb1b6-5266-490f-84b4-b5cd9571fd06";
        public const string guidAlkampferVsix2012CmdSetString = "7f651aa3-36b5-4a1d-a29c-7b78b0cf3446";

        public static readonly Guid guidAlkampferVsix2012CmdSet = new Guid(guidAlkampferVsix2012CmdSetString);
    };
}