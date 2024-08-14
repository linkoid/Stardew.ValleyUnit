using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkoid.Stardew.ValleyUnit.Assertions;

internal class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}
