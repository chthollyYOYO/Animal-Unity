using Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class RecipeSystem : Singleton<RecipeSystem>
    {
        private Dictionary<int, List<(List<int>, int)>> recipes = new(); //<kitchenware, <inItem, outItem>>

        public void Initialize()
        {
            var cfgs = ConfigManager.GetItemCfgList();
            foreach (var cfg in cfgs)
            {
                if (cfg.Type == 3)
                {
                    List<int> inItems = cfg.UseIngredient.ToList();
                    inItems.Sort();

                    if (!this.recipes.TryGetValue(cfg.UseKitchenware, out var recipes))
                        recipes = this.recipes[cfg.UseKitchenware] = new();
                    recipes.AddOrInsert((inItems, cfg.Id), p => inItems[0] <= p.Item1[0]);
                }
            }
        }

        public int GetOutItem(int[] items, int kitchenware)
        {
            var inItems = items.ToList();
            inItems.RemoveAll(id => id == 0);
            if (inItems.Count > 0)
            {
                inItems.Sort();
                if (this.recipes.TryGetValue(kitchenware, out var recipes))
                {
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        if (recipes[i].Item1[0] > inItems[0])
                            break;

                        if (recipes[i].Item1.SequenceEqual(inItems))
                        {
                            int outItem = recipes[i].Item2;
                            if (Record.current.obtainedRecipe.Contains(outItem))
                                return outItem;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
