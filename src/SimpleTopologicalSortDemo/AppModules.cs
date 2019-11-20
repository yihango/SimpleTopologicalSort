using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTopologicalSortDemo
{
    public interface IRModule
    {

    }


    public abstract class RModule : IRModule
    {

    }



    [DependModules(
        typeof(BModule)
        )]
    public class AModule : RModule
    {

    }

    [DependModules(
      typeof(CModule)
      )]
    public class BModule : RModule
    {

    }

    [DependModules(
        typeof(DModule)
    )]
    public class CModule : RModule
    {

    }

    public class DModule
    {

    }
}
