using System.Collections.Generic;
using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;
using VRage.Game;

namespace ProjectorFix
{
    [PatchShim]
    public static class ProjectorFixPatch
    {
        private static readonly MethodInfo NewBlueprintMethod =
            typeof(MyProjectorBase).GetMethod("OnNewBlueprintSuccess", BindingFlags.NonPublic | BindingFlags.Instance);

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void Patch(PatchContext ctx)
        {
            Log.Info("Patch init");
            ctx.GetPattern(typeof(MyProjectorBase).GetMethod("OnNewBlueprintSuccess",
                BindingFlags.Instance | BindingFlags.NonPublic)).Prefixes.Add(
                typeof(ProjectorFixPatch).GetMethod(nameof(PrefixNewBlueprint),
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic));
        }


        private static void PrefixNewBlueprint(MyProjectorBase __instance,
            ref List<MyObjectBuilder_CubeGrid> projectedGrids)
        {
            var proj = __instance;
            if (proj == null)
            {
                Log.Warn("No projector?");
            }

            var grid = projectedGrids[0];

            RemoveStockPile(grid);
        }

        private static void RemoveStockPile(MyObjectBuilder_CubeGrid grid)
        {
            if (grid == null)
            {
                return;
            }
            var projectedBlocks = grid.CubeBlocks;

            foreach (var block in projectedBlocks)
            {
                if(block is MyObjectBuilder_ProjectorBase)
                {
                    var myObjectBuilderCubeGrid = ((MyObjectBuilder_ProjectorBase) block).ProjectedGrids;
                    foreach (var objectBuilderCubeGrid in myObjectBuilderCubeGrid)
                    {
                        RemoveStockPile(objectBuilderCubeGrid);
                    }
                }
                if (block.ConstructionStockpile == null || block.ConstructionStockpile.Items.Length == 0)
                {
                    continue;
                }
                block.ConstructionStockpile = null;
            }
        }
    }
}