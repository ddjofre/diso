using Shin_Megami_Tensei.Enumerates;

namespace Shin_Megami_Tensei.DamageCalculators;

public class DamageCalculatorFactory
{
    private static readonly Dictionary<TypeAttack, BaseDamageCalculator> _calculators = new()
    {
        { TypeAttack.Phys, new DamageCalculatorPhys() },
        { TypeAttack.Gun, new DamageCalculatorGun() },
        { TypeAttack.Fire, new DamageCalculatorMagic() },
        { TypeAttack.Ice, new DamageCalculatorMagic() },
        { TypeAttack.Elec, new DamageCalculatorMagic() },
        { TypeAttack.Force, new DamageCalculatorMagic() }
    };

    public BaseDamageCalculator GetDamageCalculator(TypeAttack typeAttack)
    {
        if (_calculators.TryGetValue(typeAttack, out var calculator))
        {
            return calculator;
        }
        
        throw new NotImplementedException($"No damage calculator implemented for attack type: {typeAttack}");
    }
    
}