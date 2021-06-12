using System.Collections.Generic;

namespace RPG.Stats
{
    interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stats stat);
        IEnumerable<float> GetPercentageModifier(Stats stat);
    }
}